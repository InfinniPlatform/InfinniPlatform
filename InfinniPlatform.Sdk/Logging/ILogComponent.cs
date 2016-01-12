using System;

namespace InfinniPlatform.Sdk.Logging
{
    /// <summary>
    /// Компонент для получения лога из контекста
    /// </summary>
    [Obsolete("Use InfinniPlatform.Sdk.Logging.ILog")]
    public interface ILogComponent
    {
        ILog GetLog();
    }
}