namespace InfinniPlatform.Core.SystemInfo
{
    /// <summary>
    /// Провайдер информации о системе.
    /// </summary>
    /// <remarks>
    /// Предоставляет базовую информацию о системе, например, версия, номер сборки, состояние окружения и самой системы и т.д.
    /// </remarks>
    public interface ISystemInfoProvider
    {
        /// <summary>
        /// Возвращает информацию о системе.
        /// </summary>
        object GetSystemInfo();
    }
}