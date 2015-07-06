using System;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests.Configuration
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class GeneratorBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldGenerateGeneratorView()
        {
            string configurationId = "Integration";

            MetadataManagerConfiguration manager = ManagerFactoryConfiguration.BuildConfigurationManager(null);

            dynamic item = manager.CreateItem(configurationId);
            manager.DeleteItem(item);

            dynamic assembly = new DynamicWrapper();
            assembly.Name = "InfinniPlatform.SystemConfig.Tests";

            item.Assemblies.Add(assembly);

            manager.MergeItem(item);

            //создаем метаданные справочника для тестирования
            var builder = new RestQueryBuilder("SystemConfig", "metadata", "creategenerator", null);

            var eventObject = new
                {
                    GeneratorName = "TestView",
                    ActionUnit = "ActionUnitGeneratorTest",
                    Configuration = configurationId,
                    Metadata = "Common",
                    MetadataType = MetadataType.View,
                    ContextTypeKind = ContextTypeKind.ApplyMove
                };

            builder.QueryPostJson(null, eventObject);

            dynamic package = new PackageBuilder().BuildPackage(configurationId, null,
                                                                "InfinniPlatform.SystemConfig.Tests.dll");

            new UpdateApi(null).InstallPackages(new[] {package});

            RestQueryApi.QueryPostNotify(null, configurationId);

            new UpdateApi(null).UpdateStore(configurationId);

            //генерируем метаданные напрямую
            builder = new RestQueryBuilder("SystemConfig", "metadata", "generatemetadata", null);

            var body = new
                {
                    Metadata = "common",
                    GeneratorName = "TestView",
                    Configuration = configurationId
                };

            dynamic result = builder.QueryPostJson(null, body).ToDynamic();
            Assert.AreEqual(result.TestValue, "Test");

            //получаем сгенерированное представление через менеджер метаданных

            var bodyMetadata = new
                {
                    Configuration = configurationId,
                    MetadataObject = "common",
                    MetadataType = MetadataType.View,
                    MetadataName = "TestView"
                };

            builder = new RestQueryBuilder("SystemConfig", "metadata", "getmanagedmetadata", null);
            result = builder.QueryPostJson(null, bodyMetadata).ToDynamic();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TestValue, "Test");
        }
    }
}