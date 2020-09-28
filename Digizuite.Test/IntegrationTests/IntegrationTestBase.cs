using System;
using Digizuite.Models;
using Digizuite.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected IConfiguration Configuration;

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

            Configuration = new DigizuiteConfiguration
            {
                BaseUrl = new Uri(apiUrl),
                SystemUsername = username,
                SystemPassword = password,
                AccessKeyDuration = accessKeyDuration
            };
            var serviceCollection = new ServiceCollection();


            serviceCollection.AddSingleton(Configuration);
            serviceCollection.AddDigizuite();
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

    public class IntegrationTestBase<T> : IntegrationTestBase
    {
        public T Service => ServiceProvider.GetRequiredService<T>();
    }
}