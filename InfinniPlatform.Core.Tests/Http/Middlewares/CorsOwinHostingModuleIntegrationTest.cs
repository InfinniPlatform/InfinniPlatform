using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Http.Hosting;
using InfinniPlatform.Core.Http.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http;

using Microsoft.Owin;

using NUnit.Framework;

using Owin;

namespace InfinniPlatform.Core.Tests.Http.Middlewares
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Because need process AppDomain.AssemblyResolve")]
    public class CorsOwinHostingModuleIntegrationTest
    {
        [Test]
        public void ServerReplyShouldContainsAccessControlAllowOrigin()
        {
            var hostingConfig = HostingConfig.Default;

            var hostingMiddlewares = new IHttpMiddleware[] { new CorsHttpMiddleware(), new FakeHttpMiddleware() };

            var hostingService = new OwinHostingService(hostingConfig, new HostAddressParser(), hostingMiddlewares);

            hostingService.Start();

            try
            {
                // Given

                var requestUri = new Uri($"{hostingConfig.Scheme}://{hostingConfig.Name}:{hostingConfig.Port}/some/resource");
                var request = WebRequest.Create(requestUri);
                request.Method = "GET";
                request.Headers.Add("Origin", "null");

                // When

                var response = request.GetResponse();
                var headers = response.Headers;

                // Then
                Assert.IsTrue(headers.AllKeys.Contains("Access-Control-Allow-Origin"));
                Assert.AreEqual("true", headers["Access-Control-Allow-Credentials"]);
                Assert.IsTrue(headers.AllKeys.Contains("Access-Control-Allow-Credentials"));
                Assert.AreEqual("null", headers["Access-Control-Allow-Origin"]);
            }
            finally
            {
                hostingService.Stop();
            }
        }
    }


    internal sealed class FakeHttpMiddleware : HttpMiddleware
    {
        public FakeHttpMiddleware() : base(HttpMiddlewareType.Application)
        {
        }


        public override void Configure(IAppBuilder builder)
        {
            builder.Use(typeof(FakeOwinHandler));
        }


        private sealed class FakeOwinHandler : OwinMiddleware
        {
            public FakeOwinHandler(OwinMiddleware next) : base(next)
            {
            }

            public override Task Invoke(IOwinContext context)
            {
                var buffer = Encoding.UTF8.GetBytes("FakeOwinHandler");
                context.Response.StatusCode = 200;
                return context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}