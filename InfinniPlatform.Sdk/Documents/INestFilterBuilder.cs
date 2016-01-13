namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Интерфейс для создания экземпляров <see cref="IFilter" />
    /// </summary>
    public interface INestFilterBuilder
    {
        /// <summary>
        /// Создать фильтр для одного поля с указанным значением и методом сравнения
        /// </summary>
        IFilter Get(string field, dynamic value, CriteriaType compareMethod);

        /// <summary>
        /// Создать скрипт-фильтр
        /// </summary>
        IFilter Get(ICalculatedField script, object value);
    }
}