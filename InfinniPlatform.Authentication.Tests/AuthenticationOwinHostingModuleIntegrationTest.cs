using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.RestApi;
using InfinniPlatform.Sdk.Settings;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;

using Moq;

using NUnit.Framework;

using Owin;

namespace InfinniPlatform.Authentication.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class AuthenticationOwinHostingModuleIntegrationTest
    {
        private static IOwinHostingContext CreateTestOwinHostingContext(Action<UserManager<IdentityApplicationUser>> userManagerAction, params IOwinHostingModule[] owinHostingModules)
        {
            var userStore = new IdentityApplicationUserStore(new MemoryApplicationUserStore());
            var userManager = new UserManager<IdentityApplicationUser>(userStore);
            userManagerAction(userManager);

            var containerResolverMoq = new Mock<IContainerResolver>();
            containerResolverMoq.Setup(i => i.Resolve<UserManager<IdentityApplicationUser>>()).Returns(userManager);
            containerResolverMoq.Setup(i => i.Resolve<IEnumerable<IOwinHostingModule>>()).Returns(owinHostingModules);

            var hostingContextMoq = new Mock<IOwinHostingContext>();
            hostingContextMoq.SetupGet(i => i.Configuration).Returns(HostingConfig.Default);
            hostingContextMoq.SetupGet(i => i.ContainerResolver).Returns(containerResolverMoq.Object);
            hostingContextMoq.SetupGet(i => i.OwinMiddlewareResolver).Returns(new FakeOwinMiddlewareResolver());

            return hostingContextMoq.Object;
        }


        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSignInInternal(bool remember)
        {
            var appConfigMock = new Mock<IAppConfiguration>();
            appConfigMock.Setup(m => m.GetSection<CookieAuthOwinHostingModuleSettings>(CookieAuthOwinHostingModuleSettings.SectionName)).Returns(new CookieAuthOwinHostingModuleSettings());

            var requests = new List<IPrincipal>();
            var userInfo = new IdentityApplicationUser { UserName = "User1", Claims = new[] { CreateClaim("Claim1", "Value1") } };
            var owinHostingModules = new IOwinHostingModule[] { new CookieAuthOwinHostingModule(appConfigMock.Object), new InternalAuthOwinHostingModule(), new FakeOwinHostingModule(requests) };
            var owinHostingContext = CreateTestOwinHostingContext(m => m.Create(userInfo, "Password1"), owinHostingModules);
            var owinHostingService = new OwinHostingService(owinHostingContext);

            // WHEN

            owinHostingService.Start();

            try
            {
                // When

                var httpClient = CreateClient(owinHostingContext.Configuration);

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

                // Then

                // Проверяем, что все запросы прошли
                Assert.AreEqual(4, requests.Count);

                // Проверяем, что первые два запроса были аутентифицированы
                Assert.IsTrue(requests.Take(2).All(i => i != null && i.Identity.Name == "User1"));

                // Проверяем, что последние два запроса были неаутентифицированы
                Assert.IsTrue(requests.Skip(2).All(i => i == null));
            }
            finally
            {
                owinHostingService.Stop();
            }
        }


        private static HttpClient CreateClient(HostingConfig hostingConfig)
        {
            return new HttpClient { BaseAddress = new Uri($"{hostingConfig.Scheme}://{hostingConfig.Name}:{hostingConfig.Port}") };
        }

        private static void SignInInternal(HttpClient client, string userName, string password, bool remember)
        {
            var signInForm = $@"{{ 'UserName': '{userName}', 'Password': '{password}', 'Remember': {remember.ToString().ToLower()} }}";
            var signInInternalUri = new Uri("Auth/SignInInternal", UriKind.Relative);
            var response = client.PostAsync(signInInternalUri, new StringContent(signInForm, Encoding.UTF8, "application/json")).Result;
            Console.WriteLine(@"SignInInternal: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static void SignOut(HttpClient client)
        {
            var signOutUri = new Uri("Auth/SignOut", UriKind.Relative);
            var response = client.PostAsync(signOutUri, new StringContent("")).Result;
            Console.WriteLine(@"SignOut: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static void SomePostRequest(HttpClient client)
        {
            var somePostActionUri = new Uri("Some/Post/Action", UriKind.Relative);
            var response = client.PostAsync(somePostActionUri, new StringContent("")).Result;
            Console.WriteLine(@"Some HTTP POST: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static void SomeGetRequest(HttpClient client)
        {
            var someGetActionUri = new Uri("Some/Get/Action", UriKind.Relative);
            var response = client.GetAsync(someGetActionUri).Result;
            Console.WriteLine(@"Some HTTP GET: {0}, {1}", response.StatusCode, response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        private static ApplicationUserClaim CreateClaim(string type, string value)
        {
            return new ApplicationUserClaim
            {
                Type = new ForeignKey { Id = type, DisplayName = type },
                Value = value
            };
        }


        /// <summary>
        /// Фиктивный модуль хостинга на базе OWIN.
        /// Суть модуля заключается в сохранении IPrincipal всех перехваченных запросов.
        /// </summary>
        private sealed class FakeOwinHostingModule : IOwinHostingModule
        {
            public FakeOwinHostingModule(ICollection<IPrincipal> requests)
            {
                _requests = requests;
            }


            private readonly ICollection<IPrincipal> _requests;


            public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


            public void Configure(IAppBuilder builder, IOwinHostingContext context)
            {
                builder.Use(typeof(FakeOwinMiddleware), _requests);
            }


            private sealed class FakeOwinMiddleware : OwinMiddleware
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


        private sealed class FakeOwinMiddlewareResolver : IOwinMiddlewareResolver
        {
            public Type ResolveType<TOwinMiddleware>() where TOwinMiddleware : OwinMiddleware
            {
                return typeof(TOwinMiddleware);
            }
        }
    }
}