using System;
using System.Threading.Tasks;
using Digizuite.Models;
using Microsoft.Extensions.DependencyInjection;

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
                BaseUrl = new Uri("https://dam.dev.digizuite.com/"),
                SystemUsername = "",
                SystemPassword = ""
            };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDigizuite(config);

            var services = serviceCollection.BuildServiceProvider();

            var auth = services.GetRequiredService<IDamAuthenticationService>();
            var memberId = await auth.GetMemberId().ConfigureAwait(false);
            
            Console.WriteLine($"Authorized as member {memberId}");
        }
    }
}
