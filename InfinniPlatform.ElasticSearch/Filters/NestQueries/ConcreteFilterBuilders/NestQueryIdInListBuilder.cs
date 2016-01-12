using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Documents;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
	public sealed class NestQueryIdInListBuilder : IConcreteFilterBuilder
	{
		public IFilter Get(string field, object value)
		{
			IEnumerable<string> values;

			try
			{
				values = JArray.Parse((string)value).Where(s => s.Value<object>().ToString() != "{}"  ).Select(s => s.Value<string>()).ToList();
			}
			catch (Exception e)
			{
				throw new ArgumentException(Resources.ValueIsNotIdentifiersList);
			}

            return new NestQuery(Nest.Query<dynamic>.Ids(values));
		}
	}
}
