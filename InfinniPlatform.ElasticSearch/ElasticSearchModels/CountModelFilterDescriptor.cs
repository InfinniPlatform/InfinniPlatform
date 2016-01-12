using InfinniPlatform.ElasticSearch.Filters.Extensions;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.ElasticSearchModels
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