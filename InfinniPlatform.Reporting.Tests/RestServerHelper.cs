using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace InfinniPlatform.Reporting.Tests
{
	sealed class RestServerHelper
	{
		public static T InvokeServer<T>(string baseAddress, Func<T> serverInvoke)
		{
			using (var config = new HttpSelfHostConfiguration(baseAddress))
			{
				config.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

				using (var server = new HttpSelfHostServer(config))
				{
					server.OpenAsync().Wait();

					try
					{
						return serverInvoke();
					}
					finally
					{
						server.CloseAsync().Wait();
					}
				}
			}
		}
	}
}