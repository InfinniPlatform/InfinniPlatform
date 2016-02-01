using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CustomServiceApiTest
	{
		private IDisposable _server;
		private CustomApiClient _customApiClient;
		private DocumentApiClient _documentApiClient;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = InfinniPlatformInprocessHost.Start();
			_customApiClient = new CustomApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port);
			_documentApiClient = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);
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

			string docId = _documentApiClient.SetDocument("Gameshop", "review", review).Id.ToString();

			//When
			_customApiClient.ExecuteAction("Gameshop", "Review", "Like", new
			{
				DocumentId = docId
			});

			//Then
			dynamic documentResult = _documentApiClient.GetDocumentById("Gameshop", "review", docId);
			Assert.AreEqual(documentResult.Likes, 1);
		}

	}
}