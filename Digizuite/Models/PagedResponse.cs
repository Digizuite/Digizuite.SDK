using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Digizuite.Models
{
    public interface IPagedParameters
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }

    public abstract class PagedParameters : IPagedParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public abstract class PagedResponse<TResponse, TParams>
        where TParams : IPagedParameters
    {
        protected readonly TParams Parameters;

        protected PagedResponse(TParams parameters, IReadOnlyList<TResponse> items, int total)
        {
            Parameters = parameters;
            Items = items;
            Total = total;
        }

        /// <summary>
        ///     The items returned from this request
        /// </summary>
        public IReadOnlyList<TResponse> Items { get; }

        /// <summary>
        ///     How many items are available in total
        /// </summary>
        public int Total { get; }

        /// <summary>
        ///     Gets current page
        /// </summary>
        public int Page => Parameters.Page;

        /// <summary>
        ///     Gets the total number of pages available
        /// </summary>
        public int TotalPages => (int) Math.Ceiling(Total / (double) Parameters.PageSize);

        /// <summary>
        ///     Gets the search parameters for getting the next page
        /// </summary>
        [JsonIgnore]
        public TParams Next => GoToPage(Page + 1);

        /// <summary>
        ///     Gets the search parameters for getting the previous page
        /// </summary>
        [JsonIgnore]
        public TParams Previous => GoToPage(Page - 1);

        /// <summary>
        ///     Returns true if this is the last page of response
        /// </summary>
        public bool IsLast => TotalPages <= Page;

        /// <summary>
        ///     Get the search parameters for going to a specific page
        /// </summary>
        /// <param name="page">The page to go to</param>
        public TParams GoToPage(int page)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), page, "Page cannot be less than 1");

            if (page > TotalPages)
                throw new ArgumentOutOfRangeException(nameof(page), page,
                    $"Page cannot be more than TotalPages ({TotalPages})");

            return GetParametersForPage(page);
        }

        protected abstract TParams GetParametersForPage(int page);
    }
}