using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture]
    public sealed class CustomServiceApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string InfinniSessionVersion = "1";
        private InfinniCustomServiceApi _api;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _api = new InfinniCustomServiceApi(InfinniSessionServer, InfinniSessionPort, InfinniSessionVersion);
        }

        [Test]
        public void ShouldInvokeCustomService()
        {
            //_api.ExecuteAction("Gameshop","")
        }
    }
}
