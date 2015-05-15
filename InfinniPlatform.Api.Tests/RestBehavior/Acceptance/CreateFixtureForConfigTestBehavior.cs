using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Schema.Prefill;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
    [Ignore("Временно выключен до исправления localhost server")]
	public sealed class CreateFixtureForConfigTestBehavior
	{
		private ILocalHostServer _hostServer;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			_hostServer = TestApi.CreateLocalServer();
			_hostServer.RegisterStartConfiguration("TestCreateDatabase");
			_hostServer.RegisterAssembly("TestCreateDatabase", "InfinniPlatform.Api.Tests.dll");
			_hostServer.Start();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			_hostServer.Stop();
		}

		private DynamicInstance CreateAddress()
		{
			dynamic instance = new DynamicInstance();
			instance.Id = "123";
			instance.DisplayName = "Ленина, 1";
			return instance;
		}


		[Test]
		
		public void ShouldStartConfigurationAndExecuteRequestByLocalRoutingWithoutServerRun()
		{
			//заполняем тестовую базу для работы с документами
			var configurationId = "TestCreateDatabase";
			var documentId = "Patient";

			var testDocumentGenerator = new TestDocumentGenerator(configurationId, documentId);

			testDocumentGenerator.TestDataBuilder
								 .FillProperty("Gender", DefaultPropertyNames.Gender)
								 .FillCalculatedProperty("Id", instance => Guid.NewGuid().ToString())
								 .FillExpressionProperty("Name", instance =>
														 instance.Gender.Id == "1"
									  ? DefaultPropertyNames.LastNameMan
									  : DefaultPropertyNames.LastNameWoman)
								 .FillCalculatedProperty("Address", instance => CreateAddress())
								 .FillCalculatedProperty("Address.DisplayName",
														 instance => instance.Address.Id == "123" ? "г.Челябинск" : "г.Москва")
								 .FillProperty("Policies", DefaultPropertyNames.Policies, 2);

			var watch = Stopwatch.StartNew();

			int times = 100;

			testDocumentGenerator.GenerateTestDocument(times);

			watch.Stop();
			Console.WriteLine(@"written {0} records. Elapsed {1} milliseconds", times, watch.ElapsedMilliseconds);

			IEnumerable<dynamic> documents = new DocumentApi().GetDocument(configurationId, documentId, null, 0, 1);

			Assert.AreEqual(1, documents.Count());
			Assert.IsNotNull(documents.First().Name);
			//вызываем тестовый прикладной модуль 

			dynamic item = RestQueryApi.QueryPostJsonRaw("TestCreateDatabase", "Address", "TestAction", null, null).ToDynamic();
			Assert.AreEqual("Test", item.TestValue);
		}
	}
}