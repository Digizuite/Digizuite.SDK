﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Search;
using Microsoft.Extensions.DependencyInjection;

namespace Digizuite.Samples.Search
{
    public class SearchExamples
    {
        private ServiceProvider _serviceProvider = Digizuite.Samples.Initialization.Initialization.GetServiceProvider();
        
        /// <summary>
        /// Returns the first 12 assets in the default catalog folder Content (40)
        /// </summary>
        /// <returns></returns>
        public async Task<SearchResponse<Asset>> GetAssets()
        {
            var searchService = _serviceProvider.GetRequiredService<ISearchService>();
            
            var parameters = new SearchParameters("GetAssets")
            {
                {"sCatalogFolderId", "40"}
            };

            return await searchService.Search<Digizuite.Models.Asset>(parameters);
        }

        /// <summary>
        /// Returns page X, defined by page, with Y number of assets, defined by pageSize,
        /// in the default catalog folder Content (40)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<SearchResponse<Asset>> GetAssetsWithPaging(int page, int pageSize)
        {
            var searchService = _serviceProvider.GetRequiredService<ISearchService>();
            
            var parameters = new SearchParameters("GetAssets", page, pageSize)
            {
                {"sCatalogFolderId", "40"}
            };

            return await searchService.Search<Digizuite.Models.Asset>(parameters);
        }
    }
}