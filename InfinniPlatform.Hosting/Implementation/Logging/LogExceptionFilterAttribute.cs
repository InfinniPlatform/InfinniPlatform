using System.Web.Http.Filters;
using InfinniPlatform.Logging;

namespace InfinniPlatform.Hosting.WebApi.Implementation.Logging
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