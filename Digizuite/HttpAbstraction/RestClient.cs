using System;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Digizuite.Helpers;
using Digizuite.Logging;

namespace Digizuite.HttpAbstraction
{
    public class RestClient : IRestClient
    {
        private readonly ILogger<RestClient> _logger;
        private readonly HttpClient _client;

        private readonly HttpSerializationSettings _serializationSettings;

        public RestClient(HttpClient httpClient, HttpSerializationSettings serializationSettings, ILogger<RestClient> logger)
        {
            Guard.AgainstNull(httpClient, nameof(httpClient));
            Guard.AgainstNull(serializationSettings, nameof(serializationSettings));
            
            _serializationSettings = serializationSettings;
            _logger = logger;
            _client = httpClient;
        }

        public async Task<RestResponse<T?>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken)
        {
            var uri = GetUrl(request);

            using var message = new HttpRequestMessage(request.Method, uri);
            var bodyTask = Task.CompletedTask;
            
            if (request.Body != null)
            {
                (message.Content, bodyTask) = request.Body.GetBody(_serializationSettings, cancellationToken);
            }
            foreach (string key in request.Headers)
            {
                var value = request.Headers[key];
                message.Headers.Add(key, value);
            }
            
            message.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var requestTask = _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await bodyTask.ConfigureAwait(false);
            using var response = await requestTask.ConfigureAwait(false);
            
            var (responseBody, rawResponse) = await ReadResponseBody<T>(response, cancellationToken);

            _logger.LogDebug("Raw api response", nameof(rawResponse), rawResponse ?? null);
            if (rawResponse != null)
            {
                return new DebugRestResponse<T?>(responseBody, response.StatusCode, rawResponse);
            }
            
            return new RestResponse<T?>(responseBody, response.StatusCode);
        }

        private async Task<(T?, string? responseContent)> ReadResponseBody<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return (default, "");
            }
            
            if (_logger.IsLogLevelEnabled(LogLevel.Debug) || !response.IsSuccessStatusCode)
            {
                // Use more memory intensive implementation to allow easier debugging
                var stream = new MemoryStream((int) (response.Content.Headers.ContentLength ?? 0));
                await response.Content.CopyToAsync(stream).ConfigureAwait(false);

                stream.Position = 0;
                T body = default;
                try
                {
                    body = await _serializationSettings.Serializer.Deserialize<T>(stream, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Deserialization exception");
                }

                var rawResponse = Encoding.UTF8.GetString(stream.ToArray());

                return (body, rawResponse);
            }
            else
            {
                var pipe = new Pipe();

                using var writerStream = pipe.Writer.AsStream();
                
                var copyTask = response.Content.CopyToAsync(writerStream);

                Task<T?> bodyTask = Task.FromResult(default(T));
                try
                {
                    bodyTask = _serializationSettings.Serializer.Deserialize<T>(pipe.Reader.AsStream(),
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Deserialization exception");
                }

                await copyTask.ContinueWith(_ => writerStream.Dispose(), cancellationToken).ConfigureAwait(false);

                return (await bodyTask.ConfigureAwait(false), null);
            }
        }

        private Uri GetUrl(RestRequest request)
        {
            var builder = new UriBuilder(request.Uri);

            var existingQuery = HttpUtility.ParseQueryString(builder.Query);

            existingQuery.Add(request.QueryParameters);

            builder.Query = existingQuery.ToString();

            return builder.Uri;
        }
    }

    public interface IRestClient
    {
        Task<RestResponse<T?>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken);
    }

    public static class RestClientExtensions
    {
        public static Task<RestResponse<T?>> PostAsync<T>(this IRestClient client, RestRequest request, CancellationToken cancellation)
        {
            request.Method = HttpMethod.Post;

            return client.SendAsync<T>(request, cancellation);
        }

        public static Task<RestResponse<T?>> GetAsync<T>(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
        {
            request.Method = HttpMethod.Get;

            return client.SendAsync<T>(request, cancellationToken);
        }
    }
}
