using System;
using Digizuite.Models;
using Digizuite.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    public abstract class UnitTestBase
    {
        protected Configuration Configuration;

        protected IServiceProvider ServiceProvider;

        protected virtual void SetupDependencies(IServiceCollection services)
        {
//            services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), typeof(UnitTestHttpClientFactory), ServiceLifetime.Singleton));
        }

        [SetUp]
        public void Setup()
        {
            var apiUrl = "https://unittest-dam.example.local";
            var username = "fictionaluser";
            var password = "fictionalpassword";
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