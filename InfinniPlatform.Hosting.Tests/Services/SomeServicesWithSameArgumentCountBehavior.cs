using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.Hosting.Factories;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Hosting.WebApi.Factories;
using InfinniPlatform.Hosting.WebApi.Tests.Builders;
using NUnit.Framework;
using RestSharp;

namespace InfinniPlatform.Hosting.WebApi.Tests.Services
{
    [TestFixture]
	[Category(TestCategories.IntegrationTest)]
    public class SomeServicesWithSameArgumentCountBehavior
    {
	    private InfinniPlatformHostServer _multiCareHost;

	    [TestFixtureSetUp]
        public void FixtureSetup()
        {

			//регистрация шаблонов
			var registrationConfig = new ServiceTemplateConfiguration();
			registrationConfig.RegisterServiceTemplate<AnotherVerbProcessorHttpQuery>("TestPost", "ProcessTestPostVerb", handler => handler.AsVerb(VerbType.Post));
			registrationConfig.RegisterServiceTemplate<AnotherVerbProcessor2HttpQuery>("TestGet", "ProcessTestGetVerb", handler => handler.AsVerb(VerbType.Post));


			//регистрация экземпляров сервисов
			var container = new ServiceRegistrationContainer(registrationConfig);
		    container.AddRegistration("AnotherVerb", "TestPost")
					 .AddRegistration("AnotherVerb2", "TestGet");

			//регистрация экземпляров обработчиков
			container.GetRegistration("AnotherVerb", "TestPost").RegisterHandlerInstance("testpost");
			container.GetRegistration("AnotherVerb2", "TestGet").RegisterHandlerInstance("testget");


			_multiCareHost = InfinniPlatformHostServer.ConstructHostServer("http://localhost:9999", new List<Assembly>()
				                                                                                  {
					                                                                                  this.GetType().Assembly
				                                                                                  }, registrationConfig);

			_multiCareHost.InstallServices(container);
			_multiCareHost.StartServer();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
			_multiCareHost.StopServer();
        }

        [Test]
        public void ShouldSelectCorrectServiceMethod()
        {
            var restClient = new RestClient("http://localhost:9999/DrugsVidal/StandardApi/AnotherVerb/testpost");
            IRestResponse restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
                                                             .AddBody(RequestBuilder.BuildSomeServicesCheckRequestAnother()));
            Assert.True(restResponse.Content.Contains("POST2"));

            restClient = new RestClient("http://localhost:9999/DrugsVidal/StandardApi/AnotherVerb2/testget");
            restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
                                                             .AddBody(RequestBuilder.BuildSomeServicesCheckRequestAnother2()));

            Assert.True(restResponse.Content.Contains("POST3"));

        }
    }
}
