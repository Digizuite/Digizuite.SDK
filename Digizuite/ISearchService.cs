using System.Threading;
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
        /// <param name="accessKey">Optional accessKey, if not specified use DamAuthentication</param>
        /// <typeparam name="T">The type the response items should be converted into</typeparam>
        Task<SearchResponse<T>> Search<T>(SearchParameters parameters, string accessKey = null);

        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <param name="accessKey">Optional accessKey, if not specified use DamAuthentication</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The type of the response items should be converted into</typeparam>
        Task<SearchResponse<T>> Search<T>(SearchParameters<T> parameters, string accessKey = null,
            CancellationToken cancellationToken = default );
    }
}