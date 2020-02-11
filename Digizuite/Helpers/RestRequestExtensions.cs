using System;
using RestSharp;

namespace Digizuite.Helpers
{
    public static class RestRequestExtensions
    {
        /// <summary>
        /// Call this method to make sure your request can survive the digizuite api...
        /// </summary>
        /// <param name="request"></param>
        public static IRestRequest MakeRequestDamSafe(this IRestRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.OnBeforeDeserialization = response => { response.ContentType = "application/json"; };
            return request;
        }
    }
}
