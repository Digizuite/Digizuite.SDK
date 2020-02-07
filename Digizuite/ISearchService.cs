using System.Threading.Tasks;
using Digizuite.Models.Search;

namespace Digizuite
{
    /// <summary>
    /// Provides helpers for executing a search and dealing with the response
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <typeparam name="T">The type the response items should be converted into</typeparam>
        Task<SearchResponse<T>> Search<T>(SearchParameters parameters);

        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <typeparam name="T">The type of the response items should be converted into</typeparam>
        Task<SearchResponse<T>> Search<T>(SearchParameters<T> parameters);
    }
}