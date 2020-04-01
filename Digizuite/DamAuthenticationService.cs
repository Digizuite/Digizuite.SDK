using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Models;
using RestSharp;
using Timer = System.Timers.Timer;

namespace Digizuite
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class DamAuthenticationService : IDisposable, IDamAuthenticationService
    {
        private readonly IDamRestClient _restClient;

        private readonly IConfiguration _configuration;

        private readonly ILogger<DamAuthenticationService> _logger;
        private readonly Timer _renewalTimer;

        private string _accessKey;

        /// <summary>
        /// </summary>
        private DateTime _expirationTime;

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private int _memberId;

        public DamAuthenticationService(IConfiguration configuration, IDamRestClient restClient,
            ILogger<DamAuthenticationService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configuration.AccessKeyDuration.TotalMilliseconds <= 0) return;
            _renewalTimer = new Timer(configuration.AccessKeyDuration.TotalMilliseconds * 0.9);
            _renewalTimer.Elapsed += async (sender, args) =>
            {
                await Login(configuration.SystemUsername, _configuration.SystemPassword).ConfigureAwait(false);
            };
            _renewalTimer.Start();
            _renewalTimer.AutoReset = true;
        }

        /// <summary>
        ///     Indicates if the access key has expired completely
        /// </summary>
        private bool HasExpired => _expirationTime < DateTime.Now;

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
            return _accessKey;
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

            _logger.LogTrace("Returning member id", nameof(_memberId), _memberId);
            return _memberId;
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
                    _renewalTimer?.Dispose();
                    _lock?.Dispose();
                }

                _disposed = true;
            }
        }

        private async Task<string> Login(string username, string password)
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                _logger.LogTrace("Logging in", nameof(username), username, "PasswordLength", password.Length);

                // Hash the password if it has not already been md5'ed beforehand 
                if (!Regex.IsMatch(password, @"^[0-9a-fA-F]{32}$")) password = CalculateMD5Hash(password);

                var request = new RestRequest("ConnectService.js", DataFormat.Json);
                request.AddParameter("method", "CreateAccesskey");
                request.AddParameter("usertype", 2);
                request.AddParameter("useversionedmetadata", 0);
                request.AddParameter("page", 1);
                request.AddParameter("limit", 25);
                request.AddParameter("username", username);
                request.AddParameter("password", password);

                if (!string.IsNullOrWhiteSpace(_configuration.ConfigVersionId))
                {
                    request.AddParameter("configversionid", _configuration.ConfigVersionId);

                    request.AddParameter("dataversionid",
                        string.IsNullOrWhiteSpace(_configuration.DataVersionId)
                            ? _configuration.ConfigVersionId
                            : _configuration.DataVersionId);
                }

                var res = await _restClient.Execute<DigiResponse<AuthenticateResponse>>(Method.POST, request).ConfigureAwait(false);

                if (res.ErrorException != null)
                {
                    _logger.LogError(res.ErrorException, "Request failed", nameof(res.ErrorMessage), res.ErrorMessage);
                    throw new AuthenticationException("Network request failed", res.ErrorException);
                }
                
                if (!res.Data.Success)
                {
                    _logger.LogError("Authentication failed", "response", res.Content);
                    throw new AuthenticationException("Authentication failed");
                }

                var item = res.Data.Items[0];
                _accessKey = item.AccessKey;
                _memberId = int.Parse(item.MemberId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                _expirationTime = DateTime.Now.Add(_configuration.AccessKeyDuration);

                _logger.LogInformation("Authenticated successful");
                return _accessKey;
            }
            finally
            {
                _lock.Release();
            }
        }


#pragma warning disable CA5351
        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                var sb = new StringBuilder();
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < hash.Length; i++) sb.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));

                return sb.ToString();
            }
        }
#pragma warning restore CA5351
    }
}