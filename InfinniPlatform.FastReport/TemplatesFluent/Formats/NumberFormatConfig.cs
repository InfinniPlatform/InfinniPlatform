using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения числового значения.
	/// </summary>
	public sealed class NumberFormatConfig
	{
		internal NumberFormatConfig(NumberFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly NumberFormat _format;


		/// <summary>
		/// Количество знаков после запятой.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public NumberFormatConfig DecimalDigits(int value)
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
		public NumberFormatConfig GroupSeparator(string value)
		{
			_format.GroupSeparator = value;

			return this;
		}

		/// <summary>
		/// Разделитель между целой и дробной частью.
		/// </summary>
		public NumberFormatConfig DecimalSeparator(string value)
		{
			_format.DecimalSeparator = value;

			return this;
		}
	}
}