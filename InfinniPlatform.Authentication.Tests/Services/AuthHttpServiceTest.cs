using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;

using NUnit.Framework;

namespace InfinniPlatform.Authentication.Tests.Services
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class AuthHttpServiceTest
    {
        private IDisposable _server;


        [TestFixtureSetUp]
        public void SetUp()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _server.Dispose();
        }


        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSignInInternal(bool remember)
        {
            // Given

            var httpClient = CreateHttpClient();
            var userNames = new List<string>();

            var userName = $"User_{Guid.NewGuid().ToString("N")}";
            var password = userName;

            // When

            CreateUserRequest(httpClient, userName, password);

            // Пользователь входит в систему через внутренний провайдер
            SignInInternal(httpClient, userName, password, remember);

            // Затем отправляет какие-то запросы системе
            userNames.Add(SomePostRequest(httpClient));
            userNames.Add(SomeGetRequest(httpClient));

            // Пользователь выходит из системы
            SignOut(httpClient);

            // Затем отправляет какие-то запросы системе
            userNames.Add(SomePostRequest(httpClient));
            userNames.Add(SomeGetRequest(httpClient));

            // Then

            // Проверяем, что все запросы прошли
            Assert.AreEqual(4, userNames.Count);

            // Проверяем, что первые два запроса были аутентифицированы
            Assert.IsTrue(userNames.Take(2).All(i => i != null && i == userName));

            // Проверяем, что последние два запроса были неаутентифицированы
            Assert.IsTrue(userNames.Skip(2).All(string.IsNullOrEmpty));
        }


        private static HttpClient CreateHttpClient()
        {
            var hostingConfig = HostingConfig.Default;

            return new HttpClient { BaseAddress = new Uri($"{hostingConfig.Scheme}://{hostingConfig.Name}:{hostingConfig.Port}") };
        }

        private static void SignInInternal(HttpClient client, string userName, string password, bool remember)
        {
            var requestForm = $@"{{ 'UserName': '{userName}', 'Password': '{password}', 'Remember': {remember.ToString().ToLower()} }}";
            var requestUri = new Uri("Auth/SignInInternal", UriKind.Relative);
            var response = client.PostAsync(requestUri, new StringContent(requestForm, Encoding.UTF8, "application/json")).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(@"POST {0} -> {1}, {2}", requestUri, response.StatusCode, result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static void SignOut(HttpClient client)
        {
            var requestUri = new Uri("Auth/SignOut", UriKind.Relative);
            var response = client.PostAsync(requestUri, new StringContent("")).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(@"POST {0} -> {1}, {2}", requestUri, response.StatusCode, result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private static string SomeGetRequest(HttpClient client)
        {
            var requestUri = new Uri("Fake/SomeGet", UriKind.Relative);
            var response = client.GetAsync(requestUri).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(@"GET {0} -> {1}, {2}", requestUri, response.StatusCode, result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return result;
        }

        private static string SomePostRequest(HttpClient client)
        {
            var requestUri = new Uri("Fake/SomePost", UriKind.Relative);
            var response = client.PostAsync(requestUri, new StringContent("")).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(@"POST {0} -> {1}, {2}", requestUri, response.StatusCode, result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return result;
        }

        private static void CreateUserRequest(HttpClient client, string userName, string password)
        {
            var requestForm = $@"{{ 'UserName': '{userName}', 'Password': '{password}' }}";
            var requestUri = new Uri("Fake/CreateUser", UriKind.Relative);
            var response = client.PostAsync(requestUri, new StringContent(requestForm, Encoding.UTF8, "application/json")).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(@"POST {0} -> {1}, {2}", requestUri, response.StatusCode, result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Thread.Sleep(1000); // Because ElasticSearch is Async :(
        }
    }
}