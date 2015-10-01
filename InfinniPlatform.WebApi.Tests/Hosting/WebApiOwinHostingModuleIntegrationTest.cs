using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Sdk.Api;
using NUnit.Framework;

namespace InfinniPlatform.WebApi.Tests.Hosting
{
    [Ignore]
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class WebApiOwinHostingModuleIntegrationTest
    {
        private static readonly HostingConfig HostingConfig = TestSettings.DefaultHostingConfig;


        [Test]
        public void ShouldHostWebApi()
        {
            // Given

            var requestUri =
                new Uri(string.Format("{0}://{1}:{2}/api/Test/123", HostingConfig.ServerScheme, HostingConfig.ServerName,
                                      HostingConfig.ServerPort));

            var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));

            var templateConfig = new ServiceTemplateConfiguration();
            var module = new WebApiOwinHostingModule(new List<Assembly>(),
                                                     new ServiceRegistrationContainerFactory(templateConfig),
                                                     templateConfig);
            hosting.RegisterModule(module);

            // When
            hosting.Start();
            var response = WebRequestHelper.Get(requestUri).AsObject<string>();
            hosting.Stop();

            // Then
            Assert.AreEqual("123", response);
        }
    }

    public sealed class TestController : ApiController
    {
        [ActionName("123")]
        public string Get(string id)
        {
            return id;
        }
    }
}