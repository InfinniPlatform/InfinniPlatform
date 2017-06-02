using System;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// A generic interface for performance logging.
    /// </summary>
    public interface IPerformanceLogger
    {
        /// <summary>
        /// Logs the method duration.
        /// </summary>
        /// <param name="method">The method name to log.</param>
        /// <param name="duration">The method duration to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Log(string method, TimeSpan duration, Exception exception = null);

        /// <summary>
        /// Logs the method duration.
        /// </summary>
        /// <param name="method">The method name to log.</param>
        /// <param name="start">The method start time.</param>
        /// <param name="exception">The exception to log.</param>
        void Log(string method, DateTime start, Exception exception = null);
    }
}