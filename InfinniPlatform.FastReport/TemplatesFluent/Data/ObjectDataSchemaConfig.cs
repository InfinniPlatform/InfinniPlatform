using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для описания модели данных объекта.
	/// </summary>
	public sealed class ObjectDataSchemaConfig
	{
		internal ObjectDataSchemaConfig()
		{
			_schema = new DataSchema { Type = SchemaDataType.Object };
		}


		private readonly DataSchema _schema;


		/// <summary>
		/// Определить идентификатор.
		/// </summary>
		public ObjectDataSchemaConfig Id(string id)
		{
			_schema.Id = id;

			return this;
		}


		/// <summary>
		/// Определить свойство, которое содержит скалярное значение заданного типа.
		/// </summary>
		public ObjectDataSchemaConfig Property(string name, SchemaDataType type)
		{
			AddProperty(name, new DataSchema { Type = type });

			return this;
		}

		/// <summary>
		/// Определить свойство, которое содержит объект с заданной структурой.
		/// </summary>
		public ObjectDataSchemaConfig Property(string name, Action<ObjectDataSchemaConfig> type)
		{
			var builder = new ObjectDataSchemaConfig();
			type(builder);

			AddProperty(name, builder.Build());

			return this;
		}


		/// <summary>
		/// Определить свойство, которое сожержит коллекцию, элементами которой являются скалярные значения заданного типа.
		/// </summary>
		public ObjectDataSchemaConfig Collection(string name, SchemaDataType itemsType)
		{
			AddProperty(name, new DataSchema { Type = SchemaDataType.Array, Items = new DataSchema { Type = itemsType } });

			return this;
		}

		/// <summary>
		/// Определить свойство, которое сожержит коллекцию, элементами которой являются объекты с заданной структурой.
		/// </summary>
		public ObjectDataSchemaConfig Collection(string name, Action<ObjectDataSchemaConfig> itemsType)
		{
			var builder = new ObjectDataSchemaConfig();
			itemsType(builder);

			AddProperty(name, new DataSchema { Type = SchemaDataType.Array, Items = builder.Build() });

			return this;
		}

		/// <summary>
		/// Определить свойство, которое сожержит коллекцию, элементами которой являются коллекции заданной структуры.
		/// </summary>
		public ObjectDataSchemaConfig Collection(string name, Action<CollectionDataSchemaConfig> itemsType)
		{
			var builder = new CollectionDataSchemaConfig();
			itemsType(builder);

			AddProperty(name, builder.Build());

			return this;
		}

		private void AddProperty(string name, DataSchema schema)
		{
			_schema.Type = SchemaDataType.Object;

			if (_schema.Properties == null)
			{
				_schema.Properties = new Dictionary<string, DataSchema>();
			}

			_schema.Properties.Add(name, schema);
		}


		internal DataSchema Build()
		{
			return _schema;
		}
	}
}