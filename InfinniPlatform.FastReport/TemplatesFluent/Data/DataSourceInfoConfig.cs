using System;
using System.Collections;
using System.Collections.Generic;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки информации о источнике данных.
	/// </summary>
	public sealed class DataSourceInfoConfig
	{
		internal DataSourceInfoConfig(DataSourceInfo dataSourceInfo)
		{
			if (dataSourceInfo == null)
			{
				throw new ArgumentNullException("dataSourceInfo");
			}

			_dataSourceInfo = dataSourceInfo;
		}


		private readonly DataSourceInfo _dataSourceInfo;


		/// <summary>
		/// Описание модели данных.
		/// </summary>
		public DataSourceInfoConfig Schema(Type type)
		{
			// Данный метод добавлен исключительно для удобства; его лучше сделать в виде метода расширения.

			_dataSourceInfo.Schema = CreateDataSchemaForObject(new Dictionary<string, Type>(), type);

			return this;
		}

		/// <summary>
		/// Описание модели данных.
		/// </summary>
		public DataSourceInfoConfig Schema(Action<ObjectDataSchemaConfig> action)
		{
			var confinguration = new ObjectDataSchemaConfig();
			action(confinguration);

			_dataSourceInfo.Schema = confinguration.Build();

			return this;
		}


		/// <summary>
		/// Поставщик данных.
		/// </summary>
		public DataSourceInfoConfig Provider(Action<DataProviderInfoConfig> action)
		{
			var confinguration = new DataProviderInfoConfig(_dataSourceInfo);
			action(confinguration);

			return this;
		}


		private static DataSchema CreateDataSchemaForObject(IDictionary<string, Type> knownTypes, Type type)
		{
			DataSchema result;

			var typeId = type.FullName;

			if (knownTypes.ContainsKey(typeId) == false)
			{
				knownTypes.Add(typeId, type);

				result = new DataSchema
							 {
								 Id = typeId,
								 Type = SchemaDataType.Object,
								 Properties = new Dictionary<string, DataSchema>()
							 };

				var properties = type.GetProperties();

				foreach (var propertyInfo in properties)
				{
					var propertyName = propertyInfo.Name;
					var propertyType = propertyInfo.PropertyType;

					SchemaDataType propertyDataType;

					if (PrimitiveTypes.TryGetValue(propertyType, out propertyDataType))
					{
						var propertySchema = new DataSchema { Type = propertyDataType };

						result.Properties.Add(propertyName, propertySchema);
					}
					else if (EnumerableType.IsAssignableFrom(propertyType))
					{
						var propertySchema = new DataSchema { Type = SchemaDataType.Array };

						var itemsType = propertyType.GetGenericArguments()[0];

						propertySchema.Items = CreateDataSchemaForArray(knownTypes, itemsType);

						result.Properties.Add(propertyName, propertySchema);
					}
					else
					{
						var propertySchema = CreateDataSchemaForObject(knownTypes, propertyType);

						result.Properties.Add(propertyName, propertySchema);
					}
				}
			}
			else
			{
				result = new DataSchema
				{
					Id = typeId
				};
			}

			return result;
		}

		private static DataSchema CreateDataSchemaForArray(IDictionary<string, Type> knownTypes, Type type)
		{
			DataSchema result;

			SchemaDataType propertyDataType;

			if (PrimitiveTypes.TryGetValue(type, out propertyDataType))
			{
				result = new DataSchema { Type = propertyDataType };
			}
			else if (EnumerableType.IsAssignableFrom(type))
			{
				var subitemsType = type.GetGenericArguments()[0];

				result = new DataSchema { Type = SchemaDataType.Array, Items = CreateDataSchemaForArray(knownTypes, subitemsType) };
			}
			else
			{
				result = CreateDataSchemaForObject(knownTypes, type);
			}

			return result;
		}


		private static readonly Type EnumerableType = typeof(IEnumerable);

		private static readonly Dictionary<Type, SchemaDataType> PrimitiveTypes
			= new Dictionary<Type, SchemaDataType>
				  {
					  { typeof(Boolean), SchemaDataType.Boolean },
					  { typeof(Byte), SchemaDataType.Integer },
					  { typeof(SByte), SchemaDataType.Integer },
					  { typeof(Int16), SchemaDataType.Integer },
					  { typeof(UInt16), SchemaDataType.Integer },
					  { typeof(Int32), SchemaDataType.Integer },
					  { typeof(UInt32), SchemaDataType.Integer },
					  { typeof(Int64), SchemaDataType.Integer },
					  { typeof(UInt64), SchemaDataType.Integer },
					  { typeof(Single), SchemaDataType.Float },
					  { typeof(Double), SchemaDataType.Float },
					  { typeof(Decimal), SchemaDataType.Float },
					  { typeof(Char), SchemaDataType.String },
					  { typeof(String), SchemaDataType.String },
					  { typeof(DateTime), SchemaDataType.DateTime }
				  };
	}
}