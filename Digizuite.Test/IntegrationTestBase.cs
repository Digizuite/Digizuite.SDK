using System;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Samples;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test
{
    public abstract class IntegrationTestBase
    {
        protected Configuration _configuration;

        protected IServiceProvider serviceProvider;
        
        [SetUp]
        public void Setup()
        {
             var apiUrl = Environment.GetEnvironmentVariable("DIGIZUITE_API_URL");
             var username = Environment.GetEnvironmentVariable("DIGIZUITE_USERNAME");
             var password = Environment.GetEnvironmentVariable("DIGIZUITE_PASSWORD");
             var accessKeyDuration = new TimeSpan(2, 0, 0);

             _configuration = new Configuration()
             {
                 BaseUrl = apiUrl,
                 SystemUsername = username,
                 SystemPassword = password,
                 AccessKeyDuration = accessKeyDuration
             };
             
             
             
             var serviceCollection = new ServiceCollection();

             serviceCollection.AddSingleton(_configuration);
             serviceCollection.AddDigizuite();
             serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));


             serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}