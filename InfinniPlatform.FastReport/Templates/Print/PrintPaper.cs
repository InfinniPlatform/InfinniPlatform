namespace InfinniPlatform.FastReport.Templates.Print
{
	/// <summary>
	/// Формат листа бумаги.
	/// </summary>
	public sealed class PrintPaper
	{
		/// <summary>
		/// Ширина (меньшая сторона) листа.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Width { get; set; }

		/// <summary>
		/// Высота (большая сторона) листа.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Height { get; set; }

		/// <summary>
		/// Ориентация листа.
		/// </summary>
		public PaperOrientation Orientation { get; set; }
	}
}