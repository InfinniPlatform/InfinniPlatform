using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions
{
    public static class NestFilterExtensions
    {
        public static void ApplyTo<T>(this IFilter filter, Nest.SearchDescriptor<T> searchDescriptor) where T : class
        {
            if (filter == null || searchDescriptor == null)
                return;
            searchDescriptor.Filter(((IFilter<Nest.FilterContainer>)filter).GetFilterObject());
        }

        public static void ApplyTo<T>(this IFilter filter, Nest.CountDescriptor<T> countDescriptor) where T : class
        {
            if (filter == null || countDescriptor == null)
                return;

            countDescriptor.Query(q => ((IFilter<Nest.QueryContainer>)filter).GetFilterObject());
        }
    }
}
