using InfinniPlatform.Sdk.Serialization;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Core.Logging
{
    public class LoggingProvider : ILoggerProvider
    {
        public void Dispose()
        {
            //empty
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = LogManagerCache.GetLog(typeof(Logger), JsonObjectSerializer.Default) as ILogger;
            return logger;
        }
    }
}