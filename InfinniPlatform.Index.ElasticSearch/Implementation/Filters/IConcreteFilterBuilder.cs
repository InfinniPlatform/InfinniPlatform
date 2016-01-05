using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
    /// <summary>
    /// Фабрика для создания фильтров, реализующих конкретную модель сравнения
    /// </summary>
    public interface IConcreteFilterBuilder
    {
        IFilter Get(string field, object value);
    }
}
