using System.Collections;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к данным в формате JSON.
	/// </summary>
	sealed class JsonDataProvider : IDataProvider
	{
		public JsonDataProvider(IEnumerable jsonArray)
		{
			_jsonArray = jsonArray ?? new JObject[] { };
		}


		private readonly IEnumerable _jsonArray;


		public object GetPropertyValue(object instance, string propertyName)
		{
			object propertyValue;
			TryGetValue(instance, propertyName, out propertyValue);

			return propertyValue;
		}

		public static bool TryGetValue(object instance, string propertyName, out object propertyValue)
		{
			var success = false;

			propertyValue = null;

			var jObject = instance as JObject;

			if (jObject != null)
			{
				propertyValue = jObject[propertyName];

				if (propertyValue != null)
				{
					var jValue = propertyValue as JValue;

					if (jValue != null)
					{
						propertyValue = jValue.Value;
					}
				}

				success = true;
			}

			return success;
		}


		public IEnumerator GetEnumerator()
		{
			return _jsonArray.GetEnumerator();
		}
	}
}