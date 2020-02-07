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
        protected IConfiguration Configuration;
        protected IServiceProvider ServiceProvider;

        protected virtual void SetupDependencies(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), typeof(UnitTestHttpClientFactory), ServiceLifetime.Singleton));
        }

        [SetUp]
        public void Setup()
        {
            Configuration = new UnitTestConfiguration();


            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Configuration);
            serviceCollection.AddDigizuite();
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
            SetupDependencies(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}