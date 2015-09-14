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
        /// <typeparam name="T">Тип ожидаемого контракта</typeparam>
        /// <returns>Экземпляр контракта</returns>
        T GetComponent<T>() where T : class;

        /// <summary>
        ///  Получить идентификатор актуальной версии указанной конфигурации 
        /// </summary>
        /// <param name="configuration">Идентификатор конфигурации</param>
        /// <param name="userName">Логин пользователя для получения контекста</param>
        /// <returns>Идентификатор версии</returns>
        string GetVersion(string configuration, string userName);
    }
}