using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    //[Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
    [TestFixture]
	[Category(TestCategories.IntegrationTest)]
    public sealed class VersionApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "192.168.0.17";
        private const string Route = "1";

        private InfinniVersionApi _versionApi;
        private InfinniSignInApi _signInApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _versionApi = new InfinniVersionApi(InfinniSessionServer, InfinniSessionPort,Route);
            _signInApi = new InfinniSignInApi(InfinniSessionServer, InfinniSessionPort,Route);
        }

        [Test]
        public void ShouldGetIrrelevantVersions()
        {
            _signInApi.SignInInternal("Admin", "Admin", false);

            dynamic result = _versionApi.GetIrrelevantVersions("Admin");
        }

        [Test]
        public void ShouldSetRelevantVersions()
        {
            _signInApi.SignInInternal("Admin", "Admin", false);

            var version =
                new
                    {
                        ConfigurationId = "TestConfig",
                        Version = "4.1"
                    };
                

            dynamic result = _versionApi.SetRelevantVersion("Admin",version);

            Assert.AreEqual(true,result.IsValid);
        }
    }
}
