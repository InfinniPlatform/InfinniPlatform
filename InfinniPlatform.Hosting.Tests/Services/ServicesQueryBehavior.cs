using System.Collections.Generic;
using System.Reflection;
using InfinniPlatform.Hosting.Factories;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Hosting.WebApi.Factories;
using InfinniPlatform.Hosting.WebApi.Tests.Builders;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;

namespace InfinniPlatform.Hosting.WebApi.Tests.Services
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public class RestQueryBehavior
	{
		private InfinniPlatformHostServer _multiCareHost;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			//регистрация шаблонов
			var registrationConfig = new ServiceTemplateConfiguration();
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestGet", "ProcessTestGetVerb",handler => handler.AsVerb(VerbType.Get));
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestPost", "ProcessTestPostVerb", handler => handler.AsVerb(VerbType.Post));
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestPut", "ProcessTestPutVerb",handler => handler.AsVerb(VerbType.Put));
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestDelete", "ProcessTestDeleteVerb", handler => handler.AsVerb(VerbType.Delete));

			//регистрация экземпляров сервисов
			var container = new ServiceRegistrationContainer(registrationConfig);
			container.AddRegistration("REF_TEST", "TestGet")
			         .AddRegistration("REF_TEST", "TestPost")
			         .AddRegistration("REF_TEST", "TestPut")
			         .AddRegistration("REF_TEST", "TestDelete");
			
			//регистрация экземпляров обработчиков
			container.GetRegistration("REF_TEST", "TestGet").RegisterHandlerInstance("testgethandler");
			container.GetRegistration("REF_TEST", "TestPost").RegisterHandlerInstance("testposthandler");
			container.GetRegistration("REF_TEST", "TestPut").RegisterHandlerInstance("testputhandler");
			container.GetRegistration("REF_TEST", "TestDelete").RegisterHandlerInstance("testdeletehandler");

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
		[Ignore("Currently can't work")]
		public void ShouldMakeDeleteQueryToService()
		{
			var restClient = new RestClient("http://localhost:9999/DrugsVidal/StandardApi/REF_TEST/testdeletehandler");
			IRestResponse restResponse = restClient.Delete(new RestRequest("&IdList={IdList}") { RequestFormat = DataFormat.Json }
															   .AddUrlSegment("IdList", RequestBuilder.BuildDeleteArguments()));
			Assert.True(restResponse.Content.Contains("\"Data\":\"DELETE\""));
		}

		[TestCase("testgethandler",true)]
		[TestCase("testget",true)]
		[TestCase("testgetnonexisting", false)]
		public void ShouldMakeGetQueryToService(string handlerName, bool expectedResult)
		{
			var restClient = new RestClient(string.Format("http://localhost:9999/DrugsVidal/StandardApi/REF_TEST/{0}",handlerName));
			IRestResponse restResponse = restClient.Get(new RestRequest("?query={argument}") { RequestFormat = DataFormat.Json }
															   .AddUrlSegment("argument", JsonConvert.SerializeObject(RequestBuilder.BuildTestVerbProcessorComplexArguments())));
			Assert.AreEqual(expectedResult, restResponse.Content.Contains("GET"));
		}


		[TestCase("testposthandler",true)]
		[TestCase("testpost",true)]
		[TestCase("testpostnonexisting", false)]
		public void ShouldMakePostQueryToService(string handlerName, bool expectedResult)
		{
			var restClient = new RestClient(string.Format("http://localhost:9999/DrugsVidal/StandardApi/REF_TEST/{0}", handlerName));
			IRestResponse restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															 .AddBody(RequestBuilder.BuildTestVerbProcessorComplexArguments()));
			Assert.AreEqual(expectedResult, restResponse.Content.Contains("POST"));
		}

		[TestCase("testputhandler",true)]
		[TestCase("testput",true)]
		[TestCase("testputnonexisting", false)]
		public void ShouldMakePutQueryToService(string handlerName, bool expectedResult)
		{
			var restClient = new RestClient(string.Format("http://localhost:9999/DrugsVidal/StandardApi/REF_TEST/{0}", handlerName));
			IRestResponse restResponse = restClient.Put(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(RequestBuilder.BuildTestVerbProcessorComplexArguments()));
			Assert.AreEqual(expectedResult, restResponse.Content.Contains("PUT"));
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