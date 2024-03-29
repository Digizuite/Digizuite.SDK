﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class ServiceHttpWrapperTests : IntegrationTestBase<ServiceHttpWrapper>
    {
        [TestCase(ServiceType.DslService, false, false, "api/test", "https://dam.digizuite.com/DigizuiteCore/dsl-service/api/test")]
        [TestCase(ServiceType.DslService, true, false, "api/test", "https://localhost:5063/api/test")]
        [TestCase(ServiceType.DslService, true, true, "api/test", "https://localhost:5063/api/test")]
        [TestCase(ServiceType.DslService, false, true, "api/test", "http://digizuitecore_dslservice/api/test")]
        [TestCase(ServiceType.Dmm3bwsv3, false, false, "api/test", "https://dam.digizuite.com/dmm3bwsv3/api/test")]
        [TestCase(ServiceType.Dmm3bwsv3, true, false, "api/test", "http://local.dev.digizuite.com/dev/dmm3bwsv3/api/test")]
        [TestCase(ServiceType.Dmm3bwsv3, true, true, "api/test", "http://local.dev.digizuite.com/dev/dmm3bwsv3/api/test")]
        [TestCase(ServiceType.Dmm3bwsv3, false, true, "api/test", "https://dam.digizuite.com/dmm3bwsv3/api/test")]
        public void GeneratesCorrectUrl(ServiceType serviceType, bool isDevelopment, bool isInDocker, string path,
            string expectedUrl)
        {
            Configuration.BaseUrl = new Uri("https://dam.digizuite.com");
            Configuration.RunInDocker = isInDocker;
            Configuration.DevelopmentServices = new HashSet<ServiceType>();
            if (isDevelopment)
            {
                Configuration.DevelopmentServices.Add(serviceType);
            }

            var url = Service.GetServiceUrl(serviceType, path);
            Assert.That(url.ToString(), Is.EqualTo(expectedUrl));
        }
    }
}