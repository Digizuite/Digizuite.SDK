using System;
using Digizuite.Models;
using Digizuite.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Digizuite.Test
{
    public abstract class IntegrationTestBase
    {
        protected Configuration Configuration;

        protected IServiceProvider ServiceProvider;

        protected virtual void SetupDependencies(IServiceCollection services)
        {
            // services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), typeof(MakeFakeHttpClientFactory), ServiceLifetime.Singleton))
        }
        
        [SetUp]
        public void Setup()
        {
            var apiUrl = Environment.GetEnvironmentVariable("DIGIZUITE_API_URL");
            var username = Environment.GetEnvironmentVariable("DIGIZUITE_USERNAME");
            var password = Environment.GetEnvironmentVariable("DIGIZUITE_PASSWORD");
            var accessKeyDuration = new TimeSpan(2, 0, 0);

            if (string.IsNullOrWhiteSpace(apiUrl)) throw new ArgumentException("apiUrl was not set");

            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username was not set");

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password was not set");

            Configuration = new Configuration
            {
                BaseUrl = apiUrl,
                SystemUsername = username,
                SystemPassword = password,
                AccessKeyDuration = accessKeyDuration
            };


            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Configuration);
            serviceCollection.AddDigizuite();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
            SetupDependencies(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}