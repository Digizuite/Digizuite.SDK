using System;
using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite.Samples
{
    class Program
    {
        static async Task Main()
        {
            await Test1().ConfigureAwait(false);
        }



        static async Task Test1()
        {
            var config = new DigizuiteConfiguration()
            {
                AccessKeyDuration = new TimeSpan(86400000L),
                BaseUrl = new Uri("https://dam.dev.digizuite.com/"),
                SystemUsername = "",
                SystemPassword = ""
            };
            var restClient = new DamRestClient(config, new ConsoleLogger<DamRestClient>());
            using var auth = new DamAuthenticationService(config, restClient, new ConsoleLogger<DamAuthenticationService>());

            var memberId = await auth.GetMemberId().ConfigureAwait(false);
            
            Console.WriteLine($"Authorized as member {memberId}");
        }
    }
}
