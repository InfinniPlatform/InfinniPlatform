using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения процентов.
	/// </summary>
	public class PercentFormatConfig
	{
		internal PercentFormatConfig(PercentFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly PercentFormat _format;


		/// <summary>
		/// Количество знаков после запятой.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PercentFormatConfig DecimalDigits(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_format.DecimalDigits = value;

			return this;
		}

		/// <summary>
		/// Разделитель между группами.
		/// </summary>
		public PercentFormatConfig GroupSeparator(string value)
		{
			_format.GroupSeparator = value;

			return this;
		}

		/// <summary>
		/// Разделитель между целой и дробной частью.
		/// </summary>
		public PercentFormatConfig DecimalSeparator(string value)
		{
			_format.DecimalSeparator = value;

			return this;
		}

		/// <summary>
		/// Символ процента.
		/// </summary>
		public PercentFormatConfig PercentSymbol(string value)
		{
			_format.PercentSymbol = value;

			return this;
		}
	}
}