using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Profiling;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для логирования из глобального контекста
    /// </summary>
    public sealed class LogComponent : ILogComponent
    {
        private readonly ILog _logger;

        public LogComponent(ILog logger)
        {
            _logger = logger;
        }

        public ILog GetLog()
        {
            return _logger;
        }
    }
}