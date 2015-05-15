using System;
using InfinniPlatform.Api.Index.SearchOptions;
using Nest;
using IFilter = InfinniPlatform.Api.Index.SearchOptions.IFilter;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters
{
    /// <summary>
    /// Реализация интерфейса <see cref="IFilter"/> для использования с Nest
    /// </summary>
    sealed class NestFilter : IFilter<FilterContainer>
    {
        private readonly FilterContainer _filter;

        internal NestFilter(FilterContainer filter)
        {
            _filter = filter;
        }

        /// <summary>
        /// Возвращает экземпляр <see cref="Nest.FilterContainer"/>, который может быть использован для выборки документов с помощью Nest
        /// </summary>
        public FilterContainer GetFilterObject()
        {
            return _filter;
        }

        public IFilter And(IFilter target)
        {
            CheckType(target);
            return new NestFilter(_filter && ((IFilter<FilterContainer>)target).GetFilterObject());
        }

        public IFilter Or(IFilter target)
        {
            CheckType(target);
            return new NestFilter(_filter || ((IFilter<FilterContainer>)target).GetFilterObject());
        }

        public IFilter Not()
        {
            return new NestFilter(!_filter);
        }

        private static void CheckType(IFilter target)
        {
            if (!(target is IFilter<FilterContainer>))
                throw new Exception( string.Format("Несовместимые реализации интерфейса ICriteria: \"{0}\" и \"{1}\"", typeof(IFilter<FilterContainer>), target.GetType()));
        }
    }
}
