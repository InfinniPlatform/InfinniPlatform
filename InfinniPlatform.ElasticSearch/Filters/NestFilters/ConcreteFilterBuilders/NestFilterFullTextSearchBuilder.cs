using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
	internal sealed class NestFilterFullTextSearchBuilder : IConcreteFilterBuilder
	{
	    public IFilter Get(string field, object value)
	    {
            // При индексации токенайзер удаляет дефис и решетку 
            // http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/_finding_exact_values.html#_term_filter_with_numbers
            // Заменяем их пробелы

			var processedValue = "*" + value.ToString().Replace(" ","?").Replace("-", "?").Replace("#", "?").Trim() + "*";

			//TODO: полнотекстовый поиск будет работать корректно только в случае использования match query:
			//		        return new NestFilter(
			//                    Filter<dynamic>.Query(qs => qs.MultiMatch(s => s.OnFields(new [] {"Values.Street*"}).Query(processedValue).Analyzer("keywordbasedsearch"))));
			//для этого необходимо передавать список полей, по которым выполняется полнотекстовый поиск документов

	        if (string.IsNullOrEmpty(field))
	        {
	            return new NestFilter(
	                Nest.Filter<dynamic>.Query(q => q
	                    .QueryString(qs => qs
                            .Analyzer("fulltextquery")
                            .Query(processedValue))));
	        }

            return new NestFilter(
                    Nest.Filter<dynamic>.Query(q => q
                        .QueryString(qs => qs
                            .Analyzer("fulltextquery")
                            .OnFields(field.Split('\n'))
                            .Query(processedValue))));
	    }
	}
}
