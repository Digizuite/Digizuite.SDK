using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.BatchBuilder;
using Digizuite.Cache;
using Digizuite.Folders;
using Digizuite.Metadata;
using Digizuite.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Digizuite
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services from the Digizuite SDK to the service provider
        /// </summary>
        public static IServiceCollection AddDigizuite(this IServiceCollection services,
            DigizuiteConfiguration configuration, DigizuiteOption options = DigizuiteOption.Nothing)
        {
            return services.AddDigizuite((IConfiguration) configuration, options);
        }
        
        /// <summary>
        /// Adds the services from the Digizuite SDK to the service provider
        /// </summary>
        public static IServiceCollection AddDigizuite(this IServiceCollection services, IConfiguration configuration, DigizuiteOption options = DigizuiteOption.Nothing)
        {
            services.AddSingleton<IDamRestClient, DamRestClient>();
            services.AddSingleton<IDamAuthenticationService, DamAuthenticationService>();
            services.AddSingleton<IBatchUpdateClient, BatchUpdateClient>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IUploadService, UploadService>();
            services.AddSingleton<IMetadataValueService, MetadataValueService>();
            services.AddSingleton<IAssetStreamerService, AssetStreamerService>();
            services.AddSingleton<IAssetService, AssetService>();
            services.AddSingleton<IMetaFieldCacheService, MetaFieldCacheService>();
            services.AddSingleton<IMetaFieldsLoaderService, MetaFieldsLoaderService>();
            services.AddSingleton<IMetaGroupLoaderService, MetaGroupLoaderService>();
            services.AddSingleton<IFolderService, FolderService>();
            services.AddSingleton<IAssetFolderService, AssetFolderService>();
            services.AddSingleton<IItemService, ItemService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IBatchBuilderService, BatchBuilderService>();
            services.AddSingleton<IComboValueService, ComboValueService>();
            services.AddSingleton(configuration);
            
            if (!options.HasFlag(DigizuiteOption.SkipCache))
            {
                services.AddSingleton(typeof(ICache<>), typeof(InMemoryCache<>));
                services.AddMemoryCache();
            }

            return services;
        }
    }
}