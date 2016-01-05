using System;
using System.Threading;

using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
    /// <summary>
    /// Построитель фабрики фильтров (на случай необходимости замены Nest и абстракций над ним)
    /// </summary>
    public static class QueryBuilderFactory
    {
        private static readonly Lazy<IFilterBuilder> Instance = new Lazy<IFilterBuilder>(() => new NestQueryBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static IFilterBuilder GetInstance()
        {
            return Instance.Value;
        }
    }
}
