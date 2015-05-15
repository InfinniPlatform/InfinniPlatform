using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using InfinniPlatform.Hosting.Factories;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Hosting.WebApi.Factories;
using InfinniPlatform.Hosting.WebApi.Tests.Builders;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;

namespace InfinniPlatform.Hosting.WebApi.Tests.Services
{

	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public class ApiControllerBehavior
	{
		private InfinniPlatformHostServer _multiCareHost;


		[Test]
		public void ShouldBindRestParametersCorrectWithAcceptedTimeout()
		{
			var registrationConfig = InfinniPlatformHostFactory.CreateDefaultServiceConfiguration(new ServiceTemplateConfiguration());

			//регистрация шаблонов сервисов 
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestGet", "ProcessTestGetVerb",handler => handler.AsVerb(VerbType.Get));

			//регистрация экземпляров сервисов
			var container = new ServiceRegistrationContainer(registrationConfig);
			container.AddRegistration("REF_TEST", "TestGet");
			
			//регистрация экземпляров обработчиков
			container.GetRegistration("REF_TEST", "TestGet").RegisterHandlerInstance("testgetinstance");


			_multiCareHost = InfinniPlatformHostServer.ConstructHostServer("http://localhost:9999", new List<Assembly>()
				                                                                                  {
					                                                                                  this.GetType().Assembly
				                                                                                  },registrationConfig);
			_multiCareHost.InstallServices(container);
			_multiCareHost.StartServer();

			try
			{
                var restClient = new RestClient("http://localhost:9999/DrugsVidal/StandardApi/REF_TEST/testgetinstance");
				var watch = Stopwatch.StartNew();
				for (int i = 0; i < 10; i++)
				{
                    
                    IRestResponse restResponse = restClient.Get(new RestRequest("?query={argument}") { RequestFormat = DataFormat.Json }
                                                                       .AddUrlSegment("argument", JsonConvert.SerializeObject(RequestBuilder.BuildTestVerbProcessorComplexArguments()))); 
                    Assert.True(restResponse.Content.Contains("GET"));
				}
				watch.Stop();
				Assert.LessOrEqual(watch.ElapsedMilliseconds/10000,20);
			}
			finally
			{
				_multiCareHost.StopServer();

			}
		}



	}

}