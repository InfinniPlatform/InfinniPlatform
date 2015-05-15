using System.Web.Http.Filters;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Logging;

namespace InfinniPlatform.WebApi.Logging
{
	/// <summary>
	/// Фильтр для журналирования ошибок, происходящих на уровне REST-сервисов.
	/// </summary>
	public sealed class LogExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly ILog _log;

		public LogExceptionFilterAttribute(ILog log)
		{
			_log = log;
		}

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			_log.Error(actionExecutedContext.Request.ToString(), actionExecutedContext.Exception);
		}
	}
}