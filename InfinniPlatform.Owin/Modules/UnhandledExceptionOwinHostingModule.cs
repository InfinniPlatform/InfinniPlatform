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
        private readonly IPerformanceLog _performanceLog;

        public UnhandledExceptionOwinHostingModule(ILog log, IPerformanceLog performanceLog)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            if (performanceLog == null)
            {
                throw new ArgumentNullException("performanceLog");
            }

            _log = log;
            _performanceLog = performanceLog;
        }

        public override void Configure(IAppBuilder builder, IHostingContext context)
        {
            builder.Use(typeof (UnhandledExceptionOwinMiddleware), _log, _performanceLog);
        }
    }
}