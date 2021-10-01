using System;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Samples.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Digizuite.Samples.Initialization
{
    public static class Initialization
    {
        private static readonly ServiceProvider _serviceProvider;
        
        static Initialization()
        {
            var serviceCollection = new ServiceCollection(); 
            
            var config = new DigizuiteConfiguration()
            {
                BaseUrl = new Uri("https://<Digizuite url>.com/"),
                SystemUsername = "<Username>",
                SystemPassword = "<Password>"
            };
            
            serviceCollection.AddDigizuite(config);
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(SimpleLogger<>));
            _serviceProvider = serviceCollection.BuildServiceProvider(true);
        }

        public static ServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}