using System;
using System.Collections;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataProviders
{
	static class DataProviderExtensions
	{
		/// <summary>
		/// Преобразовать данные провайдера в JSON-массив согласно указанной схеме данных.
		/// </summary>
		public static JArray ToJsonArray(this IDataProvider dataProvider, DataSchema dataSchema)
		{
			return CreateJArray(dataProvider, dataSchema, dataProvider);
		}


		private static JObject CreateJObject(object instance, DataSchema dataSchema, IDataProvider dataProvider)
		{
			var jObject = new JObject();

			if (instance != null)
			{
				foreach (var property in dataSchema.Properties)
				{
					var propertyName = property.Key;
					var propertySchema = property.Value;
					var propertyValue = dataProvider.GetPropertyValue(instance, propertyName);

					if (propertyValue != null)
					{
						JToken jPropertyValue;

						if (propertySchema.Type == SchemaDataType.Object)
						{
							jPropertyValue = CreateJObject(propertyValue, propertySchema, dataProvider);
						}
						else if (propertySchema.Type == SchemaDataType.Array)
						{
							jPropertyValue = CreateJArray(propertyValue, propertySchema.Items, dataProvider);
						}
						else
						{
							jPropertyValue = CreateJValue(propertyValue, propertySchema.Type);
						}

						jObject[propertyName] = jPropertyValue;
					}
				}
			}

			return jObject;
		}

		private static JArray CreateJArray(object instance, DataSchema dataSchema, IDataProvider dataProvider)
		{
			var jArray = new JArray();

			var array = instance as IEnumerable;

			if (array != null)
			{
				// Если элементами массива являются объекты
				if (dataSchema.Type == SchemaDataType.Object)
				{
					foreach (var item in array)
					{
						var jItem = CreateJObject(item, dataSchema, dataProvider);

						jArray.Add(jItem);
					}
				}
				// Если элементами массива являются массивы
				else if (dataSchema.Type == SchemaDataType.Array)
				{
					foreach (var item in array)
					{
						var jItem = CreateJArray(item, dataSchema.Items, dataProvider);

						jArray.Add(jItem);
					}
				}
				// Если элементами массива являются скалярные типы
				else
				{
					foreach (var item in array)
					{
						var jItem = CreateJValue(item, dataSchema.Type);

						jArray.Add(jItem);
					}
				}
			}

			return jArray;
		}

		private static JValue CreateJValue(object instance, SchemaDataType dataType)
		{
			return new JValue(instance.ConvertTo(dataType));
		}
	}
}