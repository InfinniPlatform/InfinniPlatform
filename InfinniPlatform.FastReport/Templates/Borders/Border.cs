namespace InfinniPlatform.FastReport.Templates.Borders
{
	/// <summary>
	/// Границы объекта.
	/// </summary>
	public sealed class Border
	{
		/// <summary>
		/// Левая граница.
		/// </summary>
		public BorderLine Left { get; set; }

		/// <summary>
		/// Правая граница.
		/// </summary>
		public BorderLine Right { get; set; }

		/// <summary>
		/// Верхняя граница.
		/// </summary>
		public BorderLine Top { get; set; }

		/// <summary>
		/// Нижняя граница.
		/// </summary>
		public BorderLine Bottom { get; set; }
	}
}