using System;

using InfinniPlatform.Sdk.Api;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests
{
	//[Ignore("Тесты SDK не выполняют запуск сервера InfinniPlatform. Необходимо существование уже запущенного сервера на localhost : 9900")]
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class SignInApiTest
	{
		private const string InfinniSessionPort = "9900";
		private const string InfinniSessionServer = "localhost";
		private const string Route = "1";

		private InfinniSignInApi _signInApi;

		[TestFixtureSetUp]
		public void SetupApi()
		{
			_signInApi = new InfinniSignInApi(InfinniSessionServer, InfinniSessionPort, Route);
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