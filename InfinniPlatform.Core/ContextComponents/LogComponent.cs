using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    /// Компонент для логирования из глобального контекста
    /// </summary>
    public sealed class LogComponent : ILogComponent
    {
        public LogComponent(ILog logger)
        {
            _logger = logger;
        }

        private readonly ILog _logger;

        public ILog GetLog()
        {
            return _logger;
        }
    }
}