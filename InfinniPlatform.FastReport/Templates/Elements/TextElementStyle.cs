using InfinniPlatform.FastReport.Templates.Font;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Начертание текста.
	/// </summary>
	public sealed class TextElementStyle
	{
		/// <summary>
		/// Шрифт.
		/// </summary>
		public FontSetup Font { get; set; }

		/// <summary>
		/// Цвет шрифта.
		/// </summary>
		public string Foreground { get; set; }

		/// <summary>
		/// Цвет фона.
		/// </summary>
		public string Background { get; set; }

		/// <summary>
		/// Наклон текста.
		/// </summary>
		/// <remarks>Измеряется в градусах.</remarks>
		public float Angle { get; set; }

		/// <summary>
		/// Вертикальное выравнивание.
		/// </summary>
		public VerticalAlignment VerticalAlignment { get; set; }

		/// <summary>
		/// Горизонтальное выравнивание.
		/// </summary>
		public HorizontalAlignment HorizontalAlignment { get; set; }

		/// <summary>
		/// Перенос слов на новую строчку.
		/// </summary>
		public bool WordWrap { get; set; }
	}
}