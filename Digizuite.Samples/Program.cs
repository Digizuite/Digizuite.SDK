using System;
using Digizuite.Models;

namespace Digizuite.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }



        static void Test1()
        {
            var config = new Configuration()
            {
                AccessKeyDuration = new TimeSpan(86400000L),
                BaseUrl = "https://dam.dev.digizuite.com/",
                SystemUsername = "",
                SystemPassword = ""
            };
            var httpClient = new HttpClientFactory(config);
            
            var auth = new DamAuthenticationService(config, httpClient, null);

            

        }
    }
}
