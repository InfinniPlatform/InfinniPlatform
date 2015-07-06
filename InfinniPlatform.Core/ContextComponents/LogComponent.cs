using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;

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