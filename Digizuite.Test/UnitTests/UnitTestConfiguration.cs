using System;
using System.Collections.Generic;
using Digizuite.Models;

namespace Digizuite.Test.UnitTests
{
    internal class UnitTestConfiguration : IConfiguration
    {
        public Uri BaseUrl
        {
            get => Disallow<Uri>();
            set => Disallow(value);
        }

        public TimeSpan AccessKeyDuration
        {
            get => Disallow<TimeSpan>();
            set => Disallow(value);
        }

        public string SystemUsername
        {
            get => Disallow<string>();
            set => Disallow(value);
        }

        public string SystemPassword
        {
            get => Disallow<string>();
            set => Disallow(value);
        }

        public string? ConfigVersionId
        {
            get => Disallow<string>();
            set => Disallow(value);
        }

        public HashSet<ServiceType> DevelopmentServices
        {
            get => Disallow<HashSet<ServiceType>>();
            set => Disallow(value);
        }

        // ReSharper disable once UnusedParameter.Local
        private static T Disallow<T>(T _ = default)
        {
            throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }
    }
}