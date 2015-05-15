namespace InfinniPlatform.FastReport.Templates.Print
{
	/// <summary>
	/// Отступы на листе при печати.
	/// </summary>
	public sealed class PrintPaperMargin
	{
		/// <summary>
		/// Отступ слева.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Left { get; set; }

		/// <summary>
		/// Отступ справа.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Right { get; set; }

		/// <summary>
		/// Отступ сверху.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Top { get; set; }

		/// <summary>
		/// Отступ снизу.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Bottom { get; set; }

		/// <summary>
		/// Зеркально отражать отступы на нечетных страницах.
		/// </summary>
		public bool MirrorOnEvenPages { get; set; }
	}
}