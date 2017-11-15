using System;

using Microsoft.Extensions.Logging;

using Quartz.Logging;

using LogLevel = Quartz.Logging.LogLevel;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    /// <summary>
    /// Сервис логирования Quartz.
    /// </summary>
    internal class QuartzJobLogProvider : ILogProvider
    {
        public QuartzJobLogProvider(ILogger<QuartzJobLogProvider> logger)
        {
            _logger = logger;
        }


        private readonly ILogger _logger;


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
                                   _logger.LogDebug(message, exception);
                                   break;
                               case LogLevel.Info:
                                   _logger.LogTrace(message, exception);
                                   break;
                               case LogLevel.Warn:
                                   _logger.LogWarning(message, exception);
                                   break;
                               case LogLevel.Error:
                                   _logger.LogError(message, exception);
                                   break;
                               case LogLevel.Fatal:
                                   _logger.LogCritical(message, exception);
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