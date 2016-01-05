namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Сервис инициализации для обработки события старта приложения.
    /// </summary>
    /// <remarks>
    /// TODO: Нужно избавиться от этого интерфейса, так как для этих целей предназначен IApplicationEventHandler.
    /// </remarks>
    public interface IStartupInitializer
    {
        /// <summary>
        /// Возвращает порядковый номер инициализатора.
        /// </summary>
        /// <remarks>
        /// TODO: Порядок должен быть не важен.
        /// </remarks>
        int Order { get; }

        /// <summary>
        /// Обрабатывает событие старта приложения.
        /// </summary>
        void OnStart();
    }
}