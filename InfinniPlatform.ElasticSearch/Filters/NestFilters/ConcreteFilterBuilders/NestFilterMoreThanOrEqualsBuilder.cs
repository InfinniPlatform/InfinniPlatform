﻿using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterMoreThanOrEqualsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<dynamic>.Range(r => r.OnField(field).GreaterOrEquals(value.ToString())));
        }
    }
}
