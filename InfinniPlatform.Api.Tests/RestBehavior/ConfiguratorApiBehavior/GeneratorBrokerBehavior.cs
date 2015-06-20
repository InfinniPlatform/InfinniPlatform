using System;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.ConfiguratorApiBehavior
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class GeneratorBrokerBehavior
    {
        private IDisposable _server;
        private string _configurationId = "testconfig_generator";

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

        private void CreateTestConfiguration()
        {
            MetadataManagerConfiguration manager = ManagerFactoryConfiguration.BuildConfigurationManager(null);
            dynamic item = manager.CreateItem(_configurationId);
            manager.DeleteItem(item);
            manager.MergeItem(item);
            RestQueryApi.QueryPostNotify(null, _configurationId); //загружаем обновленную конфигурацию
            new UpdateApi(null).UpdateStore(_configurationId);
        }

        [Test]
        public void ShouldCreateAndDeleteGenerator()
        {
            //given
            CreateTestConfiguration(); //создаем тестовую конфигурацию для проверки
            var broker = new GeneratorBroker(null, _configurationId, "common");
            //when
            string testGeneratorName = "TestGenerator";
            var eventObject = new
                {
                    GeneratorName = testGeneratorName,
                    ActionUnit = "ActionUnitGeneratorTest",
                    MetadataType = MetadataType.View,
                };

            broker.CreateGenerator(eventObject);
            //then
            var manager = new ManagerFactoryDocument(null, _configurationId, "common");

            IDataReader managerGenerator = manager.BuildGeneratorMetadataReader();
            IDataReader managerScenario = manager.BuildScenarioMetadataReader();
            IDataReader managerService = manager.BuildServiceMetadataReader();
            IDataReader managerProcess = manager.BuildProcessMetadataReader();

            dynamic generator = managerGenerator.GetItem(testGeneratorName);
            Assert.IsNotNull(generator);

            dynamic scenario = managerScenario.GetItem(testGeneratorName);
            Assert.IsNotNull(scenario);

            dynamic service = managerService.GetItem(testGeneratorName);
            Assert.IsNotNull(service);

            dynamic process = managerProcess.GetItem(testGeneratorName);
            Assert.IsNotNull(process);

            //when
            broker.DeleteGenerator(testGeneratorName);

            //then
            generator = managerGenerator.GetItem(testGeneratorName);
            Assert.IsNull(generator);

            scenario = managerScenario.GetItem(testGeneratorName);
            Assert.IsNull(scenario);

            service = managerService.GetItem(testGeneratorName);
            Assert.IsNull(service);

            process = managerProcess.GetItem(testGeneratorName);
            Assert.IsNull(process);
        }
    }
}