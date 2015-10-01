using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для получения профайлера операций из контекста
    /// </summary>
    public interface IProfilerComponent
    {
        /// <summary>
        ///     Получить профайлер операций
        /// </summary>
        /// <returns>Профайлер операций</returns>
        IOperationProfiler GetOperationProfiler(string method, string arguments);
    }
}