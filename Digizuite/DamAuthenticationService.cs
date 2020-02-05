using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
                _logger.LogError("Authentication failed",  "response", res);
                throw new Exception("Request was unsuccessful");
            }

            var item = res.Items[0];
            _accessKey = item.AccessKey;
            _memberId = int.Parse(item.MemberId);
            _expirationTime = DateTime.Now.Add(_configuration.AccessKeyDuration);

            _logger.LogInformation("Authenticated successful");
            return _accessKey;
        }
        
        
        private  string CalculateMD5Hash(string input)
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