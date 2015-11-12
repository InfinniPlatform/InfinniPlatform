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
    }
}