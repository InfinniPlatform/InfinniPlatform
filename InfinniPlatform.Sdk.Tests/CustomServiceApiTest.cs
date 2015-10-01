using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
    [TestFixture]
	[Category(TestCategories.IntegrationTest)]
    public sealed class CustomServiceApiTest
    {
        private const string InfinniSessionPort = "9900";
        private const string InfinniSessionServer = "localhost";
        private const string Route = "1";

        private InfinniCustomServiceApi _customServiceApi;
        private InfinniDocumentApi _documentApi;

        [TestFixtureSetUp]
        public void SetupApi()
        {
            _customServiceApi = new InfinniCustomServiceApi(InfinniSessionServer, InfinniSessionPort,Route);
            _documentApi = new InfinniDocumentApi(InfinniSessionServer, InfinniSessionPort,Route);
        }

        [Test]
        public void ShouldInvokeCustomService()
        {
            //Given
            dynamic review = new
            {
                Game = new
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = "X-Com:Enemy Within"
                },
                Likes = 0
            };

            string docId = _documentApi.SetDocument("Gameshop", "review", review).Id.ToString();

            //When
            _customServiceApi.ExecuteAction("Gameshop", "Review", "Like", new
            {
                DocumentId = docId
            });

            //Then
            dynamic documentResult = _documentApi.GetDocumentById("Gameshop", "review", docId);
            Assert.AreEqual(documentResult.Likes,1);
        }

    }
}
