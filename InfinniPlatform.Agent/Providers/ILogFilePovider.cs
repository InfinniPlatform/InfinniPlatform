using System;
using System.IO;

namespace InfinniPlatform.Agent.Providers
{
    /// <summary>
    /// Предоставляет доступ к файлам логов.
    /// </summary>
    public interface ILogFilePovider
    {
        /// <summary>
        /// Возвращает содержимое файла лога событий приложения.
        /// </summary>
        /// <param name="appFullName">Полное наименование приложения (содержит имя, версию, имя экземпляра).</param>
        Func<Stream> GetAppLog(string appFullName);

        /// <summary>
        /// Возвращает содержимое файла лога производительности приложения.
        /// </summary>
        /// <param name="appFullName">Полное наименование приложения (содержит имя, версию, имя экземпляра).</param>
        Func<Stream> GetPerformanceLog(string appFullName);

        /// <summary>
        /// Возвращает содержимое лога приложения Infinni.Node.
        /// </summary>
        Func<Stream> GetNodeLog();
    }
}