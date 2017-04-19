namespace InfinniPlatform.Core.IoC
{
    /// <summary>
    /// Модуль регистрации зависимостей.
    /// </summary>
    public interface IContainerModule
    {
        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор зависимостей и правил их разрешения.</param>
        void Load(IContainerBuilder builder);
    }
}