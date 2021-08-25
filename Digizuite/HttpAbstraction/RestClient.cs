using System;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
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
        private readonly HttpClient _client;
        private readonly ILogger<RestClient> _logger;

        private readonly HttpSerializationSettings _serializationSettings;

        public RestClient(HttpClient httpClient, HttpSerializationSettings serializationSettings,
            ILogger<RestClient> logger)
        {
            Guard.AgainstNull(httpClient, nameof(httpClient));
            Guard.AgainstNull(serializationSettings, nameof(serializationSettings));

            _serializationSettings = serializationSettings;
            _logger = logger;
            _client = httpClient;
        }

        public async Task<RestResponse<T?>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken)
            where T : notnull
        {
            using var response = await SendRequest(request, cancellationToken);

            var (responseBody, rawResponse, exception, activityId) =
                await ReadResponseBody<T>(response, cancellationToken);

            _logger.LogTrace("Raw api response", nameof(rawResponse), rawResponse, nameof(activityId), activityId);
            if (rawResponse != null)
                return new DebugRestResponse<T?>(response.StatusCode, exception, responseBody, rawResponse, activityId);

            return new RestResponse<T?>(response.StatusCode, exception, responseBody, activityId);
        }

        public async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken)
        {
            using var response = await SendRequest(request, cancellationToken);

            var content = await response.Content.ReadAsStringAsync();
            var activityId = response.Headers.TryGetValues("X-Activity-Id", out var activityHeaders)
                ? activityHeaders.FirstOrDefault()
                : null;

            _logger.LogTrace("Raw api response", nameof(content), content, nameof(activityId), activityId);

            return new RestResponse(response.StatusCode, null, content, activityId);
        }

        public async Task<Stream> StreamAsync(RestRequest request, CancellationToken cancellationToken)
        {
            var url = request.Uri.AbsoluteUri;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            var httpResponseMessage = await _client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            return await httpResponseMessage.Content.ReadAsStreamAsync();
        }

        private async Task<HttpResponseMessage> SendRequest(RestRequest request, CancellationToken cancellationToken)
        {
            var uri = GetUrl(request);

            using var message = new HttpRequestMessage(request.Method, uri);
            var bodyTask = Task.CompletedTask;

            if (request.Body != null)
                (message.Content, bodyTask) = request.Body.GetBody(_serializationSettings, cancellationToken);

            foreach (string key in request.Headers)
            {
                var value = request.Headers[key];
                message.Headers.Add(key, value);
            }

            message.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var requestTask = _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await bodyTask.ConfigureAwait(false);
            return await requestTask.ConfigureAwait(false);
        }


        private async Task<ReadResponseResult<T>> ReadResponseBody<T>(HttpResponseMessage response,
            CancellationToken cancellationToken)
            where T : notnull
        {
            var activityId = response.Headers.TryGetValues("X-Activity-Id", out var activityHeaders)
                ? activityHeaders.FirstOrDefault()
                : null;


            if (response.StatusCode == HttpStatusCode.NoContent)
                return new ReadResponseResult<T>(default, "", null, activityId);

            if (_logger.IsLogLevelEnabled(LogLevel.Debug) || !response.IsSuccessStatusCode)
            {
                // Use more memory intensive implementation to allow easier debugging
                var stream = new MemoryStream((int)(response.Content.Headers.ContentLength ?? 0));
                await response.Content.CopyToAsync(stream).ConfigureAwait(false);

                stream.Position = 0;
                Exception? deserializationException = null;
                T? body = default;
                try
                {
                    body = await _serializationSettings.Serializer.Deserialize<T>(stream, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Deserialization exception");
                    deserializationException = e;
                }

                var rawResponse = Encoding.UTF8.GetString(stream.ToArray());

                return new ReadResponseResult<T>(body, rawResponse, deserializationException, activityId);
            }
            else
            {
                var pipe = new Pipe();

                using var writerStream = pipe.Writer.AsStream();

                var copyTask = response.Content.CopyToAsync(writerStream);

                Exception? deserializationException = null;
                Task<T?> bodyTask = Task.FromResult(default(T));
                try
                {
                    bodyTask = _serializationSettings.Serializer.Deserialize<T>(pipe.Reader.AsStream(),
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Deserialization exception");
                    deserializationException = e;
                }

                await copyTask.ContinueWith(_ => writerStream.Dispose(), cancellationToken).ConfigureAwait(false);

                var bodyResult = await bodyTask.ConfigureAwait(false);
                return new ReadResponseResult<T>(bodyResult, null, deserializationException, activityId);
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

        private readonly struct ReadResponseResult<T>
            where T : notnull
        {
            public readonly T? Body;
            public readonly string? ResponseContent;
            public readonly Exception? Exception;
            public readonly string? ActivityId;

            public ReadResponseResult(T? body, string? responseContent, Exception? exception, string? activityId)
            {
                Body = body;
                ResponseContent = responseContent;
                Exception = exception;
                ActivityId = activityId;
            }

            public void Deconstruct(out T? body, out string? responseContent, out Exception? exception,
                out string? activityId)
            {
                body = Body;
                responseContent = ResponseContent;
                exception = Exception;
                activityId = ActivityId;
            }
        }
    }

    public interface IRestClient
    {
        Task<RestResponse<T?>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken)
            where T : notnull;

        Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken);


        Task<Stream> StreamAsync(RestRequest request, CancellationToken cancellationToken);
    }

    public static class RestClientExtensions
    {
        public static Task<RestResponse<T?>> PostAsync<T>(this IRestClient client, RestRequest request,
            CancellationToken cancellation)
            where T : notnull
        {
            request.Method = HttpMethod.Post;

            return client.SendAsync<T>(request, cancellation);
        }

        public static Task<RestResponse<T?>> GetAsync<T>(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
            where T : notnull
        {
            request.Method = HttpMethod.Get;

            return client.SendAsync<T>(request, cancellationToken);
        }

        public static Task<RestResponse> GetAsync(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
        {
            request.Method = HttpMethod.Get;

            return client.SendAsync(request, cancellationToken);
        }

        public static Task<RestResponse> PostAsync(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
        {
            request.Method = HttpMethod.Post;

            return client.SendAsync(request, cancellationToken);
        }

        public static Task<RestResponse<T?>> DeleteAsync<T>(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
            where T : notnull
        {
            request.Method = HttpMethod.Delete;

            return client.SendAsync<T>(request, cancellationToken);
        }

        public static Task<RestResponse> DeleteAsync(this IRestClient client, RestRequest request,
            CancellationToken cancellationToken)
        {
            request.Method = HttpMethod.Delete;

            return client.SendAsync(request, cancellationToken);
        }
    }
}