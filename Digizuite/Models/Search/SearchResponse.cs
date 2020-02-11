using System;
using System.Collections.Generic;

namespace Digizuite.Models.Search
{
    /// <summary>
    ///     The response you get back from a search
    /// </summary>
    /// <typeparam name="T">The specific type of objects the search should be serialized into</typeparam>
    public class SearchResponse<T>
    {
        /// <summary>
        ///     The parameters that was used when requesting this search
        /// </summary>
        private readonly SearchParameters _searchParameters;

        /// <summary>
        ///     The items returned from this request
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        ///     How many items are available in total
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Creates a new search response. You probably should not be constructing this by hand. 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="total"></param>
        /// <param name="searchParameters"></param>
        public SearchResponse(IReadOnlyList<T> items, int total, SearchParameters searchParameters)
        {
            Items = items;
            Total = total;
            _searchParameters = new SearchParameters(searchParameters);
        }

        /// <summary>
        ///     Gets current page
        /// </summary>
        public int Page => _searchParameters.Page;
        /// <summary>
        ///     Gets the total number of pages available
        /// </summary>
        public int TotalPages => (int) Math.Ceiling(Total / (double) _searchParameters.PageSize);

        /// <summary>
        ///     Gets the search parameters for getting the next page
        /// </summary>
        public SearchParameters<T> Next => GoToPage(_searchParameters.Page + 1);

        /// <summary>
        ///     Gets the search parameters for getting the previous page
        /// </summary>
        public SearchParameters<T> Previous => GoToPage(_searchParameters.Page - 1);

        /// <summary>
        ///     Returns true if this is the last page of response
        /// </summary>
        public bool IsLast => TotalPages <= _searchParameters.Page;

        /// <summary>
        ///     Get the search parameters for going to a specific page
        /// </summary>
        /// <param name="page">The page to go to</param>
        public SearchParameters<T> GoToPage(int page)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), page, "Page cannot be less than 1");

            if (page > TotalPages)
                throw new ArgumentOutOfRangeException(nameof(page), page,
                    $"Page cannot be more than TotalPages ({TotalPages})");

            return new SearchParameters<T>(_searchParameters)
            {
                Page = page
            };
        }
    }
}