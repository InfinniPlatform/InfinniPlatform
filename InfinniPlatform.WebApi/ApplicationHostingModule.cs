using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.WebApi.Middleware;
using Owin;

namespace InfinniPlatform.WebApi
{
	/// <summary>
	///   Модуль хостинга приложений на базе кастомного роутинга
	/// </summary>
	public sealed class ApplicationHostingModule : OwinHostingModule
	{
		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.Use(typeof (ApplicationHostingRoutingMiddleware));
		}
	}
}
