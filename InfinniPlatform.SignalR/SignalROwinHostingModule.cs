using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;

using Microsoft.AspNet.SignalR;

using Owin;

namespace InfinniPlatform.SignalR
{
	/// <summary>
	/// Модуль хостинга ASP.NET SignalR на базе OWIN.
	/// </summary>
	public sealed class SignalROwinHostingModule : OwinHostingModule
	{
		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			var config = new HubConfiguration
							 {
								 EnableDetailedErrors = true
							 };

			builder.MapSignalR(config);
		}
	}
}