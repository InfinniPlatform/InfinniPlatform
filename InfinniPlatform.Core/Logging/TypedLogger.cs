using System;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// Delegates to a new <see cref="ILogger" /> instance using the <see cref="TComponent" /> for getting the logger category name.
    /// </summary>
    /// <typeparam name="TComponent">The event source.</typeparam>
    /// <remarks>
    /// Generally the <typeparamref name="TComponent" /> is used to enable activation of a named <see cref="ILogger" /> from dependency injection.
    /// </remarks>
    public class TypedLogger<TComponent> : ILogger<TComponent>
    {
        /// <summary>
        /// Creates a new <see cref="TypedLogger{TComponent}" />.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public TypedLogger(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            var loggerCategoryName = LoggerNameHelper.GetCategoryNameGeneric(typeof(TComponent));

            _logger = loggerFactory.CreateLogger(loggerCategoryName);
        }


        private readonly ILogger _logger;


        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}