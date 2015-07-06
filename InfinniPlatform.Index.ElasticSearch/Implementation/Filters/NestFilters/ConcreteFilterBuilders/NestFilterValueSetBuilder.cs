﻿using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterValueSetBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            var valueSet = value.ToString().Split('\n');

            return new NestFilter(Nest.Filter<dynamic>.Terms(field, valueSet));
        }
    }
}
