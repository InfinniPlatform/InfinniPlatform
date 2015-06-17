using System;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;

using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class RealConfigControllerBehavior
	{
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

			TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_server.Dispose();
		}

		private string configurationId = "Integration";

		private void GenerateTestConfig()
		{
			var manager = ManagerFactoryConfiguration.BuildConfigurationManager(null);
			var item = manager.CreateItem(configurationId);
			manager.DeleteItem(item);
			manager.MergeItem(item);
		}



		[Test]
		public void ShouldGenerateServiceWithoutState()
		{
			GenerateTestConfig();

			//создаем метаданные справочника для тестирования
			var builder = new RestQueryBuilder(null,"SystemConfig", "metadata", "generateservicewithoutstate", null);

			var eventObject = new
			{
				ActionUnit = "FakeActionUnit",
				ActionName = "FakeActionName",
				Configuration = configurationId,
				ContextTypeKind = ContextTypeKind.ApplyMove,
				Metadata = "Common"
			};

			var response = builder.QueryPostJson(null, eventObject);

			var factory = new ManagerFactoryDocument(null, configurationId, "Common");
			var readerServices = factory.BuildServiceMetadataReader();
			var readerProcesses = factory.BuildProcessMetadataReader();
			var readerScenario = factory.BuildScenarioMetadataReader();

			dynamic service = readerServices.GetItem("FakeActionName");
			Assert.IsNotNull(service);

			dynamic scenario = readerScenario.GetItem("FakeActionName");
			Assert.IsNotNull(scenario);

			dynamic process = readerProcesses.GetItem("FakeActionName");
			Assert.IsNotNull(process);
		}

		[Test]
		public void ShouldCreateGenerator()
		{


			GenerateTestConfig();
			//создаем метаданные справочника для тестирования
			var builder = new RestQueryBuilder(null,"SystemConfig", "metadata", "creategenerator", null);

			var eventObject = new
			{
				GeneratorName = "FakeGenerator",
				ActionUnit = "FakeActionUnit",
				Configuration = configurationId,
				Metadata = "Common",
				MetadataType = MetadataType.Menu,
				ContextTypeKind = ContextTypeKind.ApplyMove
			};

			var response = builder.QueryPostJson(null, eventObject);

			var factory = new ManagerFactoryDocument(null, configurationId, "Common");
			var readerServices = factory.BuildServiceMetadataReader();
			var readerProcesses = factory.BuildProcessMetadataReader();
			var readerScenario = factory.BuildScenarioMetadataReader();
			var readerGenerator = factory.BuildGeneratorMetadataReader();

			dynamic service = readerServices.GetItem("FakeGenerator");
			Assert.IsNotNull(service);

			dynamic scenario = readerScenario.GetItem("FakeGenerator");
			Assert.IsNotNull(scenario);

			dynamic process = readerProcesses.GetItem("FakeGenerator");
			Assert.IsNotNull(process);

			dynamic generator = readerGenerator.GetItem("FakeGenerator");
			Assert.IsNotNull(generator);
		}

	}
}
