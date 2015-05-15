using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Ячейка таблицы.
	/// </summary>
	public sealed class TableCell : IFormatElement, IDataBindElement
	{
		/// <summary>
		/// Индекс строки таблицы.
		/// </summary>
		public int RowIndex { get; set; }

		/// <summary>
		/// Индекс столбца таблицы.
		/// </summary>
		public int ColumnIndex { get; set; }


		/// <summary>
		/// Границы элемента.
		/// </summary>
		public Border Border { get; set; }

		/// <summary>
		/// Начертание текста.
		/// </summary>
		public TextElementStyle Style { get; set; }


		/// <summary>
		/// Формат отображения.
		/// </summary>
		public IFormat Format { get; set; }

		/// <summary>
		/// Привязка данных.
		/// </summary>
		public IDataBind DataBind { get; set; }


		/// <summary>
		/// Объединить ячейку с соседними справа.
		/// </summary>
		public int ColSpan { get; set; }

		/// <summary>
		/// Объединить ячейку с соседними снизу.
		/// </summary>
		public int RowSpan { get; set; }
	}
}