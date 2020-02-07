using Digizuite.BatchUpdate;
using Microsoft.Extensions.DependencyInjection;

namespace Digizuite
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services from the Digizuite SDK to the service provider
        /// </summary>
        public static IServiceCollection AddDigizuite(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IDamAuthenticationService, DamAuthenticationService>();
            services.AddSingleton<IBatchUpdateClient, BatchUpdateClient>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IUploadService, UploadService>();
            
            return services;
        }
    }
}