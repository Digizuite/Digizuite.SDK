using System;
using System.Collections.Generic;
using Digizuite.Logging;
using Digizuite.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected IConfiguration Configuration = null!;

        protected IServiceProvider ServiceProvider = null!;

        protected virtual void SetupDependencies(IServiceCollection services)
        {
        }

        protected virtual DigizuiteOption Options => DigizuiteOption.Nothing;
        
        [SetUp]
        public void Setup()
        {
            var apiUrl = Environment.GetEnvironmentVariable("DIGIZUITE_API_URL");
            var username = Environment.GetEnvironmentVariable("DIGIZUITE_USERNAME");
            var password = Environment.GetEnvironmentVariable("DIGIZUITE_PASSWORD");

            if (string.IsNullOrWhiteSpace(apiUrl)) throw new ArgumentException("apiUrl was not set");

            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username was not set");

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password was not set");

            var cfg = new DigizuiteConfiguration
            {
                BaseUrl = new Uri(apiUrl),
                SystemUsername = username,
                SystemPassword = password,
                DevelopmentServices = new HashSet<ServiceType>
                {
                    ServiceType.TranscodeService
                }
            };
            var serviceCollection = new ServiceCollection();

            Configuration = cfg;

            serviceCollection.AddDigizuite(cfg, Options);
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
            SetupDependencies(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions()
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
        }
        
        
        protected static void ReplaceWith<TReplaced, TReplacer>(IServiceCollection services)
            where TReplaced : class
            where TReplacer : new()
        {
            services.Replace(new ServiceDescriptor(typeof(TReplaced), new TReplacer()));
        }
        
        
        protected TAs Get<TType, TAs>()
            where TType : class
            where TAs : class, TType
        {
            if (ServiceProvider.GetRequiredService<TType>() is TAs type)
            {
                return type;
            }

            throw new ArgumentException("TAs was not of the correct type to cast to TType");
        }

    }

    public class IntegrationTestBase<TService> : IntegrationTestBase where TService : class
    {
        protected TService Service => ServiceProvider.GetRequiredService<TService>();

        protected override void SetupDependencies(IServiceCollection services)
        {
            services.AddSingleton<TService>();
            base.SetupDependencies(services);
        }
    }
}