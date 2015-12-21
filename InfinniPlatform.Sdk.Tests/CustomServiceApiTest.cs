using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CustomServiceApiTest
	{
		private const string Route = "1";

		private IDisposable _server;
		private InfinniCustomServiceApi _customServiceApi;
		private InfinniDocumentApi _documentApi;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = InfinniPlatformInprocessHost.Start();
			_customServiceApi = new InfinniCustomServiceApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
			_documentApi = new InfinniDocumentApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_server.Dispose();
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
			Assert.AreEqual(documentResult.Likes, 1);
		}

	}
}