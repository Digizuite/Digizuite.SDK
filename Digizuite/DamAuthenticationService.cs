using System;
using System.Threading.Tasks;
using System.Timers;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class DamAuthenticationService : IDisposable, IDamAuthenticationService
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly Configuration _configuration;

        private readonly ILogger<DamAuthenticationService> _logger;
        private readonly Timer _renewalTimer;

        private string _accessKey;

        /// <summary>
        /// </summary>
        private DateTime _expirationTime;

        private int _memberId;

        public DamAuthenticationService(Configuration configuration, IHttpClientFactory clientFactory,
            ILogger<DamAuthenticationService> logger)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _logger = logger;

            Login(configuration.SystemUsername, configuration.SystemPassword);

            _renewalTimer = new Timer(configuration.AccessKeyDuration.TotalMilliseconds * 0.9);
            _renewalTimer.Elapsed += (sender, args) => { Login(configuration.SystemUsername, _configuration.SystemPassword); };
            _renewalTimer.Start();
            _renewalTimer.AutoReset = true;
        }

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
        /// <param name="forceNew">
        ///     If true, a new access key will be generated,
        ///     even if the old one is still considered valid
        /// </param>
        public async Task<string> GetAccessKey(bool forceNew = false)
        {
            _logger.LogTrace("Getting access key", nameof(forceNew), forceNew);
            if (forceNew || HasExpired)
            {
                _logger.LogTrace("Loading new access key", nameof(forceNew), forceNew, nameof(HasExpired), HasExpired);
                return await Login(_configuration.SystemUsername, _configuration.SystemPassword);
            }

            _logger.LogTrace("Reusing previous access key");
            return _accessKey;
        }

        /// <summary>
        ///     Gets the member id of the authenticated user
        /// </summary>
        /// <param name="forceNew"></param>
        /// <returns></returns>
        public async Task<int> GetMemberId(bool forceNew = false)
        {
            _logger.LogTrace("Getting memberId", nameof(forceNew), forceNew);
            if (forceNew || HasExpired)
            {
                _logger.LogTrace("Loading new member id", nameof(forceNew), forceNew, nameof(HasExpired), HasExpired);
                await Login(_configuration.SystemUsername, _configuration.SystemPassword);
            }

            _logger.LogTrace("Returning member id", nameof(_memberId), _memberId);
            return _memberId;
        }

        private async Task<string> Login(string username, string password)
        {
            var client = _clientFactory.GetRestClient();
            var request = new RestRequest("/dmm3bwsv3/ConnectService.js", DataFormat.Json);
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
                _logger.LogError("Authentication failed");
                throw new Exception("Request was unsuccessful");
            }

            var item = res.Items[0];
            _accessKey = item.AccessKey;
            _memberId = int.Parse(item.MemberId);
            _expirationTime = DateTime.Now.Add(_configuration.AccessKeyDuration);

            _logger.LogInformation("Authenticated successful");
            return _accessKey;
        }
    }
}