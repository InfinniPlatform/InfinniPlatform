using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class VersionApiTest
	{
		private const string Route = "1";

		private IDisposable _server;
		private InfinniVersionApi _versionApi;
		private InfinniSignInApi _signInApi;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = InfinniPlatformInprocessHost.Start();
			_versionApi = new InfinniVersionApi(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort.ToString(), Route);
			_signInApi = new InfinniSignInApi(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort.ToString(), Route);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_server.Dispose();
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


			dynamic result = _versionApi.SetRelevantVersion("Admin", version);

			Assert.AreEqual(true, result.IsValid);
		}
	}
}