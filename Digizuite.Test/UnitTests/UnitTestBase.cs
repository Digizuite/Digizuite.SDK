using System;
using Digizuite.Logging;
using Digizuite.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    public abstract class UnitTestBase
    {
        protected IConfiguration Configuration = null!;
        protected IServiceProvider ServiceProvider = null!;

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