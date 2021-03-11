using System.Collections.Generic;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class HttpClientFactory
    {
        private readonly Dictionary<string, IRestClient> _cachedClients = new();

        public IRestClient GetRestClient(string baseUrl = "")
        {
            lock (_cachedClients)
            {
                if (_cachedClients.TryGetValue(baseUrl, out var client)) return client!;

                client = CreateClient(baseUrl);
                client.UseSystemTextJsonSerializer();

                _cachedClients[baseUrl] = client;

                return client;
            }
        }

        private IRestClient CreateClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) return new RestClient();

            return new RestClient(baseUrl);
        }
    }

    public class ServiceHttpWrapper
    {
        private readonly HttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly DevServerConfigurations _devServerConfigurations;

        private readonly Dictionary<ServiceType, string> _developmentServiceUrls = new()
        {
            {ServiceType.BusinessWorkflow, "https://localhost:7001"},
            {ServiceType.DslService, "https://localhost:5063"},
            {ServiceType.LoginService, "https://localhost:5091"},
            {ServiceType.LegacyService, "https://localhost:5012"}
        };

        private readonly Dictionary<ServiceType, string> _productionServiceUrls = new()
        {
            {ServiceType.BusinessWorkflow, "/DigizuiteCore/BusinessWorkflowService"},
            {ServiceType.DslService, "/DigizuiteCore/dsl-service"},
            {ServiceType.LoginService, "/DigizuiteCore/LoginService"},
            {ServiceType.LegacyService, "/DigizuiteCore/LegacyService"}
        };

        public ServiceHttpWrapper(HttpClientFactory clientFactory, DevServerConfigurations devServerConfigurations,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _devServerConfigurations = devServerConfigurations;
            _configuration = configuration;
        }

        public (IRestClient, IRestRequest) GetClientAndRequest(ServiceType serviceType, string path)
        {
            var isDev = _devServerConfigurations.IsDevelopmentMode(serviceType);

            var baseUrl = isDev ? "" : _configuration.BaseUrl.ToString();
            var pathUrl = isDev ? _developmentServiceUrls[serviceType] : _productionServiceUrls[serviceType];

            var client = _clientFactory.GetRestClient(baseUrl);
            var request = new RestRequest(pathUrl + path);

            return (client, request);
        }
    }

    public class DevServerConfigurations
    {
        private readonly HashSet<ServiceType> _overridenServices;

        public DevServerConfigurations(IConfiguration configuration)
        {
            _overridenServices = configuration.DevelopmentServices;
        }


        public bool IsDevelopmentMode(ServiceType serviceType)
        {
            return _overridenServices.Contains(serviceType);
        }
    }

    public enum ServiceType
    {
        BusinessWorkflow,
        DslService,
        LoginService,
        LegacyService
    }
}