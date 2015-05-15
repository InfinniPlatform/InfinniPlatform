using System;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки печати блока отчета.
	/// </summary>
	public sealed class ReportBandPrintSetupConfig
	{
		internal ReportBandPrintSetupConfig(ReportBandPrintSetup printSetup)
		{
			if (printSetup == null)
			{
				throw new ArgumentNullException("printSetup");
			}

			_printSetup = printSetup;
		}


		private readonly ReportBandPrintSetup _printSetup;


		/// <summary>
		/// Печатать на новой странице.
		/// </summary>
		public ReportBandPrintSetupConfig StartNewPage()
		{
			_printSetup.IsStartNewPage = true;

			return this;
		}

		/// <summary>
		/// Местоположение печати блока.
		/// </summary>
		public ReportBandPrintSetupConfig PrintOn(PrintTargets value)
		{
			_printSetup.PrintTargets = value;

			return this;
		}
	}
}