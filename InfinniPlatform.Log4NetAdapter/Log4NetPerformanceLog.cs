using System;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Log4NetAdapter
{
    /// <summary>
    /// Сервис <see cref="IPerformanceLog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetPerformanceLog : IPerformanceLog
    {
        public Log4NetPerformanceLog(log4net.ILog internalLog, IJsonObjectSerializer serializer)
        {
            _internalLog = internalLog;
            _serializer = serializer;
        }


        private readonly log4net.ILog _internalLog;
        private readonly IJsonObjectSerializer _serializer;


        public void Log(string method, TimeSpan duration, Exception exception = null)
        {
            _internalLog.Info(new PerformanceLogEvent(method, (long)duration.TotalMilliseconds, exception, _serializer));
        }

        public void Log(string method, DateTime start, Exception exception = null)
        {
            Log(method, DateTime.Now - start, exception);
        }
    }
}