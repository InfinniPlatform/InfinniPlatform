using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.Environment.Index;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
	public sealed class NestFilterIdInListBuilder : IConcreteFilterBuilder
	{
		public IFilter Get(string field, object value)
		{
			IEnumerable<string> values;

		    if (value == null || string.IsNullOrEmpty(value.ToString()))
		    {
                return new NestFilter(Nest.Filter<dynamic>.Ids(new string[0]));
		    } 

			try
			{
				values = JArray.Parse((string)value).Where(s => s.Value<object>().ToString() != "{}"  ).Select(s => s.Value<string>()).ToList();
			}
			catch (Exception e)
			{
				throw new ArgumentException(Resources.ValueIsNotIdentifiersList);
			}

            return new NestFilter(Nest.Filter<dynamic>.Ids(values));
		}
	}
}
