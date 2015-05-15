using System;

using InfinniPlatform.FastReport.Templates.Print;

namespace InfinniPlatform.FastReport.TemplatesFluent.Print
{
	/// <summary>
	/// Интерфейс для настройки принтера.
	/// </summary>
	public sealed class PrinterConfig
	{
		internal PrinterConfig(PrinterSettings printerSettings)
		{
			if (printerSettings == null)
			{
				throw new ArgumentNullException("printerSettings");
			}

			_printerSettings = printerSettings;
		}


		private readonly PrinterSettings _printerSettings;


		/// <summary>
		/// Режим печати.
		/// </summary>
		public PrinterConfig PrintMode(PrintMode value)
		{
			_printerSettings.PrintMode = value;

			return this;
		}

		/// <summary>
		/// Ширина (меньшая сторона) листа для печати.
		/// </summary>
		/// <param name="value">Значение в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrinterConfig PaperWidth(float value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_printerSettings.PaperWidth = value;

			return this;
		}

		/// <summary>
		/// Высота (большая сторона) листа для печати.
		/// </summary>
		/// <param name="value">Значение в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrinterConfig PaperHeight(float value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_printerSettings.PaperHeight = value;

			return this;
		}
	}
}