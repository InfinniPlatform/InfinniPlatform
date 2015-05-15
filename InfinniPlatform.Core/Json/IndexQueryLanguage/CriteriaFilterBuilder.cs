using System.Linq;
using InfinniPlatform.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage
{
	public class CriteriaFilterBuilder
	{
		private readonly JsonParser _jsonParser;

		public CriteriaFilterBuilder()
		{
			_jsonParser = new JsonParser();
		}

		private JToken GetPropertyValue(JToken[] tokens)
		{
			return tokens != null && tokens.Any() ? tokens.Select(j => ((JProperty) j).Value).FirstOrDefault() : null;
		}


		public JObject GetClientProjection(JObject filteringObject, Criteria[] criteria)
		{
			foreach (var criterion in criteria)
			{
				var tokens = _jsonParser.FindJsonPath(filteringObject, criterion.Property);
				var value = GetPropertyValue(tokens.ToArray());
				if (value != null)
				{
					value.Parent.Remove();
				}
			}
			return filteringObject;
		}
	}
}