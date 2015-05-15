using System.Collections.Generic;

using FastReport.Data;
using FastReport.Utils;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	/// <summary>
	/// Источник данных FastReport для JSON-объектов.
	/// </summary>
	sealed class JsonObjectDataSource : BusinessObjectDataSource
	{
		static JsonObjectDataSource()
		{
			RegisteredObjects.Add(typeof(JsonObjectDataSource), "", 0);
		}

		protected override object GetValue(Column propertyColumn)
		{
			object result = null;

			if (propertyColumn != null)
			{
				// Получение пути до свойства

				var propertyPath = new Stack<Column>();

				while (propertyColumn != this)
				{
					propertyPath.Push(propertyColumn);
					propertyColumn = (Column)propertyColumn.Parent;
				}

				// Получение значения свойства

				var propertyValue = CurrentRow;

				foreach (var property in propertyPath)
				{
					var jObject = propertyValue as JObject;

					if (jObject != null)
					{
						propertyValue = jObject[property.Name];

						if (propertyValue is JValue)
						{
							propertyValue = ((JValue)propertyValue).Value;
						}
					}
					else
					{
						propertyValue = null;
						break;
					}
				}

				result = propertyValue;
			}

			return result;
		}
	}
}