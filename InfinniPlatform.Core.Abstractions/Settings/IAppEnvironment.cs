namespace InfinniPlatform.Core.Abstractions.Settings
{
    /// <summary>
    /// Общие настройки приложения.
    /// </summary>
    public interface IAppEnvironment
    {
        /// <summary>
        /// Уникальное имя приложения.
        /// </summary>
        /// <remarks>
        /// Используется для изоляции данных между приложениями.
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Идентификатор текущего экземпляра приложения.
        /// </summary>
        string InstanceId { get; }

        /// <summary>
        /// Идентификатор принадлежности экземпляра к кластеру.
        /// </summary>
        bool IsInCluster { get; }
    }
}