using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;
using InfinniPlatform.Sdk.Environment.Index;

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