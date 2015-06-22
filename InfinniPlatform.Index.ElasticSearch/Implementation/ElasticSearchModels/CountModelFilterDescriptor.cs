using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
	public class CountModelFilterDescriptor<T> where T:class
	{
        private readonly Nest.CountDescriptor<T> _countDescriptor;

        public CountModelFilterDescriptor(Nest.CountDescriptor<T> countDescriptor)
		{
			_countDescriptor = countDescriptor;
		}

		public void ApplyModel(IFilter criteria)
		{
            criteria.ApplyTo(_countDescriptor);
		}
	}
}