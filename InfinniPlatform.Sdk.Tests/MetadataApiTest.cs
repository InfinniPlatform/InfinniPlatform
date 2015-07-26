using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Metadata;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture]
    public class MetadataApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";

        private InfinniMetadataApi _metadataApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _metadataApi = new InfinniMetadataApi(InfinniSessionServer, InfinniSessionPort);
        }

        [Test]
        public void ShouldInsertUpdateDeleteSolution()
        {
            var solution = new SolutionMetadata();
            solution.Id = Guid.NewGuid().ToString();
            solution.Name = "TestSolutionVersion_"+Guid.NewGuid().ToString();
            solution.Version = "2.0.0.0";

            _metadataApi.InsertSolution(solution);
            dynamic solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);

            Assert.IsNotNull(solutionRead);

            solution.Name = "TestSolutionVersion_" + Guid.NewGuid().ToString() + "Changed";
            _metadataApi.UpdateSolution(solution);

            solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);
            Assert.AreEqual(solution.Name, solutionRead.Name);

            _metadataApi.DeleteSolution(solution.Version, solution.Name);
            solutionRead = _metadataApi.GetSolution(solution.Version, solution.Name);
            Assert.IsNull(solutionRead);
        }

        [Test]
        public void ShouldInsertUpdateDeleteConfig()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";

            _metadataApi.InsertConfig(config);
            dynamic configRead = _metadataApi.GetConfig(config.Version, config.Name);

            Assert.IsNotNull(configRead);

            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString() + "Changed";
            _metadataApi.UpdateConfig(config);

            configRead = _metadataApi.GetConfig(config.Version, config.Name);
            Assert.AreEqual(config.Name, configRead.Name);

            _metadataApi.DeleteConfig(config.Version, config.Name);
            configRead = _metadataApi.GetConfig(config.Version, config.Name);
            Assert.IsNull(configRead);
        }

        [Test()]
        public void ShouldInsertUpdateDeleteMenu()
        {
            var config = new ConfigurationMetadata();
            config.Id = Guid.NewGuid().ToString();
            config.Name = "TestConfigVersion_" + Guid.NewGuid().ToString();
            config.Version = "2.0.0.0";
            _metadataApi.InsertConfig(config);

            var menu = new MenuMetadata();
            menu.Id = Guid.NewGuid().ToString();
            menu.Name = "MainMenu";

            _metadataApi.InsertMenu(menu, config.Version, config.Name);

            var menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNotNull(menuRead);

            menu.Name = "MainMenu_v1";

            _metadataApi.UpdateMenu(menu, config.Version, config.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.AreEqual(menuRead.Name, menu.Name);

            _metadataApi.DeleteMenu(config.Version, config.Name, menu.Name);

            menuRead = _metadataApi.GetMenu(config.Version, config.Name, menu.Name);

            Assert.IsNull(menuRead);

            _metadataApi.DeleteConfig(config.Version, config.Name);

        }
    }
}
