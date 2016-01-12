using System;
using System.Threading;

using InfinniPlatform.ElasticSearch.Filters.NestFilters;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters
{
    /// <summary>
    /// Построитель фабрики фильтров (на случай необходимости замены Nest и абстракций над ним)
    /// </summary>
    public static class FilterBuilderFactory
    {
        private static readonly Lazy<IFilterBuilder> Instance = new Lazy<IFilterBuilder>(() => new NestFilterBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static IFilterBuilder GetInstance()
        {
            return Instance.Value;
        }
    }
}
