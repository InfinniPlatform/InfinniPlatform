using System;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Properties;
using InfinniPlatform.Sdk.Environment.Log;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Обработчик HTTP-запросов на базе OWIN для логирования необработанных исключений.
    /// </summary>
    /// <remarks>
    ///     Логирует необработанные исключения, возникающие на уровне OWIN.
    /// </remarks>
    internal sealed class UnhandledExceptionOwinMiddleware : OwinMiddleware
    {
        private static readonly Task EmptyTask = Task.FromResult<object>(null);
        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;

        public UnhandledExceptionOwinMiddleware(OwinMiddleware next, ILog log, IPerformanceLog performanceLog)
            : base(next)
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

        public override Task Invoke(IOwinContext context)
        {
            var start = DateTime.Now;

            try
            {
                _log.InitThreadLoggingContext(context.Environment);

                return Next.Invoke(context).ContinueWith(task =>
                {
                    _log.InitThreadLoggingContext(context.Environment);

                    if (task.IsFaulted)
                    {
                        LogException(task.Exception);
                    }

                    var action = string.Format("{0}::{1}", context.Request.Method, context.Request.Path);
                    _performanceLog.Log("OWIN", action, start, task.Exception != null ? task.Exception.Message : null);

                    return EmptyTask;
                });
            }
            catch (Exception exception)
            {
                try
                {
                    LogException(exception);

                    return EmptyTask;
                }
                catch
                {
                    // Не удалось залогировать ошибку
                }

                _performanceLog.Log("OWIN", "Invoke", start, exception.Message);

                throw;
            }
        }

        private void LogException(Exception exception)
        {
            _log.Error(Resources.UnhandledExceptionOwinMiddleware, null, exception);
        }
    }
}