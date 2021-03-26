using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace Digizuite.HttpAbstraction
{
    public class RestRequest
    {
        public readonly NameValueCollection Headers = new();

        public readonly NameValueCollection QueryParameters = new();

        public readonly Uri Uri;
        public HttpMethod Method = HttpMethod.Get;

        public RestRequest(Uri uri)
        {
            Uri = uri;
        }

        public RestRequest(string uri)
        {
            Uri = new Uri(uri);
        }

        public IBodyParameters? Body { get; set; } = null;
    }

    public static class RestRequestExtensions
    {
        public static RestRequest AddQueryParameter(this RestRequest request, string key, string value)
        {
            request.QueryParameters.Add(key, value);
            return request;
        }

        public static RestRequest AddQueryParameter(this RestRequest request, string key, int value)
        {
            return request.AddQueryParameter(key, value.ToString());
        }

        public static RestRequest AddQueryParameter(this RestRequest request, string key, bool value)
        {
            return request.AddQueryParameter(key, value.ToString().ToLower());
        }

        public static RestRequest AddJsonBody<T>(this RestRequest request, T body)
        {
            request.Body = new JsonBody<T>(body);
            return request;
        }
    }
}