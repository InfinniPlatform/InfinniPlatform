using System;

using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки начертания текста.
	/// </summary>
	public sealed class TextElementStyleConfig
	{
		internal TextElementStyleConfig(TextElementStyle textElementStyle)
		{
			if (textElementStyle == null)
			{
				throw new ArgumentNullException("textElementStyle");
			}

			_textElementStyle = textElementStyle;
		}


		private readonly TextElementStyle _textElementStyle;


		/// <summary>
		/// Шрифт.
		/// </summary>
		/// <param name="family">Семейство шрифта.</param>
		/// <param name="size">Размер шрифта.</param>
		/// <param name="sizeUnit">Единицы измерения размера шрифта.</param>
		/// <param name="options">Дополнительные настройки шрифта.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public TextElementStyleConfig Font(string family, float size, FontSizeUnit sizeUnit = default(FontSizeUnit), Action<FontSetupConfig> options = null)
		{
			if (string.IsNullOrWhiteSpace(family))
			{
				throw new ArgumentNullException("family");
			}

			if (size <= 0)
			{
				throw new ArgumentOutOfRangeException("size");
			}

			if (_textElementStyle.Font == null)
			{
				_textElementStyle.Font = new FontSetup();
			}

			_textElementStyle.Font.FontFamily = family;
			_textElementStyle.Font.FontSize = size;
			_textElementStyle.Font.FontSizeUnit = sizeUnit;

			if (options != null)
			{
				options(new FontSetupConfig(_textElementStyle.Font));
			}

			return this;
		}

		/// <summary>
		/// Цвет шрифта (в формате #XXXXXX).
		/// </summary>
		public TextElementStyleConfig Foreground(string value)
		{
			_textElementStyle.Foreground = value;

			return this;
		}

		/// <summary>
		/// Цвет фона (в формате #XXXXXX).
		/// </summary>
		public TextElementStyleConfig Background(string value)
		{
			_textElementStyle.Background = value;

			return this;
		}

		/// <summary>
		/// Наклон текста (в градусах, от 0 до 360).
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public TextElementStyleConfig Angle(float value)
		{
			if (value < 0 || value > 360)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_textElementStyle.Angle = value;

			return this;
		}

		/// <summary>
		/// Вертикальное выравнивание.
		/// </summary>
		public TextElementStyleConfig VerticalAlignment(VerticalAlignment value)
		{
			_textElementStyle.VerticalAlignment = value;

			return this;
		}

		/// <summary>
		/// Горизонтальное выравнивание.
		/// </summary>
		public TextElementStyleConfig HorizontalAlignment(HorizontalAlignment value)
		{
			_textElementStyle.HorizontalAlignment = value;

			return this;
		}

		/// <summary>
		/// Перенос слов на новую строчку.
		/// </summary>
		public TextElementStyleConfig WordWrap()
		{
			_textElementStyle.WordWrap = true;

			return this;
		}
	}
}