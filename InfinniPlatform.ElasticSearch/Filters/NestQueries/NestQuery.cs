using System;

using InfinniPlatform.Sdk.Documents;

using Nest;

using IFilter = InfinniPlatform.Sdk.Documents.IFilter;


namespace InfinniPlatform.ElasticSearch.Filters.NestQueries
{
    /// <summary>
    /// Реализация интерфейса <see cref="Nest.IQuery"/> для использования с Nest
    /// </summary>
    sealed class NestQuery : IFilter<QueryContainer>
    {
        private readonly QueryContainer _query;

        internal NestQuery(QueryContainer query)
        {
            _query = query;
        }

        /// <summary>
        /// Возвращает экземпляр <see cref="Nest.FilterContainer"/>, который может быть использован для выборки документов с помощью Nest
        /// </summary>
        public QueryContainer GetFilterObject()
        {
            return _query;
        }

        public IFilter And(IFilter target)
        {
            CheckType(target);
            return new NestQuery(_query && ((IFilter<QueryContainer>)target).GetFilterObject());
        }

        public IFilter Or(IFilter target)
        {
            CheckType(target);
            return new NestQuery(_query || ((IFilter<QueryContainer>)target).GetFilterObject());
        }

        public IFilter Not()
        {
            return new NestQuery(!_query);
        }

        private static void CheckType(IFilter target)
        {
            if (!(target is IFilter<QueryContainer>))
                throw new Exception( string.Format("Несовместимые реализации интерфейса ICriteria: \"{0}\" и \"{1}\"", typeof(IFilter<QueryContainer>), target.GetType()));
        }
    }
}
