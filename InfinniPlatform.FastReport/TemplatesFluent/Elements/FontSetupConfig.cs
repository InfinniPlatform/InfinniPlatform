using System;

using InfinniPlatform.FastReport.Templates.Font;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки шрифта.
	/// </summary>
	public sealed class FontSetupConfig
	{
		internal FontSetupConfig(FontSetup fontSetup)
		{
			if (fontSetup == null)
			{
				throw new ArgumentNullException("fontSetup");
			}

			_fontSetup = fontSetup;
		}


		private readonly FontSetup _fontSetup;


		/// <summary>
		/// Стиль шрифта.
		/// </summary>
		public FontSetupConfig FontStyle(FontStyle value)
		{
			_fontSetup.FontStyle = value;

			return this;
		}

		/// <summary>
		/// Степень растягивания шрифта по горизонтали. 
		/// </summary>
		public FontSetupConfig FontStretch(FontStretch value)
		{
			_fontSetup.FontStretch = value;

			return this;
		}

		/// <summary>
		/// Насыщенность шрифта.
		/// </summary>
		public FontSetupConfig FontWeight(FontWeight value)
		{
			_fontSetup.FontWeight = value;

			return this;
		}

		/// <summary>
		/// Оформление текста.
		/// </summary>
		public FontSetupConfig TextDecoration(TextDecoration value)
		{
			_fontSetup.TextDecoration = value;

			return this;
		}

		/// <summary>
		/// Вертикальное выравнивание шрифта.
		/// </summary>
		public FontSetupConfig FontVerticalAlign(FontVerticalAlign value)
		{
			_fontSetup.FontVerticalAlign = value;

			return this;
		}
	}
}