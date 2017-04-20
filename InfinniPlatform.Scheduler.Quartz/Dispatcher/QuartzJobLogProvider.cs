using System;

using Quartz.Logging;

using ILog = InfinniPlatform.Logging.ILog;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    /// <summary>
    /// Сервис логирования Quartz.
    /// </summary>
    internal class QuartzJobLogProvider : ILogProvider
    {
        public QuartzJobLogProvider(ILog log)
        {
            _log = log;
        }


        private readonly ILog _log;


        public Logger GetLogger(string name)
        {
            return (logLevel, messageFunc, exception, formatParameters) =>
                   {
                       if (messageFunc != null)
                       {
                           var message = string.Format(messageFunc() ?? string.Empty, formatParameters);

                           switch (logLevel)
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