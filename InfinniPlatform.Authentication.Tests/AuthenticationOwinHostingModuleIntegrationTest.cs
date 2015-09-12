using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Security;

using Microsoft.Owin;

using NUnit.Framework;

using Owin;

namespace InfinniPlatform.Authentication.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class AuthenticationOwinHostingModuleIntegrationTest
	{
		private static readonly HostingConfig HostingConfig = TestSettings.DefaultHostingConfig;


		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void ShouldSignInInternal(bool remember)
		{
			// GIVEN

			var requests = new List<IPrincipal>();

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));

			var userStore = new MemoryApplicationUserStore();
			var passwordHasher = new FakeApplicationUserPasswordHasher();

			var user = new ApplicationUser { UserName = "User1", PasswordHash = passwordHasher.HashPassword("Password1") };
			userStore.CreateUser(user);

			// Сначала регистрируется модуль аутентификации
			hosting.RegisterModule(new AuthenticationOwinHostingModule());

			// Затем регистрируются все остальные модули, которые нуждаются в IPrincipal
			hosting.RegisterModule(new FakeOwinHostingModule(userStore, passwordHasher, requests));

			// WHEN

			hosting.Start();

			try
			{
				var httpClient = CreateClient();

				// Пользователь входит в систему через внутренний провайдер
				SignInInternal(httpClient, "User1", "Password1", remember);

				// Затем отправляет какие-то запросы системе
				SomePostRequest(httpClient);
				SomeGetRequest(httpClient);

				// Пользователь выходит из системы
				SignOut(httpClient);

				// Затем отправляет какие-то запросы системе
				SomePostRequest(httpClient);
				SomeGetRequest(httpClient);
			}
			finally
			{
				hosting.Stop();
			}

			// THEN

			// Проверяем, что все запросы прошли
			Assert.AreEqual(4, requests.Count);

			// Проверяем, что первые два запроса были аутентифицированы
			Assert.IsTrue(requests.Take(2).All(i => i != null && i.Identity.Name == "User1"));

			// Проверяем, что последние два запроса были неаутентифицированы
			Assert.IsTrue(requests.Skip(2).All(i => i == null));
		}

		private static HttpClient CreateClient()
		{
			return new HttpClient();
		}

		private static void SignInInternal(HttpClient client, string userName, string password, bool remember)
		{
			var signInForm = string.Format(@"{{ 'UserName': '{0}', 'Password': '{1}', 'Remember': {2} }}", userName, password, remember.ToString().ToLower());
			var signInInternalUri = new Uri(string.Format("{0}://{1}:{2}/Auth/SignInInternal", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));
			var response = client.PostAsync(signInInternalUri, new StringContent(signInForm, Encoding.UTF8, "application/json")).Result;
			Console.WriteLine(@"SignInInternal: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
		}

		private static void SignOut(HttpClient client)
		{
			var signOutUri = new Uri(string.Format("{0}://{1}:{2}/Auth/SignOut", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));
			var response = client.PostAsync(signOutUri, new StringContent("")).Result;
			Console.WriteLine(@"SignOut: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
		}

		private static void SomePostRequest(HttpClient client)
		{
			var somePostActionUri = new Uri(string.Format("{0}://{1}:{2}/Some/Post/Action", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));
			var response = client.PostAsync(somePostActionUri, new StringContent("")).Result;
			Console.WriteLine(@"Some HTTP POST: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
		}

		private static void SomeGetRequest(HttpClient client)
		{
			var someGetActionUri = new Uri(string.Format("{0}://{1}:{2}/Some/Get/Action", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));
			var response = client.GetAsync(someGetActionUri).Result;
			Console.WriteLine(@"Some HTTP GET: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
		}


		/// <summary>
		/// Фиктивный модуль хостинга на базе OWIN.
		/// Суть модуля заключается в сохранении IPrincipal всех перехваченных запросов.
		/// </summary>
		public sealed class FakeOwinHostingModule : OwinHostingModule
		{
			public FakeOwinHostingModule(IApplicationUserStore userStore, IApplicationUserPasswordHasher passwordHasher, ICollection<IPrincipal> requests)
			{
				_userStore = userStore;
				_passwordHasher = passwordHasher;
				_requests = requests;
			}


			private readonly IApplicationUserStore _userStore;
			private readonly IApplicationUserPasswordHasher _passwordHasher;
			private readonly ICollection<IPrincipal> _requests;


			public override void Configure(IAppBuilder builder, IHostingContext context)
			{
				context.Set(_userStore);
				context.Set(_passwordHasher);
				builder.Use(typeof(FakeOwinMiddleware), _requests);
			}


			/// <summary>
			/// Фиктивный обработчик HTTP-запросов на базе OWIN.
			/// Суть обработчика заключается в сохранении IPrincipal всех перехваченных запросов.
			/// </summary>
			public sealed class FakeOwinMiddleware : OwinMiddleware
			{
				public FakeOwinMiddleware(OwinMiddleware next, ICollection<IPrincipal> requests)
					: base(next)
				{
					_requests = requests;
				}


				private readonly ICollection<IPrincipal> _requests;


				public override Task Invoke(IOwinContext context)
				{
					// Сохраняем IPrincipal всех перехваченных запросов
					_requests.Add(context.Request.User);

					// Здесь цепочка обработки запроса прерывается
					context.Response.StatusCode = 200;
					return Task.FromResult(true);
				}
			}
		}


		/// <summary>
		/// Фиктивные методы хэширования пароля.
		/// </summary>
		public class FakeApplicationUserPasswordHasher : IApplicationUserPasswordHasher
		{
			public string HashPassword(string password)
			{
				return password;
			}

			public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
			{
				return (hashedPassword == providedPassword);
			}
		}
	}
}