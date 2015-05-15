namespace InfinniPlatform.FastReport.Templates.Font
{
	/// <summary>
	/// Настройки шрифта.
	/// </summary>
	public sealed class FontSetup
	{
		/// <summary>
		/// Семейство шрифта.
		/// </summary>
		public string FontFamily { get; set; }

		/// <summary>
		/// Размер шрифта.
		/// </summary>
		public float FontSize { get; set; }

		/// <summary>
		/// Единицы измерения размера шрифта.
		/// </summary>
		public FontSizeUnit FontSizeUnit { get; set; }

		/// <summary>
		/// Стиль шрифта.
		/// </summary>
		public FontStyle? FontStyle { get; set; }

		/// <summary>
		/// Степень растягивания шрифта по горизонтали.
		/// </summary>
		public FontStretch? FontStretch { get; set; }

		/// <summary>
		/// Насыщенность шрифта.
		/// </summary>
		public FontWeight? FontWeight { get; set; }

		/// <summary>
		/// Оформление текста.
		/// </summary>
		public TextDecoration? TextDecoration { get; set; }

		/// <summary>
		/// Вертикальное выравнивание шрифта.
		/// </summary>
		public FontVerticalAlign? FontVerticalAlign { get; set; }
	}
}