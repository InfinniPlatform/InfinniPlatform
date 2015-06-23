namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    ///     Глобальный контекст выполнения скрипта
    /// </summary>
    public interface IGlobalContext
    {
        /// <summary>
        ///     Получить компонент, реализующий тип Т из глоабльного контекста
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <typeparam name="T">Тип ожидаемого контракта</typeparam>
        /// <returns>Экземпляр контракта</returns>
        T GetComponent<T>(string version) where T : class;
    }
}