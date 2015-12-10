﻿using System;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Properties;
using InfinniPlatform.Sdk.Environment.Log;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Обработчик HTTP-запросов для обработки ошибок выполнения запросов.
    /// </summary>
    internal sealed class ErrorHandlingOwinMiddleware : OwinMiddleware
    {
        private static readonly Task EmptyTask = Task.FromResult<object>(null);

        public ErrorHandlingOwinMiddleware(OwinMiddleware next, ILog log, IPerformanceLog performanceLog) : base(next)
        {
            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


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

                                                             LogPerformance($"{context.Request.Method}::{context.Request.Path}", start, task.Exception);

                                                             return EmptyTask;
                                                         });
            }
            catch (Exception exception)
            {
                LogException(exception);
                LogPerformance("Invoke", start, exception);

                return EmptyTask;
            }
        }


        private void LogException(Exception exception)
        {
            _log.Error(Resources.UnhandledExceptionOwinMiddleware, null, exception);
        }

        private void LogPerformance(string method, DateTime start, Exception exception)
        {
            _performanceLog.Log("OWIN", method, start, exception.GetMessage());
        }
    }
}