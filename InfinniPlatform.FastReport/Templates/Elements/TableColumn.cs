namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Столбец таблицы.
	/// </summary>
	public sealed class TableColumn
	{
		/// <summary>
		/// Индекс.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Ширина.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Ширина определяется автоматически.
		/// </summary>
		public bool AutoWidth { get; set; }
	}
}