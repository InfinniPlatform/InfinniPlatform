using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
	internal sealed class NestQueryFullTextSearchBuilder : IConcreteFilterBuilder
	{
	    public IFilter Get(string field, object value)
	    {
	        // При индексации токенайзер удаляет дефис и решетку 
	        // http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/_finding_exact_values.html#_term_filter_with_numbers
	        // Заменяем их пробелы

	        var processedValue = "*" + value.ToString().Replace(" ", "?").Replace("-", "?").Replace("#", "?").Trim() + "*";

	        //TODO: полнотекстовый поиск будет работать корректно только в случае использования match query:
	        //		        return new NestFilter(
	        //                    Filter<dynamic>.Query(qs => qs.MultiMatch(s => s.OnFields(new [] {"Values.Street*"}).Query(processedValue).Analyzer("keywordbasedsearch"))));
	        //для этого необходимо передавать список полей, по которым выполняется полнотекстовый поиск документов

	        if (string.IsNullOrEmpty(field))
	        {
	            return new NestQuery(
	                Nest.Query<dynamic>.QueryString(qs => qs
	                    .Analyzer("fulltextquery")
	                    .Query(processedValue)));
	        }

	        return new NestQuery(
	            Nest.Query<dynamic>.QueryString(qs => qs
	                .Analyzer("fulltextquery")
	                .OnFields(field.Split('\n'))
	                .Query(processedValue)));
	    }
	}
}
