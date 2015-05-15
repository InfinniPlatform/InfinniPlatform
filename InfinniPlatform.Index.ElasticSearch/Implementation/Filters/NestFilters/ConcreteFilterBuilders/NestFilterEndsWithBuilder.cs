﻿using InfinniPlatform.Api.Index.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterEndsWithBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<dynamic>.Query(q => q.Wildcard(field, "*" + value.ToString().ToLowerInvariant())));
        }
    }
}


