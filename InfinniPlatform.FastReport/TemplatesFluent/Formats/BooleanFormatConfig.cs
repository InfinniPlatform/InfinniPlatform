using System;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения логического значения.
	/// </summary>
	public sealed class BooleanFormatConfig
	{
		internal BooleanFormatConfig(BooleanFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}

			_format = format;
		}


		private readonly BooleanFormat _format;


		/// <summary>
		/// Текст для отображения ложного значения.
		/// </summary>
		public BooleanFormatConfig FalseText(string value)
		{
			_format.FalseText = value;

			return this;
		}

		/// <summary>
		/// Текст для отображения истинного значения.
		/// </summary>
		public BooleanFormatConfig TrueText(string value)
		{
			_format.TrueText = value;

			return this;
		}
	}
}