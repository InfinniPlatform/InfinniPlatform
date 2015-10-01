using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для получения лога из контекста
    /// </summary>
    public interface ILogComponent
    {
        ILog GetLog();
    }
}