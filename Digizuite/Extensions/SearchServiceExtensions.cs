using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Models.Search;

namespace Digizuite.Extensions
{
    public static class SearchServiceExtensions
    {
        /// <summary>
        /// Helpers method for executing a specific search, and just getting all results, skipping any parameters
        /// </summary>
        /// <param name="searchService">The search service to use</param>
        /// <param name="searchName">The name of the search to execute</param>
        public static async Task<IReadOnlyList<T>> ExecuteSearch<T>(this ISearchService searchService, string searchName)
        {
            if (searchService == null)
            {
                throw new ArgumentNullException(nameof(searchService));
            }

            if (searchName == null)
            {
                throw new ArgumentNullException(nameof(searchName));
            }
            
            var parameters = new SearchParameters(searchName, 1, int.MaxValue);

            var result = await searchService.Search<T>(parameters).ConfigureAwait(false);

            return result.Items;
        }
    }
}