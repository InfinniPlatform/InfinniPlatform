using System;

using Microsoft.Owin;

using InfinniPlatform.SystemInfo;

namespace InfinniPlatform.Owin.Middleware
{
	/// <summary>
	/// Обработчик HTTP-запросов на базе OWIN для вывода информации о системе.
	/// </summary>
	sealed class SystemInfoOwinMiddleware : RoutingOwinMiddleware
	{
		public SystemInfoOwinMiddleware(OwinMiddleware next, ISystemInfoProvider systemInfoProvider)
			: base(next)
		{
			if (systemInfoProvider == null)
			{
				throw new ArgumentNullException("systemInfoProvider");
			}

			_systemInfoProvider = systemInfoProvider;

			RegisterGetRequestHandler(new PathString("/Info").Create(Priority.Standard), GetSystemInfo);
            RegisterGetRequestHandler(new PathString("/favicon.ico").Create(Priority.Standard), GetFavicon);
		}


		private readonly ISystemInfoProvider _systemInfoProvider;


		/// <summary>
		/// Возвращает информацию о системе.
		/// </summary>
		private IRequestHandlerResult GetSystemInfo(IOwinContext context)
		{
			try
			{
				var systemInfo = _systemInfoProvider.GetSystemInfo();

				return new ValueRequestHandlerResult(systemInfo);
			}
			catch (Exception error)
			{
				return new ErrorRequestHandlerResult(error);
			}
		}

		/// <summary>
		/// Обрабатывает запрос браузера на получение "favicon.ico".
		/// </summary>
		private static IRequestHandlerResult GetFavicon(IOwinContext context)
		{
			return new EmptyRequestHandlerResult();
		}
	}
}