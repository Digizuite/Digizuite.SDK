using System;
using System.Collections.Generic;
using System.Net.Http;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;

namespace Digizuite
{
    public class ServiceHttpWrapper
    {
        private readonly IConfiguration _configuration;

        private readonly IRestClient _coreRestClient;

        private readonly Dictionary<ServiceType, string> _developmentServiceUrls = new()
        {
            {ServiceType.BusinessWorkflow, "https://localhost:7001"},
            {ServiceType.DslService, "https://localhost:5063"},
            {ServiceType.LoginService, "https://localhost:5091"},
            {ServiceType.LegacyService, "https://localhost:5012"},
            {ServiceType.Dmm3bwsv3, "http://local.dev.digizuite.com/dev/dmm3bwsv3"}
        };

        private readonly DevServerConfigurations _devServerConfigurations;
        private readonly IRestClient _dmm3bwsv3RestClient;

        private readonly Dictionary<ServiceType, string> _productionServiceUrls = new()
        {
            {ServiceType.BusinessWorkflow, "/DigizuiteCore/BusinessWorkflowService"},
            {ServiceType.DslService, "/DigizuiteCore/dsl-service"},
            {ServiceType.LoginService, "/DigizuiteCore/LoginService"},
            {ServiceType.LegacyService, "/DigizuiteCore/LegacyService"},
            {ServiceType.Dmm3bwsv3, "/dmm3bwsv3/"}
        };

        public ServiceHttpWrapper(DevServerConfigurations devServerConfigurations,
            IConfiguration configuration, HttpClient httpClient, ILogger<RestClient> logger)
        {
            _devServerConfigurations = devServerConfigurations;
            _configuration = configuration;

            _coreRestClient = new RestClient(httpClient, new HttpSerializationSettings
            {
                Serializer = new SystemTextJsonSerializer()
            }, logger);
            _dmm3bwsv3RestClient = new RestClient(httpClient, new HttpSerializationSettings
            {
                Serializer = new JsonNetJsonSerializer()
            }, logger);
        }

        private Uri GetUri(string baseUrl, string servicePath, string requestedPath)
        {
            var url = string.IsNullOrWhiteSpace(baseUrl) ? servicePath.TrimEnd('/') + "/" + requestedPath.TrimStart('/') : $"{baseUrl.TrimEnd('/')}/{servicePath.Trim('/')}/{requestedPath.TrimStart('/')}";

            return new Uri(url);
        }
        
        public (IRestClient Client, RestRequest Request) GetClientAndRequest(ServiceType serviceType, string path)
        {
            var isDev = _devServerConfigurations.IsDevelopmentMode(serviceType);

            var baseUrl = isDev ? "" : _configuration.BaseUrl.ToString();
            var pathUrl = isDev ? _developmentServiceUrls[serviceType] : _productionServiceUrls[serviceType];

            var uri = GetUri(baseUrl, pathUrl, path);

            var client = serviceType == ServiceType.Dmm3bwsv3 ? _dmm3bwsv3RestClient : _coreRestClient;

            var request = new RestRequest(uri);

            return (client, request);
        }
    }

    public class DevServerConfigurations
    {
        private readonly HashSet<ServiceType> _overridenServices;

        public DevServerConfigurations(IConfiguration configuration)
        {
            _overridenServices = configuration.DevelopmentServices ?? new HashSet<ServiceType>();
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
        LegacyService,
        Dmm3bwsv3
    }

    public static class ServiceHttpWrapperExtensions
    {
        public static (IRestClient Client, RestRequest Request) GetSearchServiceClient(this ServiceHttpWrapper serviceHttpWrapper)
        {
            return serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, "SearchService.js");
        }
    }
}