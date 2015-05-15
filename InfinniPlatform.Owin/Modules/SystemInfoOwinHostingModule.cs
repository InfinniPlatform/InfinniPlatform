using System;

using Owin;

using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.SystemInfo;

namespace InfinniPlatform.Owin.Modules
{
	/// <summary>
	/// Модуль хостинга на базе OWIN для вывода информации о системе.
	/// </summary>
	public sealed class SystemInfoOwinHostingModule : OwinHostingModule
	{
		public SystemInfoOwinHostingModule(ISystemInfoProvider systemInfoProvider)
		{
			if (systemInfoProvider == null)
			{
				throw new ArgumentNullException("systemInfoProvider");
			}

			_systemInfoProvider = systemInfoProvider;
		}


		private readonly ISystemInfoProvider _systemInfoProvider;


		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.Use(typeof(SystemInfoOwinMiddleware), _systemInfoProvider);
		}
	}
}