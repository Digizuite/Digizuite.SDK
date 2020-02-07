using System;
using Digizuite.Models;

namespace Digizuite.Test.UnitTests
{
    internal class UnitTestConfiguration : IConfiguration
    {
        static T Disallow<T>()
        {
            throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }

        public string BaseUrl
        {
            get => Disallow<string>();
            set => Disallow<string>();
        }

        public TimeSpan AccessKeyDuration
        {
            get => Disallow<TimeSpan>();
            set => Disallow<TimeSpan>();
        }

        public string SystemUsername
        {
            get => Disallow<string>();
            set => Disallow<string>();
        }

        public string SystemPassword
        {
            get => Disallow<string>();
            set => Disallow<string>();
        }
    }
}
