namespace InfinniPlatform.Factories
{
    /// <summary>
    ///     Конструктор фабрик прикладных скриптов
    /// </summary>
    public interface IScriptFactoryBuilder
    {
        /// <summary>
        ///     Создать фабрику прикладных скриптов для указанной версии конфигурации
        /// </summary>
        /// <returns>Фабрика скриптов</returns>
        IScriptFactory BuildScriptFactory(string metadataConfigurationId, string version);
    }
}