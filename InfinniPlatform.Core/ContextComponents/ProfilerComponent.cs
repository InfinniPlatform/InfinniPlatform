using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.Profiling.Implementation;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для получения профайлера операций
    /// </summary>
    public sealed class ProfilerComponent : IProfilerComponent
    {
        private readonly ILog _logger;

        public ProfilerComponent(ILog logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Получить профайлер операций
        /// </summary>
        /// <returns>Профайлер операций</returns>
        public IOperationProfiler GetOperationProfiler(string method, string arguments)
        {
            if (HostingConfig.Default.ServerProfileQuery)
            {
                return new ActionUnitProfiler(_logger, method, arguments);
            }
            return new NoQueryProfiler();
        }
    }
}