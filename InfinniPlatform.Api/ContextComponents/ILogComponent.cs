using InfinniPlatform.Api.Profiling;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент для получения лога из контекста
    /// </summary>
    public interface ILogComponent
    {
        ILog GetLog();
    }
}