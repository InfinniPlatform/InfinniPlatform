using InfinniPlatform.Api.Profiling;

namespace InfinniPlatform.Api.ContextComponents
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