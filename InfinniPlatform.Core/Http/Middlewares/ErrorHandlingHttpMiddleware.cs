using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    ///     Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal class ErrorHandlingHttpMiddleware : HttpMiddlewareBase<ErrorHandlingMiddlewareOptions>
    {
        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;

        public ErrorHandlingHttpMiddleware(ILog log, IPerformanceLog performanceLog) : base(HttpMiddlewareType.ErrorHandling)
        {
            _log = log;
            _performanceLog = performanceLog;
        }


        public override void Configure(IApplicationBuilder app, ErrorHandlingMiddlewareOptions options)
        {
            app.UseMiddleware<ErrorHandlingOwinMiddleware>(_log, _performanceLog);
        }
    }
}