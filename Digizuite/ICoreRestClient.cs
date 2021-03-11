using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Digizuite
{
    public interface ICoreRestClient
    {
        /// <summary>
        /// Execute Rest Request
        /// </summary>
        /// <typeparam name="T">The type the response items should be converted into</typeparam>
        /// <param name="method">Request Method (GET, POST ...)</param>
        /// <param name="request">Request Arguments</param>
        /// <param name="accessKey">optional accessKey, if specified, the accessKey is added to the request parameters</param>
        /// <param name="cancellationToken"></param>
        /// <returns>the rest response</returns>
        Task<IRestResponse<T>> Execute<T>(Method method, RestRequest request, string? accessKey = null,
            CancellationToken cancellationToken = default);
    }
}