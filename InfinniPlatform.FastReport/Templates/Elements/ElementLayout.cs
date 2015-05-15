namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Расположение элемента представления.
	/// </summary>
	public sealed class ElementLayout
	{
		/// <summary>
		/// Отступ сверху.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Top { get; set; }

		/// <summary>
		/// Отступ слева.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Left { get; set; }

		/// <summary>
		/// Ширина.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Width { get; set; }

		/// <summary>
		/// Высота.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Height { get; set; }
	}
}