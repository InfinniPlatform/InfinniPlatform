using System.Collections.Generic;
using System.Web.Http.Filters;

using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.WebApi.Logging
{
    /// <summary>
    /// Фильтр для журналирования ошибок, происходящих на уровне REST-сервисов.
    /// </summary>
    public sealed class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public LogExceptionFilterAttribute(ILog log)
        {
            _log = log;
        }

        private readonly ILog _log;

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _log.Error("Error executing REST.", new Dictionary<string, object>
                                                {
                                                    { "request", actionExecutedContext.Request.ToString() }
                                                }, actionExecutedContext.Exception);
        }
    }
}