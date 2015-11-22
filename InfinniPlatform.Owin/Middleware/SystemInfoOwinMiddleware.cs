using System;

using InfinniPlatform.SystemInfo;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Обработчик HTTP-запросов на базе OWIN для вывода информации о системе.
    /// </summary>
    internal sealed class SystemInfoOwinMiddleware : RoutingOwinMiddleware
    {
        public SystemInfoOwinMiddleware(OwinMiddleware next, ISystemInfoProvider systemInfoProvider)
            : base(next)
        {
            if (systemInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(systemInfoProvider));
            }

            _systemInfoProvider = systemInfoProvider;

            RegisterHandler(new RegistrationHandlerBase("GET", new PathString("/Info"), GetSystemInfo));
            RegisterHandler(new RegistrationHandlerBase("GET", new PathString("/favicon.ico"), GetFavicon));
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