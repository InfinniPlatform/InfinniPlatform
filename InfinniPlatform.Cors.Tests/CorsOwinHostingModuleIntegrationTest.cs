using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Cors.Modules;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;

using Microsoft.Owin;

using Moq;

using NUnit.Framework;

using Owin;

namespace InfinniPlatform.Cors.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class CorsOwinHostingModuleIntegrationTest
    {
        private static IOwinHostingContext CreateTestOwinHostingContext(params IOwinHostingModule[] owinHostingModules)
        {
            var containerResolverMoq = new Mock<IContainerResolver>();
            containerResolverMoq.Setup(i => i.Resolve<IEnumerable<IOwinHostingModule>>()).Returns(owinHostingModules);

            var hostingContextMoq = new Mock<IOwinHostingContext>();
            hostingContextMoq.SetupGet(i => i.Configuration).Returns(new HostingConfig { Port = 9901 });
            hostingContextMoq.SetupGet(i => i.ContainerResolver).Returns(containerResolverMoq.Object);

            return hostingContextMoq.Object;
        }


        [Test]
        public void ServerReplyShouldContainsAccessControlAllowOrigin()
        {
            var owinHostingContext = CreateTestOwinHostingContext(new CorsOwinHostingModule(), new FakeOwinHostingModule());
            var owinHostingService = new OwinHostingService(owinHostingContext);

            owinHostingService.Start();

            try
            {
                // Given

                var requestUri = new Uri($"{owinHostingContext.Configuration.Scheme}://{owinHostingContext.Configuration.Name}:{owinHostingContext.Configuration.Port}/some/resource");
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
                owinHostingService.Stop();
            }
        }
    }


    internal sealed class FakeOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
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