﻿using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterNotEqualsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(!Nest.Filter<dynamic>.Term(field, value.ToString()));
        }
    }
}
