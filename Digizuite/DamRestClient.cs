using System;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Extensions;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable NotAccessedField.Local

namespace Digizuite
{
    /// <inheritdoc />
    public class DamRestClient : IDamRestClient
    {
        private readonly IRestClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client">optional IRestClient used for unit testing</param>
        public DamRestClient(IConfiguration configuration,  IRestClient? client = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }


            var baseUrl = configuration.GetDmm3Bwsv3Url();
            _client = client ?? new RestClient(baseUrl).UseSerializer(() => new JsonNetSerializer());
        }
        
        /// <inheritdoc />
        public Task<IRestResponse<T>>  Execute<T>(Method method, RestRequest request, string? accessKey = null, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.OnBeforeDeserialization = response => { response.ContentType = "application/json"; };
            request.Method = method;
            if (!string.IsNullOrWhiteSpace(accessKey))
            {
                request.AddAccessKey(accessKey!);
            }
            return _client.ExecuteAsync<T>(request, cancellationToken);
        }
    }
}
