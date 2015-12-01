namespace InfinniPlatform.Sdk.Environment.Settings
{
    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public interface IAppConfiguration
    {
        /// <summary>
        /// Возвращает динамический объект с описанием секции конфигурации.
        /// </summary>
        /// <param name="sectionName">Имя секции конфигурации.</param>
        dynamic GetSection(string sectionName);

        /// <summary>
        /// Возвращает типизированный объект с описанием секции конфигурации.
        /// </summary>
        /// <typeparam name="TSection">Тип секции конфигурации.</typeparam>
        /// <param name="sectionName">Имя секции конфигурации.</param>
        TSection GetSection<TSection>(string sectionName) where TSection : new();
    }
}