using System;
using System.Threading;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
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
