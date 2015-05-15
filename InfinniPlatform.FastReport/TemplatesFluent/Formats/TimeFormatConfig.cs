using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения времени.
	/// </summary>
	public sealed class TimeFormatConfig
	{
		internal TimeFormatConfig(TimeFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly TimeFormat _format;


		/// <summary>
		/// Короткое строковое представление времени (например, 12:34).
		/// </summary>
		public void Short()
		{
			_format.Format = TimeFormats.ShortTime;
		}

		/// <summary>
		/// Длинное строковое представление времени (например, 12:34:56).
		/// </summary>
		public void Long()
		{
			_format.Format = TimeFormats.LongTime;
		}
	}
}