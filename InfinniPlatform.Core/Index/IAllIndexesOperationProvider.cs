namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Реализация провайдера для выполнения операций без уточнения индекса и типа
    ///     Не может применяться для получения записей метаданных с системным роутингом
    /// </summary>
    public interface IAllIndexesOperationProvider
    {
        /// <summary>
        ///     Получить объект по идентификатору
        /// </summary>
        /// <param name="key">Идентификатор индексируемого объекта</param>
        /// <returns>Индексируемый объект</returns>
        dynamic GetItem(string key);
    }
}