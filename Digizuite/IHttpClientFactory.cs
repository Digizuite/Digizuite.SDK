using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace Digizuite
{
    public interface IHttpClientFactory
    {
        IRestClient GetRestClient();
    }
}
