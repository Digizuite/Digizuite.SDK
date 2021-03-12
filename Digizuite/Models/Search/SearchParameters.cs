using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace Digizuite.Models.Search
{
#pragma warning disable CA1010,CA1710
    public class SearchParameters : NameValueCollection, IPagedParameters
    {
        public const string PageSizeKey = "limit";
        public const int DefaultPageSize = 12;

        public const string PageKey = "page";
        public const int DefaultPage = 1;

        public const string SearchNameKey = "SearchName";
        public const string MethodNameKey = "method";

        /// <summary>
        /// Creates a new set of search parameters
        /// </summary>
        /// <param name="searchName">The name of the search to execute</param>
        /// <param name="page">The page to load</param>
        /// <param name="pageSize">How many items to return</param>
        /// <param name="method">A method name in the api</param>
        public SearchParameters(string? searchName = null, int page = DefaultPage, int pageSize = DefaultPageSize, string? method = null)
        {
            PageSize = pageSize;
            Page = page;
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                SearchName = searchName;
            }

            if (!string.IsNullOrWhiteSpace(method))
            {
                Method = method;
            }
        }

        /// <summary>
        /// Alternative constructor for creating a copy of an existing set of search parameters
        /// </summary>
        public SearchParameters(SearchParameters? parameters) : base(parameters ?? new NameValueCollection())
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// How many items will at the most be returned from the request
        /// </summary>
        /// <exception cref="InvalidCastException">If the stored key is not a valid integer</exception>
        #pragma warning disable CA1065
        public int PageSize
        {
            get
            {
                var pageSize = this[PageSizeKey];

                if (int.TryParse(pageSize, out var size)) return size;

                throw new InvalidCastException(
                    $"The stored page size was not a valid integer. Value was: '{pageSize}'");
            }
            set => this[PageSizeKey] = value.ToString(CultureInfo.InvariantCulture);
        }
#pragma warning restore CA1065

        /// <summary>
        /// What page of items will be returned
        /// </summary>
        /// <exception cref="InvalidCastException">If the stored key is not a valid integer</exception>
#pragma warning disable CA1065
        public int Page
        {
            get
            {
                var page = this[PageKey];
                if (int.TryParse(page, out var p)) return p;

                throw new InvalidCastException($"The stored page was not a valid integer. Value was: '{page}'");
            }
            set => this[PageKey] = value.ToString(CultureInfo.InvariantCulture);
        }
#pragma warning restore CA1065

        /// <summary>
        /// Get or sets the name of the specific search that should be executed
        /// </summary>
        public string? SearchName
        {
            get => this[SearchNameKey];
            set => this[SearchNameKey] = value;
        }

        /// <summary>
        /// Get or sets the name of the method to execute
        /// </summary>
        public string? Method
        {
            get => this[MethodNameKey];
            set => this[MethodNameKey] = value;
        }

        /// <summary>
        /// Sets the provided values.
        ///
        /// If values is empty, then the key is removed
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="values">The values to set for that specific key</param>
        public void Set(string key, IEnumerable<string> values)
        {
            Remove(key);

            var internalValues = values.ToArray();

            if (internalValues.Any())
            {
                foreach (var value in internalValues)
                {
                    Add(key, value);
                }

                Set(MultiStringsName(key), "1");
            }
            else
            {
                Remove(MultiStringsName(key));
            }
        }

        private static string MultiStringsName(string key)
        {
            return $"{key}_type_multistrings";
        }

        private static string MultiIdsName(string key)
        {
            return $"{key}_type_multiids";
        }

        /// <summary>
        /// Sets the specific value
        /// </summary>
        /// <param name="key">The parameter key</param>
        /// <param name="value">The values to set</param>
        public void Set(string key, int value)
        {
            Set(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the specific values
        /// </summary>
        /// <param name="key">They parameter key</param>
        /// <param name="values">The values to set</param>
        public void Set(string key, IEnumerable<int> values)
        {
            var valueList = values.ToList();
            Set(key, valueList.Select(v => v.ToString(CultureInfo.InvariantCulture)));
            Remove(MultiStringsName(key));
            if (valueList.Any())
                Set(MultiIdsName(key), "1");
            else
                Remove(MultiIdsName(key));
        }

        /// <summary>
        /// Sets data specifically for date between parameters
        /// </summary>
        /// <param name="key">The key to set</param>
        /// <param name="from">From which date to search. If you don't specify anything DateTime.MinValue will be used</param>
        /// <param name="to">To which date to search. If you don't specify anything DateTime.MaxValue will be used</param>
        public void SetDateBetween(string key, DateTime from = default, DateTime to = default)
        {
            // Make the parameters optional
            if (from == default)
            {
                from = DateTime.MinValue;
            }

            if (to == default)
            {
                to = DateTime.MaxValue;
            }

            if (to < from)
            {
                throw new ArgumentException("from date is after to date");
            }

            var endName = $"{key}_end";
            Set(key, from.ToString(CultureInfo.InvariantCulture));
            Set($"{key}_type_date", endName);
            Set(endName, to.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Sets the specific values
        /// </summary>
        public void Add(string key, int value)
        {
            Set(key, value);
        }

        /// <summary>
        /// Sets the specific values
        /// </summary>
        public void Add(string key, IEnumerable<string> values)
        {
            Set(key, values);
        }

        /// <summary>
        /// Sets the specific values
        /// </summary>
        public void Add(string key, IEnumerable<int> values)
        {
            Set(key, values);
        }
    }

    /// <summary>
    /// Helper class for inferring responses when paging through an api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchParameters<T> : SearchParameters
    {
        /// <inheritdoc/>
        public SearchParameters(string? searchName = null, int page = DefaultPage, int pageSize = DefaultPageSize, string? method = null) : base(
            searchName, page, pageSize, method)
        {
        }

        /// <summary>
        /// Creates a new copy of an existing set of search parameters
        /// </summary>
        /// <param name="parameters"></param>
        public SearchParameters(SearchParameters<T>? parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Creates a type specific copy of an existing set of search parameters
        /// </summary>
        /// <param name="parameters"></param>
        public SearchParameters(SearchParameters? parameters) : base(parameters)
        {
        }
    }
#pragma warning restore CA1010,CA1710
}