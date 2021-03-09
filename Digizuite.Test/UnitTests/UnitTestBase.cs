using System;
using Digizuite.Models;
using Digizuite.Samples;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    public abstract class UnitTestBase
    {
        protected IConfiguration Configuration;
        protected IServiceProvider ServiceProvider;

        protected virtual void SetupDependencies(IServiceCollection services)
        {
        }

        [SetUp]
        public void Setup()
        {
            Configuration = new UnitTestConfiguration();


            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDigizuite(Configuration);
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
            SetupDependencies(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}