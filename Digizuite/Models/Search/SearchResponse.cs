using System.Collections.Generic;

namespace Digizuite.Models.Search
{
    /// <summary>
    ///     The response you get back from a search
    /// </summary>
    /// <typeparam name="T">The specific type of objects the search should be serialized into</typeparam>
    public class SearchResponse<T> : PagedResponse<T, SearchParameters<T>>
    {
        public SearchResponse(IReadOnlyList<T> items, int total, SearchParameters<T> parameters) : base(parameters, items,
            total)
        {
        }

        protected override SearchParameters<T> GetParametersForPage(int page)
        {
            return new SearchParameters<T>(Parameters)
            {
                Page = page
            };
        }
    }
}