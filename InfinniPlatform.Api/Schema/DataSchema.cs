using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Schema
{
	/// <summary>
	/// Описание модели данных.
	/// </summary>
	/// <remarks>
	/// За основу была взята спецификация "JSON Schema".
	/// </remarks>
	public sealed class DataSchema
	{

		public DataSchema()
		{
			Properties = new Dictionary<string, DataSchema>();
		}

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Тип данных.
		/// </summary>
		/// <remarks>
		/// Модель должна иметь строго определенный тип.
		/// </remarks>
		public SchemaDataType Type { get; set; }

        /// <summary>
        /// Возможна ли сортировка
        /// </summary>
        public bool Sortable { get; set; }

		/// <summary>
		/// Описание элементов колеции.
		/// </summary>
		public DataSchema Items { get; set; }

		/// <summary>
		/// Описание свойств модели данных.
		/// </summary>
		public IDictionary<string, DataSchema> Properties { get; set; }
	}
}
