using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения денежных единиц.
	/// </summary>
	public sealed class CurrencyFormatConfig
	{
		internal CurrencyFormatConfig(CurrencyFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly CurrencyFormat _format;


		/// <summary>
		/// Количество знаков после запятой.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public CurrencyFormatConfig DecimalDigits(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			_format.DecimalDigits = value;

			return this;
		}

		/// <summary>
		/// Разделитель между группами.
		/// </summary>
		public CurrencyFormatConfig GroupSeparator(string value)
		{
			_format.GroupSeparator = value;

			return this;
		}

		/// <summary>
		/// Разделитель между целой и дробной частью.
		/// </summary>
		public CurrencyFormatConfig DecimalSeparator(string value)
		{
			_format.DecimalSeparator = value;

			return this;
		}

		/// <summary>
		/// Символ валюты.
		/// </summary>
		public CurrencyFormatConfig CurrencySymbol(string value)
		{
			_format.CurrencySymbol = value;

			return this;
		}
	}
}