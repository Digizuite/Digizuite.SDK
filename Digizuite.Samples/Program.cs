using System;
using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite.Samples
{
    class Program
    {
        static async Task Main()
        {
            await Test1();
        }



        static async Task Test1()
        {
            var config = new Configuration()
            {
                AccessKeyDuration = new TimeSpan(86400000L),
                BaseUrl = "https://dam.dev.digizuite.com/",
                SystemUsername = "",
                SystemPassword = ""
            };
            var httpClient = new HttpClientFactory(config, new ConsoleLogger<HttpClientFactory>());
            
            var auth = new DamAuthenticationService(config, httpClient, new ConsoleLogger<DamAuthenticationService>());

            var memberId = await auth.GetMemberId();
            
            Console.WriteLine($"Authorized as member {memberId}");



        }
    }
}
