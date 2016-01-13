using System;
using System.Threading;

using InfinniPlatform.ElasticSearch.Filters.NestQueries;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters
{
    /// <summary>
    /// Построитель фабрики фильтров (на случай необходимости замены Nest и абстракций над ним)
    /// </summary>
    public static class QueryBuilderFactory
    {
        private static readonly Lazy<INestFilterBuilder> Instance = new Lazy<INestFilterBuilder>(() => new NestQueryBuilder(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static INestFilterBuilder GetInstance()
        {
            return Instance.Value;
        }
    }
}
