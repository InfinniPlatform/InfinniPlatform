using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// A generic interface for performance logging.
    /// </summary>
    /// <typeparam name="TComponent">The event source.</typeparam>
    /// <remarks>
    /// Generally the <typeparamref name="TComponent" /> is used to enable activation of a named <see cref="ILogger" /> from dependency injection.
    /// </remarks>
    [LoggerName(nameof(IPerformanceLogger<TComponent>))]
    public interface IPerformanceLogger<TComponent> : IPerformanceLogger
    {
    }
}