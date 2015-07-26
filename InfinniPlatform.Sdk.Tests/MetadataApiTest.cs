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
    }
}
