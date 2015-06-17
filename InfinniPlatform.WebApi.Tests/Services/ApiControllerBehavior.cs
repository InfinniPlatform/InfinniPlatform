﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using InfinniPlatform.Api.Hosting;
using Newtonsoft.Json;

using NUnit.Framework;

using RestSharp;

using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.WebApi.Tests.Builders;

namespace InfinniPlatform.WebApi.Tests.Services
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public class ApiControllerBehavior
	{
		private IHostingService _service;


		[Test]
		public void ShouldBindRestParametersCorrectWithAcceptedTimeout()
		{
			var registrationConfig = new ServiceTemplateConfiguration().CreateDefaultServiceConfiguration();

			// регистрация шаблонов сервисов 
			registrationConfig.RegisterServiceTemplate<TestVerbProcessorHttpQuery>("TestGet", "ProcessTestGetVerb", new ExtensionPointHandlerConfig().AsVerb(VerbType.Get));

			// регистрация экземпляров сервисов
			var container = new ServiceRegistrationContainer(registrationConfig, "DrugsVidal");
			container.AddRegistration("REF_TEST", "TestGet");

			// регистрация экземпляров обработчиков
			container.GetRegistration("REF_TEST", "TestGet").RegisterHandlerInstance("testgetinstance");

			var factory = new OwinHostingServiceFactory(new List<Assembly> { GetType().Assembly }, registrationConfig);

			_service = factory.CreateHostingService();

			factory.InfinniPlatformHostServer.InstallServices(null, container);

			_service.Start();

			try
			{
				var restClient = new RestClient(string.Format("{0}://{1}:{2}/DrugsVidal/StandardApi/REF_TEST/testgetinstance", TestSettings.DefaultHostingConfig.ServerScheme, TestSettings.DefaultHostingConfig.ServerName, TestSettings.DefaultHostingConfig.ServerPort));
				var watch = Stopwatch.StartNew();

				for (var i = 0; i < 10; i++)
				{

					var restResponse = restClient.Get(new RestRequest("?query={argument}") { RequestFormat = DataFormat.Json }
						.AddUrlSegment("argument", JsonConvert.SerializeObject(RequestBuilder.BuildTestVerbProcessorComplexArguments())));

					Assert.True(restResponse.Content.Contains("GET"));
				}

				watch.Stop();

				Assert.LessOrEqual(watch.ElapsedMilliseconds / 10000, 20);
			}
			finally
			{
				_service.Stop();
			}
		}
	}
}