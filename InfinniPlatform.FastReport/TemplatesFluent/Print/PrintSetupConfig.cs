using System;

using InfinniPlatform.FastReport.Templates.Print;

namespace InfinniPlatform.FastReport.TemplatesFluent.Print
{
	/// <summary>
	/// Интерфейс для настройки печати отчета.
	/// </summary>
	public sealed class PrintSetupConfig
	{
		internal PrintSetupConfig(PrintSetup printSetup)
		{
			if (printSetup == null)
			{
				throw new ArgumentNullException("printSetup");
			}

			_printSetup = printSetup;
		}


		private readonly PrintSetup _printSetup;


		/// <summary>
		/// Формат листа бумаги.
		/// </summary>
		public PrintSetupConfig Paper(Action<PrintPaperConfig> action)
		{
			if (_printSetup.Paper == null)
			{
				_printSetup.Paper = new PrintPaper();
			}

			var configuration = new PrintPaperConfig(_printSetup.Paper);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Отступы на листе при печати.
		/// </summary>
		public PrintSetupConfig Margin(Action<PrintPaperMarginConfig> action)
		{
			if (_printSetup.Margin == null)
			{
				_printSetup.Margin = new PrintPaperMargin();
			}

			var configuration = new PrintPaperMarginConfig(_printSetup.Margin);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Настройки принтера.
		/// </summary>
		public PrintSetupConfig Printer(Action<PrinterConfig> action)
		{
			if (_printSetup.Printer == null)
			{
				_printSetup.Printer = new PrinterSettings();
			}

			var configuration = new PrinterConfig(_printSetup.Printer);
			action(configuration);

			return this;
		}
	}
}