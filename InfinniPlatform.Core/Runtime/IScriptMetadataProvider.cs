namespace InfinniPlatform.Runtime
{
    /// <summary>
    ///     Провайдер метаданных скриптов
    /// </summary>
    public interface IScriptMetadataProvider
    {
        /// <summary>
        ///     Получить метаданные скриптов
        /// </summary>
        /// <param name="scriptIdentifier">Идентификатор метаданных скрипта</param>
        /// <returns>Метаданные скрипта</returns>
        ScriptMetadata GetScriptMetadata(string scriptIdentifier);

        /// <summary>
        ///     Добавить метаданные скрипта
        /// </summary>
        /// <param name="scriptMetadata">Метаданные скрипта</param>
        void SetScriptMetadata(ScriptMetadata scriptMetadata);
    }
}