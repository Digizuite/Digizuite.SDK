using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;
using Timer = System.Timers.Timer;

namespace Digizuite
{
    public class DamAuthenticationService : IDisposable, IDamAuthenticationService
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly IConfiguration _configuration;

        private readonly ILogger<DamAuthenticationService> _logger;
        private readonly Timer _renewalTimer;

        private string _accessKey;

        /// <summary>
        /// </summary>
        private DateTime _expirationTime;

        private int _memberId;

        public DamAuthenticationService(IConfiguration configuration, IHttpClientFactory clientFactory,
            ILogger<DamAuthenticationService> logger)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _logger = logger;

            _renewalTimer = new Timer(configuration.AccessKeyDuration.TotalMilliseconds * 0.9);
            _renewalTimer.Elapsed += async (sender, args) =>
            {
                await Login(configuration.SystemUsername, _configuration.SystemPassword);
            };
            _renewalTimer.Start();
            _renewalTimer.AutoReset = true;
        }

        private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        /// <summary>
        ///     Indicates if the access key has expired completely
        /// </summary>
        private bool HasExpired => _expirationTime < DateTime.Now;

        public void Dispose()
        {
            _renewalTimer?.Dispose();
        }

        /// <summary>
        ///     Gets the active access key for the system user
        /// </summary>
        public async Task<string> GetAccessKey()
        {
            _logger.LogTrace("Getting access key");
            if (HasExpired)
            {
                _logger.LogTrace("Loading new access key", nameof(HasExpired), HasExpired);
                return await Login(_configuration.SystemUsername, _configuration.SystemPassword);
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
                await Login(_configuration.SystemUsername, _configuration.SystemPassword);
            }

            _logger.LogTrace("Returning member id", nameof(_memberId), _memberId);
            return _memberId;
        }

        private async Task<string> Login(string username, string password)
        {
            await _lock.WaitAsync();
            try
            {
                _logger.LogTrace("Logging in", nameof(username), username, "PasswordLength", password.Length);

                // Hash the password if it has not already been md5'ed beforehand 
                if (!Regex.IsMatch(password, @"^[0-9a-fA-F]{36}$"))
                {
                    password = CalculateMD5Hash(password);
                }

                var client = _clientFactory.GetRestClient();
                var request = new RestRequest("ConnectService.js", DataFormat.Json);
                request.AddParameter("method", "CreateAccesskey");
                request.AddParameter("usertype", 2);
                request.AddParameter("useversionedmetadata", 0);
                request.AddParameter("page", 1);
                request.AddParameter("limit", 25);
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                request.MakeRequestDamSafe();

                var res = await client.PostAsync<DigiResponse<AuthenticateResponse>>(request);

                if (!res.Success)
                {
                    _logger.LogError("Authentication failed", "response", res);
                    throw new Exception("Request was unsuccessful");
                }

                var item = res.Items[0];
                _accessKey = item.AccessKey;
                _memberId = int.Parse(item.MemberId);
                _expirationTime = DateTime.Now.Add(_configuration.AccessKeyDuration);

                _logger.LogInformation("Authenticated successful");
                return _accessKey;
            }
            finally
            {
                _lock.Release();
            }
        }


        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}