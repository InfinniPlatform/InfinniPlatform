using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal class ErrorHandlingHttpMiddleware : HttpMiddlewareBase<ErrorHandlingMiddlewareOptions>
    {
        public ErrorHandlingHttpMiddleware(ILog log, IPerformanceLog performanceLog) : base(HttpMiddlewareType.ErrorHandling)
        {
            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        public override void Configure(IApplicationBuilder app, ErrorHandlingMiddlewareOptions options)
        {
            app.UseMiddleware<ErrorHandlingOwinMiddleware>(_log, _performanceLog);
        }
    }
}