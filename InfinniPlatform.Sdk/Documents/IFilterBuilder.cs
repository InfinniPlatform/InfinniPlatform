namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Интерфейс для создания экземпляров <see cref="IFilter" />
    /// </summary>
    public interface IFilterBuilder
    {
        /// <summary>
        ///     Создать фильтр для одного поля с указанным значением и методом сравнения
        /// </summary>
        IFilter Get(string field, dynamic value, CriteriaType compareMethod);

        /// <summary>
        ///     Создать скрипт-фильтр
        /// </summary>
        IFilter Get(ICalculatedField script, object value);
    }
}