using System;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для описания модели данных коллекции.
	/// </summary>
	public sealed class CollectionDataSchemaConfig
	{
		internal CollectionDataSchemaConfig()
		{
			_schema = new DataSchema { Type = SchemaDataType.Array };
		}


		private readonly DataSchema _schema;


		/// <summary>
		/// Определить вложенную коллекцию, элементами которой являются скалярные значения заданного типа.
		/// </summary>
		public CollectionDataSchemaConfig Collection(SchemaDataType itemsType)
		{
			_schema.Items = new DataSchema { Type = itemsType };

			return this;
		}

		/// <summary>
		/// Определить вложенную коллекцию, элементами которой являются объекты с заданной структурой.
		/// </summary>
		public CollectionDataSchemaConfig Collection(Action<ObjectDataSchemaConfig> itemsType)
		{
			var builder = new ObjectDataSchemaConfig();
			itemsType(builder);

			_schema.Items = new DataSchema { Type = SchemaDataType.Array, Items = builder.Build() };

			return this;
		}

		/// <summary>
		/// Определить вложенную коллекцию, элементами которой являются коллекции заданной структуры.
		/// </summary>
		public CollectionDataSchemaConfig Collection(Action<CollectionDataSchemaConfig> itemsType)
		{
			var builder = new CollectionDataSchemaConfig();
			itemsType(builder);

			_schema.Items = builder.Build();

			return this;
		}


		internal DataSchema Build()
		{
			return _schema;
		}
	}
}