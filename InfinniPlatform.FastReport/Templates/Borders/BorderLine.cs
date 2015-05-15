namespace InfinniPlatform.FastReport.Templates.Borders
{
	/// <summary>
	/// Линия границы.
	/// </summary>
	public sealed class BorderLine
	{
		/// <summary>
		/// Толщина линии.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Цвет линии.
		/// </summary>
		/// <remarks>HTML-представление цвета.</remarks>
		/// <example>Red, #57ABFF.</example>
		public string Color { get; set; }

		/// <summary>
		/// Стиль линии.
		/// </summary>
		public BorderLineStyle Style { get; set; }
	}
}