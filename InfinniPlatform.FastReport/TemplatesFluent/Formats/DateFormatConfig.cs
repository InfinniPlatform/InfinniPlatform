using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения даты.
	/// </summary>
	public sealed class DateFormatConfig
	{
		internal DateFormatConfig(DateFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly DateFormat _format;


		/// <summary>
		/// Короткое строковое представление даты (например, 01.01.2011).
		/// </summary>
		public void Short()
		{
			_format.Format = DateFormats.ShortDate;
		}

		/// <summary>
		/// Длинное строковое представление даты (например, 1 января 2011 г.).
		/// </summary>
		public void Long()
		{
			_format.Format = DateFormats.LongDate;
		}
	}
}