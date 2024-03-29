using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Timer = System.Timers.Timer;

namespace Digizuite
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DamAuthenticationService : IDisposable, IDamAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly ILogger<DamAuthenticationService> _logger;
        
        private readonly Timer _renewalTimer;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private AccessKey? _accessKey;

        public DamAuthenticationService(IConfiguration configuration, ILogger<DamAuthenticationService> logger, ServiceHttpWrapper serviceHttpWrapper)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceHttpWrapper = serviceHttpWrapper ?? throw new ArgumentNullException(nameof(serviceHttpWrapper));


            _renewalTimer = new Timer();
            _renewalTimer.Elapsed += (_, _) => { Login(configuration.SystemUsername, configuration.SystemPassword).ConfigureAwait(false); };
            _renewalTimer.AutoReset = true;
            
#pragma warning disable 4014
            Login(configuration.SystemUsername, configuration.SystemPassword);
#pragma warning restore 4014
        }

        /// <summary>
        ///     Indicates if the access key has expired completely
        /// </summary>
        private bool HasExpired => _accessKey == null || _accessKey?.Expiration < DateTimeOffset.Now;

        /// <summary>
        ///     Gets the active access key for the system user
        /// </summary>
        public async Task<string> GetAccessKey()
        {
            _logger.LogTrace("Getting access key");
            if (HasExpired)
            {
                _logger.LogTrace("Loading new access key", nameof(HasExpired), HasExpired);
                return await Login(_configuration.SystemUsername, _configuration.SystemPassword).ConfigureAwait(false);
            }

            _logger.LogTrace("Reusing previous access key");
            return _accessKey!.Token;
        }

        public async Task<string> Impersonate(int memberId, AccessKeyOptions options)
        {
            var accessKey = await GetAccessKey();

            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new AuthenticationException("Authentication failed, not yet authenticated");    
            }
            var accessKeyImpersonated = await GenerateImpersonatedToken(accessKey, memberId, options).ConfigureAwait(false);

            return accessKeyImpersonated;
        }

        /// <summary>
        ///     Gets the member id of the authenticated user
        /// </summary>
        public async Task<int> GetMemberId()
        {
            _logger.LogTrace("Getting memberId");
            if (HasExpired)
            {
                _logger.LogTrace("Loading new member id", nameof(HasExpired), HasExpired);
                await Login(_configuration.SystemUsername, _configuration.SystemPassword).ConfigureAwait(false);
            }

            _logger.LogTrace("Returning member id", nameof(_accessKey.MemberId), _accessKey!.MemberId);
            return _accessKey.MemberId;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _renewalTimer.Dispose();
                    _lock.Dispose();
                }

                _disposed = true;
            }
        }

        
        private class LoginRequest
        {
            public string Username { get; }
            public string Password { get; }
            public PasswordEncoding PasswordEncoding { get; set; } = PasswordEncoding.Md5;
            public AccessKeyOptions? Options { get; set; }

            public LoginRequest(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        private class ImpersonationRequest
        {
            /// <summary>
            /// The id of the member to impersonate
            /// </summary>
            public int MemberId { get; set; }
        
            /// <summary>
            /// Any options to use when creating the access key
            /// </summary>
            public AccessKeyOptions? Options { get; set; }
        }
        
        private enum PasswordEncoding
        {
            Md5,
            PlainText
        }

        public class AccessKeyOptions
        {
            /// <summary>
            /// The config id to create the access key for
            /// </summary>
            public string? ConfigId { get; set; }
            /// <summary>
            /// The language id to create the access key for
            /// </summary>
            public int? LanguageId { get; set; }
        
            /// <summary>
            /// If the users language should be updated going forward
            /// </summary>
            public bool PersistLanguage { get; set; }
        
            /// <summary>
            /// How long the access key should be valid
            /// </summary>
            public TimeSpan? Duration { get; set; }
        }
        
        private async Task<string> Login(string username, string password,
            CancellationToken cancellationToken = default )
        {
            await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                _logger.LogTrace("Logging in", nameof(username), username, "PasswordLength", password.Length);

                var passwordEncoding = Regex.IsMatch(password, @"^[0-9a-fA-F]{32}$") switch
                {
                    true => PasswordEncoding.Md5,
                    false => PasswordEncoding.PlainText
                };
                
                var (client, request) =
                    _serviceHttpWrapper.GetClientAndRequest(ServiceType.LoginService, "/api/access-key");

                var body = new LoginRequest(username, password)
                {
                    PasswordEncoding = passwordEncoding
                };

                if (!string.IsNullOrWhiteSpace(_configuration.ConfigVersionId))
                {
                    body.Options = new AccessKeyOptions
                    {
                        ConfigId = _configuration.ConfigVersionId
                    };
                }

                request.AddJsonBody(body);
            
                var res = await client.PostAsync<AccessKey>(request, cancellationToken);

                if (!res.IsSuccessful)
                {
                    _logger.LogError("Authentication failed", nameof(res), res);
                    throw new AuthenticationException("Authentication failed", res.Exception);
                }

                _accessKey = res.Data;
                var duration = _accessKey!.Expiration - DateTimeOffset.Now;

                var totalMillis = duration.TotalMilliseconds * 0.9;
                if (totalMillis > int.MaxValue)
                {
                    totalMillis = int.MaxValue;
                }
                
                _renewalTimer.Interval = totalMillis;
                _renewalTimer.Start();

                _logger.LogInformation("Authenticated successful");
                return _accessKey.Token;
            }
            finally
            {
                _lock.Release();
            }
        }
        
        private async Task<string> GenerateImpersonatedToken(string accessKey, int memberId, AccessKeyOptions? options, 
            CancellationToken cancellationToken = default )
        {
            _logger.LogTrace("Getting Impersonated AccessKey", nameof(memberId), memberId, nameof(options), options);
            
            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LoginService, "/api/access-key/impersonate");

            var body = new ImpersonationRequest
            {
                MemberId = memberId,
                Options = options
            };
            request.AddJsonBody(body);
            request.AddAccessKey(accessKey);
            
            var res = await client.PostAsync<AccessKey>(request, cancellationToken);

            if (res.IsSuccessful) return res.Data!.Token;
            _logger.LogError("Impersonation failed", nameof(res), res);
            throw new AuthenticationException("impersonation failed", res.Exception);
        }
       
    }
}
