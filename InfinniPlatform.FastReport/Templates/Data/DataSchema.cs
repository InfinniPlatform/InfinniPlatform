using System.Collections.Generic;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Описание модели данных.
	/// </summary>
	/// <remarks>
	/// За основу была взята спецификация "JSON Schema".
	/// </remarks>
	public sealed class DataSchema
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		/// <remarks>
		/// Модель с типом <see cref="DataType.Object"/> должна иметь идентификатор.
		/// </remarks>
		public string Id { get; set; }

		/// <summary>
		/// Тип данных.
		/// </summary>
		/// <remarks>
		/// Модель должна иметь строго определенный тип, отличный от <see cref="DataType.None"/>, либо в свойстве <see cref="Id"/> должна быть ссылка на ранее определенный тип.
		/// </remarks>
		public DataType Type { get; set; }

		/// <summary>
		/// Описание элементов колеции.
		/// </summary>
		/// <remarks>
		/// Модель с типом <see cref="DataType.Array"/> должна иметь описание элементов коллекции.
		/// </remarks>
		public DataSchema Items { get; set; }

		/// <summary>
		/// Описание свойств модели данных.
		/// </summary>
		/// <remarks>
		/// Модель с типом <see cref="DataType.Object"/> должна иметь описание свойств модели данных.
		/// </remarks>
		public IDictionary<string, DataSchema> Properties { get; set; }
	}
}