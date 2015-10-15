using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class SignInApiTest
	{
		private const string Route = "1";

		private IDisposable _server;
		private InfinniSignInApi _signInApi;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_server = InfinniPlatformInprocessHost.Start();
			_signInApi = new InfinniSignInApi(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort.ToString(), Route);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			_server.Dispose();
		}

		[Test]
		public void ShouldSignInAndOut()
		{
			Console.WriteLine(@"SignInInternal:");
			TestDelegate signInInternal = () => Console.WriteLine(_signInApi.SignInInternal("Admin", "Admin", true));
			Assert.DoesNotThrow(signInInternal);

			Console.WriteLine(@"SignOut:");
			TestDelegate signOut = () => Console.WriteLine(_signInApi.SignOut());
			Assert.DoesNotThrow(signOut);
		}
	}
}