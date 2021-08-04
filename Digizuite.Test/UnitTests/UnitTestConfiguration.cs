using System;
using System.Collections.Generic;
using Digizuite.Models;

#nullable disable
namespace Digizuite.Test.UnitTests
{
    internal class UnitTestConfiguration : IConfiguration
    {
        public Uri BaseUrl => Disallow<Uri>();

        public string SystemUsername => Disallow<string>();

        public string SystemPassword => Disallow<string>();

        public string ConfigVersionId => Disallow<string>();

        public HashSet<ServiceType> DevelopmentServices => Disallow<HashSet<ServiceType>>();

        public long UploadChunkSize => Disallow<long>();

        public bool RunInDocker => Disallow<bool>();

        // ReSharper disable once UnusedParameter.Local
        private static T Disallow<T>(T _ = default)
        {
            throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }
    }
}