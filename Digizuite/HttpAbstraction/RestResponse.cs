using System;
using System.Net;

namespace Digizuite.HttpAbstraction
{
    public record BaseRestResponse(HttpStatusCode StatusCode, Exception? Exception, string? ActivityId)
    {
        public bool IsSuccessful => Exception == null && StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
    }
    
    
    public record RestResponse(HttpStatusCode StatusCode, Exception? Exception, string Content, string? ActivityId) 
        : BaseRestResponse(StatusCode, Exception, ActivityId);

    public record RestResponse<T>(HttpStatusCode StatusCode, Exception? Exception, T Data, string? ActivityId) 
        : BaseRestResponse(StatusCode, Exception, ActivityId);

    public record DebugRestResponse<T>(HttpStatusCode StatusCode, Exception? Exception, T Data, string? Content, string? ActivityId) 
        : RestResponse<T>(StatusCode, Exception, Data, ActivityId);
}