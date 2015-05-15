using System;
using System.Collections.Generic;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders;
using InfinniPlatform.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
	public class FilterBuilderConfig
	{
		private readonly Dictionary<CriteriaType, Func<IFilterBuilder>> _builders = new Dictionary<CriteriaType, Func<IFilterBuilder>>();
		private Func<IFilterBuilder> _defaultBuilder;

		public FilterBuilderConfig BuilderFor(CriteriaType condition, Func<IFilterBuilder> filterBuilder)
		{
			_builders.Add(condition, filterBuilder);
			return this;
		}

		public FilterBuilderConfig DefaultBuilder(Func<IFilterBuilder> filterBuilder)
		{
			_defaultBuilder = filterBuilder;
			return this;
		}


		public IFilterBuilder Build(CriteriaType criteriaType)
		{
			Func<IFilterBuilder> filterBuilder;
			_builders.TryGetValue(criteriaType, out filterBuilder);
			return filterBuilder != null ? filterBuilder() : _defaultBuilder();
		}
	}

	public static class FilterBuilderConfigExtensions
	{
		public static FilterBuilderConfig BuildStandardConfig(this FilterBuilderConfig filterBuilderConfig)
		{
			filterBuilderConfig.BuilderFor(CriteriaType.IsEquals,           () => new FilterBuilderEquals());
            filterBuilderConfig.BuilderFor(CriteriaType.IsNotEquals,        () => new FilterBuilderNotEquals());
            filterBuilderConfig.BuilderFor(CriteriaType.IsMoreThan,         () => new FilterBuilderMoreThan());
            filterBuilderConfig.BuilderFor(CriteriaType.IsLessThan,         () => new FilterBuilderLessThan());
            filterBuilderConfig.BuilderFor(CriteriaType.IsMoreThanOrEquals, () => new FilterBuilderMoreThanOrEquals());
            filterBuilderConfig.BuilderFor(CriteriaType.IsLessThanOrEquals, () => new FilterBuilderLessThanOrEquals());
            filterBuilderConfig.BuilderFor(CriteriaType.IsContains,         () => new FilterBuilderContains());
            filterBuilderConfig.BuilderFor(CriteriaType.IsNotContains,      () => new FilterBuilderNotContains());
            filterBuilderConfig.BuilderFor(CriteriaType.IsEmpty,            () => new FilterBuilderEmpty());
            filterBuilderConfig.BuilderFor(CriteriaType.IsNotEmpty,         () => new FilterBuilderNotEmpty());
            filterBuilderConfig.BuilderFor(CriteriaType.IsStartsWith,       () => new FilterBuilderStartsWith());
            filterBuilderConfig.BuilderFor(CriteriaType.IsNotStartsWith,    () => new FilterBuilderNotStartsWith());
            filterBuilderConfig.BuilderFor(CriteriaType.IsEndsWith,         () => new FilterBuilderEndsWith());
            filterBuilderConfig.BuilderFor(CriteriaType.IsNotEndsWith,      () => new FilterBuilderNotEndsWith());
		    filterBuilderConfig.BuilderFor(CriteriaType.Script,             () => new FilterBuilderScript());
			return filterBuilderConfig;
		}
	}


}