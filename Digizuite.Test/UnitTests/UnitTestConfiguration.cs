using System;
using Digizuite.Models;

namespace Digizuite.Test.UnitTests
{
    internal class UnitTestConfiguration : IConfiguration
    {
        public string BaseUrl
        {
            get => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
            set => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }

        public TimeSpan AccessKeyDuration
        {
            get => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
            set => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }

        public string SystemUsername
        {
            get => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
            set => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }

        public string SystemPassword
        {
            get => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
            set => throw new NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }
    }
}
