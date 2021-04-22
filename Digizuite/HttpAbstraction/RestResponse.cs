using System;
using System.Net;

namespace Digizuite.HttpAbstraction
{
    public record BaseRestResponse
    {
        public readonly HttpStatusCode StatusCode;

        public bool IsSuccessful => Exception == null && StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;

        public readonly Exception? Exception;

        public BaseRestResponse(HttpStatusCode statusCode, Exception? exception)
        {
            StatusCode = statusCode;
            Exception = exception;
        }
    }
    
    
    public record RestResponse : BaseRestResponse
    {
        public readonly string Content;

        public RestResponse(HttpStatusCode statusCode, Exception? exception, string content) : base(statusCode, exception)
        {
            Content = content;
        }
    }
    
    
    public record RestResponse<T> : BaseRestResponse
    {
        public readonly T Data;

        public RestResponse(HttpStatusCode statusCode, Exception? exception, T data) : base(statusCode, exception)
        {
            Data = data;
        }
    }

    public record DebugRestResponse<T> : RestResponse<T>
    {
        public readonly string Content;

        public DebugRestResponse(HttpStatusCode statusCode, Exception? exception, T data, string content) : base(statusCode, exception, data)
        {
            Content = content;
        }
    }
}