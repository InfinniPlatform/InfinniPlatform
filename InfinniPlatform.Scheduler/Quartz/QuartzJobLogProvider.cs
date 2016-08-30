using System;

using Quartz.Logging;

namespace InfinniPlatform.Scheduler.Quartz
{
    /// <summary>
    /// Сервис логирования Quartz.
    /// </summary>
    internal class QuartzJobLogProvider : ILogProvider
    {
        private QuartzJobLogProvider(Sdk.Logging.ILog log)
        {
            _log = log;
        }


        private readonly Sdk.Logging.ILog _log;


        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
                   {
                       var message = string.Format(func() ?? string.Empty, parameters);

                       switch (level)
                       {
                           case LogLevel.Trace:
                           case LogLevel.Debug:
                               _log.Debug(message, exception);
                               break;
                           case LogLevel.Info:
                               _log.Info(message, exception);
                               break;
                           case LogLevel.Warn:
                               _log.Warn(message, exception);
                               break;
                           case LogLevel.Error:
                               _log.Error(message, exception);
                               break;
                           case LogLevel.Fatal:
                               _log.Fatal(message, exception);
                               break;
                       }

                       return true;
                   };
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotSupportedException();
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotSupportedException();
        }
    }
}