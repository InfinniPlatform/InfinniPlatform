﻿using System.Collections.Generic;
using System.Net;
using System.Reflection;

using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.WebApi.Tests.Builders;
using InfinniPlatform.WebApi.Tests.Properties;

using Newtonsoft.Json;

using NUnit.Framework;

using RestSharp;

namespace InfinniPlatform.WebApi.Tests.Services
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ServicesQueryBehavior
    {
        private IHostingService _service;


        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            // регистрация шаблонов
            IServiceTemplateConfiguration registrationConfig =
                new ServiceTemplateConfiguration().CreateDefaultServiceConfiguration();
            registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestGet", "ProcessTestGetVerb",
                                                                                   new ExtensionPointHandlerConfig()
                                                                                       .AsVerb(VerbType.Get));
            registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestPost", "ProcessTestPostVerb",
                                                                                   new ExtensionPointHandlerConfig()
                                                                                       .AsVerb(VerbType.Post));
            registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestPut", "ProcessTestPutVerb",
                                                                                   new ExtensionPointHandlerConfig()
                                                                                       .AsVerb(VerbType.Put));
            registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestDelete", "ProcessTestDeleteVerb",
                                                                                   new ExtensionPointHandlerConfig()
                                                                                       .AsVerb(VerbType.Delete));
            registrationConfig.RegisterServiceTemplate<TestVerbProcessorUploadQuery>("TestUpload",
                                                                                     "ProcessStreamPostVerb",
                                                                                     new ExtensionPointHandlerConfig()
                                                                                         .AsVerb(VerbType.Upload));

            // регистрация экземпляров сервисов
            var container = new ServiceRegistrationContainer(registrationConfig, "DrugsVidal");
            container.AddRegistration("REF_TEST", "TestGet")
                     .AddRegistration("REF_TEST", "TestPost")
                     .AddRegistration("REF_TEST", "TestPut")
                     .AddRegistration("REF_TEST", "TestDelete")
                     .AddRegistration("REF_TEST", "TestUpload");

            // регистрация экземпляров обработчиков
            container.GetRegistration("REF_TEST", "TestGet").RegisterHandlerInstance("testgethandler");
            container.GetRegistration("REF_TEST", "TestPost").RegisterHandlerInstance("testposthandler");
            container.GetRegistration("REF_TEST", "TestPut").RegisterHandlerInstance("testputhandler");
            container.GetRegistration("REF_TEST", "TestDelete").RegisterHandlerInstance("testdeletehandler");
            container.GetRegistration("REF_TEST", "TestUpload").RegisterHandlerInstance("testuploadhandler");

            var factory = new OwinHostingServiceFactory(new List<Assembly> {GetType().Assembly}, registrationConfig);

            _service = factory.CreateHostingService();

            factory.InfinniPlatformHostServer.InstallServices(null, container);

            _service.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _service.Stop();
        }


        [TestCase("testgethandler", true)]
        [TestCase("testget", true)]
        [TestCase("testgetnonexisting", false)]
        public void ShouldMakeGetQueryToService(string handlerName, bool expectedResult)
        {
            var restClient =
                new RestClient(string.Format("{0}://{1}:{2}/DrugsVidal/StandardApi/REF_TEST/{3}",
											 HostingConfig.Default.ServerScheme,
											 HostingConfig.Default.ServerName,
											 HostingConfig.Default.ServerPort, handlerName));

            IRestResponse restResponse =
                restClient.Get(new RestRequest("?query={argument}") {RequestFormat = DataFormat.Json}
                                   .AddUrlSegment("argument",
                                                  JsonConvert.SerializeObject(
                                                      RequestBuilder.BuildTestVerbProcessorComplexArguments())));

            Assert.AreEqual(expectedResult, restResponse.StatusCode != HttpStatusCode.InternalServerError);
        }

        [TestCase("testposthandler", true)]
        [TestCase("testpost", true)]
        [TestCase("testpostnonexisting", false)]
        public void ShouldMakePostQueryToService(string handlerName, bool expectedResult)
        {
            var restClient =
                new RestClient(string.Format("{0}://{1}:{2}/DrugsVidal/StandardApi/REF_TEST/{3}",
											 HostingConfig.Default.ServerScheme,
											 HostingConfig.Default.ServerName,
											 HostingConfig.Default.ServerPort, handlerName));

            IRestResponse restResponse = restClient.Post(new RestRequest {RequestFormat = DataFormat.Json}
                                                             .AddBody(
                                                                 RequestBuilder.BuildTestVerbProcessorComplexArguments()));

            Assert.AreEqual(expectedResult, restResponse.StatusCode != HttpStatusCode.InternalServerError);
        }

        [TestCase("testuploadhandler", true)]
        [TestCase("testupload", true)]
        [TestCase("testuploadnonexisting", false)]
        public void ShouldMakeUploadQueryToService(string handlerName, bool expectedResult)
        {
            var restClient =
                new RestClient(string.Format("{0}://{1}:{2}/DrugsVidal/Upload/REF_TEST/{3}/",
											 HostingConfig.Default.ServerScheme,
											 HostingConfig.Default.ServerName,
											 HostingConfig.Default.ServerPort, handlerName));

            IRestResponse restResponse =
                restClient.Post(new RestRequest("?linkedData={argument}") {RequestFormat = DataFormat.Json}
                                    .AddUrlSegment("argument", "123")
                                    .AddFile("uploadFile", Resources.CheckUploadFile, "fileUploaded", "multipart/form-data"));


            Assert.AreEqual(expectedResult, restResponse.Content.Contains("STREAM_123"));
        }
    }


    public static class RestSharpExtensions
    {
        public static IRestRequest AddUrlSegment(this IRestRequest target, string name, object value)
        {
            return target.AddUrlSegment(name, (value != null)
                                                  ? (target.RequestFormat == DataFormat.Json)
                                                        ? target.JsonSerializer.Serialize(value)
                                                        : target.XmlSerializer.Serialize(value)
                                                  : string.Empty);
        }
    }
}