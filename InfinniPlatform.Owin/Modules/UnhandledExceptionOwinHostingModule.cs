using System;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;
using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    ///     Модуль хостинга на базе OWIN для логирования необработанных исключений.
    /// </summary>
    /// <remarks>
    ///     Логирует необработанные исключения, возникающие на уровне OWIN.
    /// </remarks>
    public sealed class UnhandledExceptionOwinHostingModule : OwinHostingModule
    {
        private readonly ILog _log;

        public UnhandledExceptionOwinHostingModule(ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _log = log;
        }

        public override void Configure(IAppBuilder builder, IHostingContext context)
        {
            builder.Use(typeof (UnhandledExceptionOwinMiddleware), _log);
        }
    }
}