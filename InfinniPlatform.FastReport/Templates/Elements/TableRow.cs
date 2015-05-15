namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Строка таблицы.
	/// </summary>
	public sealed class TableRow
	{
		/// <summary>
		/// Индекс.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Высота.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Высота определяется автоматически.
		/// </summary>
		public bool AutoHeight { get; set; }
	}
}