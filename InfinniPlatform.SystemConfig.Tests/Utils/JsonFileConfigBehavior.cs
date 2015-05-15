using System;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.File;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests.Utils
{
    [TestFixture]
	[Category(TestCategories.IntegrationTest)]
    public sealed class JsonFileConfigBehavior
    {
		private IDisposable _server;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));
		}

        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldConstructJsonConfiguration()
        {
            var jsonFileConfigManager = new JsonFileConfigManager(Path.GetFullPath(@"TestData\Configurations"));

            jsonFileConfigManager.ReadConfigurations();

            var configList = jsonFileConfigManager.GetConfigurationList();

            
            Assert.True(configList.Select(c => c.ToLowerInvariant()).Contains("classifierstorage"));
            Assert.True(configList.Select(c => c.ToLowerInvariant()).Contains("classifierloader"));
            dynamic config = jsonFileConfigManager.GetJsonFileConfig("classifierstorage");
            Assert.IsNotNull(config);
            Assert.True(config.Documents.Count > 0);

        }
    }
}