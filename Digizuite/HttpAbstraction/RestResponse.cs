using System.Net;

namespace Digizuite.HttpAbstraction
{
    public record RestResponse
    {
        public readonly HttpStatusCode StatusCode;

        public bool IsSuccessful => StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;

        public readonly string Content;

        public RestResponse(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
    
    
    public record RestResponse<T>
    {
        public readonly HttpStatusCode StatusCode;

        public bool IsSuccessful => StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
        
        public readonly T Data;

        public RestResponse(T data, HttpStatusCode statusCode)
        {
            Data = data;
            StatusCode = statusCode;
        }
    }

    public record DebugRestResponse<T> : RestResponse<T>
    {
        public readonly string Content;

        public DebugRestResponse(T data, HttpStatusCode statusCode, string content) : base(data, statusCode)
        {
            Content = content;
        }
    }
}