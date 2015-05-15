using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;

using NUnit.Framework;

using Owin;

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace InfinniPlatform.Cors.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CorsOwinHostingModuleIntegrationTest
	{
		private static readonly HostingConfig HostingConfig = TestSettings.DefaultHostingConfig;


		[Test]
		public void ServerReplyShouldContainsAccessControlAllowOrigin()
		{
			// Given

			var requestUri = new Uri(string.Format("{0}://{1}:{2}/some/resource", HostingConfig.ServerScheme, HostingConfig.ServerName, HostingConfig.ServerPort));

			var hosting = new OwinHostingService(config => config.Configuration(HostingConfig));

			hosting.RegisterModule(new CorsOwinHostingModule());
			hosting.RegisterModule(new FakeOwinHostingModule());

			// When

			hosting.Start();

			var request = WebRequest.Create(requestUri);
			request.Method = "GET";
			request.Headers.Add("Origin", "null");
			var response = request.GetResponse();
			var headers = response.Headers;

			hosting.Stop();

			// Then
			Assert.IsTrue(headers.AllKeys.Contains("Access-Control-Allow-Origin"));
			Assert.AreEqual("true", headers["Access-Control-Allow-Credentials"]);
			Assert.IsTrue(headers.AllKeys.Contains("Access-Control-Allow-Credentials"));
			Assert.AreEqual("null", headers["Access-Control-Allow-Origin"]);
		}
	}


	public class FakeOwinHostingModule : OwinHostingModule
	{
		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.Use(typeof(FakeOwinHandler), "Fake");
		}

		// ReSharper disable NotAccessedField.Local

		public class FakeOwinHandler
		{
			private readonly AppFunc _next;
			private readonly string _prefix;

			public FakeOwinHandler(AppFunc next, string prefix)
			{
				_next = next;
				_prefix = prefix;
			}

			public Task Invoke(IDictionary<string, object> environment)
			{
				var headers = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
				headers.Add("Content-Type", new[] { "text/plain; charset=utf-8" });

				var outstream = (Stream)environment["owin.ResponseBody"];
				var buffer = Encoding.UTF8.GetBytes("FakeOwinHandler");
				outstream.WriteAsync(buffer, 0, buffer.Length);

				return _next(environment);
			}
		}

		// ReSharper restore NotAccessedField.Local
	}
}