using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Json.IndexQueryLanguage.ResultConverters;
using InfinniPlatform.Json.IndexQueryLanguage.ResultConverters.ConverterImplementations;
using InfinniPlatform.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage
{
    public class ProjectionBuilder
    {
        private readonly JsonParser _jsonEventParser = new JsonParser();
        
        private readonly ResultConverterConfiguration _resultConverterConfig =  new ResultConverterConfiguration();

        public ProjectionBuilder()
        {
            _resultConverterConfig = new ResultConverterConfiguration();
            _resultConverterConfig.BuilderFor(tokens => !tokens.Any(), () => new ResultConverterEmpty());
            _resultConverterConfig.BuilderFor(tokens => tokens.Count() > 1, () => new ResultConverterArray());
            _resultConverterConfig.DefaultBuilder(() => new ResultConverterDefault());
        }
        
        public JObject GetProjection(JObject jsonObject, IEnumerable<ProjectionObject> projectionItems,IEnumerable<Criteria> where )
        {
            var resultProjection = new JObject();

			jsonObject.FilterItems(where);
			
            foreach (var projectionItem in projectionItems)
            {

                var result = _jsonEventParser.FindJsonPath(jsonObject, projectionItem.ProjectionPath);
	           


                var converter = _resultConverterConfig.Build(result);
                
				if (converter == null)
                {
                    throw new ArgumentException("no registered result converters found");
                }

                converter.BuildResultJToken(projectionItem.ProjectionName,resultProjection,result);
            }
            return projectionItems.Any() ? resultProjection : jsonObject;
        } 
    }

    public static class ProjectionBuilderExtensions
    {
        public static void FilterItems(this IEnumerable<JToken> jtokens, IEnumerable<Criteria> criteria)
        {
	        var queryInterpreter = new QueryCriteriaInterpreter();
            foreach (var filteredToken in jtokens)
            {
	            var applicableToken = filteredToken;

                foreach (var criterion in criteria)
                {
	                var filterCriteria = new JsonFilterCriteria(criterion, applicableToken);

	                var clientFilterOperator = queryInterpreter.BuildOperator(criterion);
					
					filterCriteria.RemoveUnsatisfiedTokens(clientFilterOperator);

	                applicableToken = filterCriteria.JsonToken;
                }
            }
        }

		public static void FilterItems(this JToken item, IEnumerable<Criteria> criteria)
		{
			new[] {item}.FilterItems(criteria);
		}

    }

}