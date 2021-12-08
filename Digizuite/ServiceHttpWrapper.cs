using System;
using System.Collections.Generic;
using System.Net.Http;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using JetBrains.Annotations;

namespace Digizuite
{
    public class ServiceHttpWrapper
    {
        private readonly IConfiguration _configuration;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _internalConfiguration;

        private readonly IRestClient _coreRestClient;

        private readonly Dictionary<ServiceType, string> _developmentServiceUrls = new()
        {
            {ServiceType.BusinessWorkflow, "https://localhost:7001"},
            {ServiceType.DslService, "https://localhost:5063"},
            {ServiceType.LoginService, "https://localhost:5091"},
            {ServiceType.LegacyService, "https://localhost:5012"},
            {ServiceType.NotificationService, "https://localhost:5201"},
            {ServiceType.FileService, "https://localhost:5081"},
            {ServiceType.TranscodeService, "https://localhost:5023"},
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
            {ServiceType.NotificationService, "/DigizuiteCore/NotificationService"},
            {ServiceType.FileService, "/DigizuiteCore/FileService"},
            {ServiceType.TranscodeService, "/DigizuiteCore/TranscodeService"},
            {ServiceType.Dmm3bwsv3, "/dmm3bwsv3/"}
        };

        private readonly Dictionary<ServiceType, string> _dockerProductionUrls;

        public ServiceHttpWrapper(
            DevServerConfigurations devServerConfigurations,
            IConfiguration configuration,
            Microsoft.Extensions.Configuration.IConfiguration internalConfiguration,
            HttpClient httpClient, 
            ILogger<RestClient> logger)
        {
            _devServerConfigurations = devServerConfigurations;
            _configuration = configuration;
            _internalConfiguration = internalConfiguration;

            _coreRestClient = new RestClient(httpClient, new HttpSerializationSettings
            {
                Serializer = new DigizuiteCoreSerializer()
            }, logger);
            _dmm3bwsv3RestClient = new RestClient(httpClient, new HttpSerializationSettings
            {
                Serializer = new Dimmer3Serializer()
            }, logger);
            
            _dockerProductionUrls = new()
            {
                {ServiceType.BusinessWorkflow, _internalConfiguration["InternalUrls:businessworkflowservice"]},
                {ServiceType.DslService, _internalConfiguration["InternalUrls:dslservice"]},
                {ServiceType.LoginService, _internalConfiguration["InternalUrls:loginservice"]},
                {ServiceType.LegacyService, _internalConfiguration["InternalUrls:legacyservice"]},
                {ServiceType.NotificationService, _internalConfiguration["InternalUrls:notificationservice"]},
                {ServiceType.FileService, _internalConfiguration["InternalUrls:fileservice"]},
                {ServiceType.TranscodeService, _internalConfiguration["InternalUrls:transcodeservice"]},
                {ServiceType.Dmm3bwsv3, "/dmm3bwsv3/"}
            };
        }

        private Uri GetUri(string baseUrl, string servicePath, string requestedPath)
        {
            var url = string.IsNullOrWhiteSpace(baseUrl) 
                ? servicePath.TrimEnd('/') + "/" + requestedPath.TrimStart('/') 
                : $"{baseUrl.TrimEnd('/')}/{servicePath.Trim('/')}/{requestedPath.TrimStart('/')}";

            return new Uri(url);
        }
        
        public (IRestClient Client, RestRequest Request) GetClientAndRequest(ServiceType serviceType, [UriString] string path)
        {
            var uri = GetServiceUrl(serviceType, path);

            var client = serviceType == ServiceType.Dmm3bwsv3 ? _dmm3bwsv3RestClient : _coreRestClient;

            var request = new RestRequest(uri);

            return (client, request);
        }

        public (IRestClient client, RestRequest request) GetRawClient(string baseUrl, [UriString] string path)
        {
            var uri = GetUri("", baseUrl, path);

            return (_coreRestClient, new RestRequest(uri));
        }

        public Uri GetServiceUrl(ServiceType serviceType, [UriString] string path)
        {
            var isDev = _devServerConfigurations.IsDevelopmentMode(serviceType);
            var pathUrl = (isDev, _configuration.RunInDocker) switch
            {
                (true, _) => _developmentServiceUrls[serviceType],
                (_, true) => _dockerProductionUrls[serviceType],
                _ => _productionServiceUrls[serviceType],
            };

            var baseUrl = "";
            if (!Uri.IsWellFormedUriString(pathUrl, UriKind.Absolute))
            {
                baseUrl = _configuration.BaseUrl.ToString();
            }

            return GetUri(baseUrl, pathUrl, path);
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
        Dmm3bwsv3,
        NotificationService,
        FileService,
        TranscodeService,
    }

    public static class ServiceHttpWrapperExtensions
    {
        public static (IRestClient Client, RestRequest Request) GetSearchServiceClient(this ServiceHttpWrapper serviceHttpWrapper)
        {
            return serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, "SearchService.js");
        }
    }
}
