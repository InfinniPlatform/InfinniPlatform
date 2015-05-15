using System.Collections.Generic;

using FastReport;

using InfinniPlatform.FastReport.Templates.Print;

using PrintMode = InfinniPlatform.FastReport.Templates.Print.PrintMode;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Print
{
	sealed class PrintSetupTemplateBuilder : IReportObjectTemplateBuilder<PrintSetup>
	{
		public PrintSetup BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var report = (Report)reportObject;
			var reportPage = (ReportPage)report.Pages[0];

			return new PrintSetup
					   {
						   Paper = GetPrintPaper(reportPage),
						   Margin = GetPrintPaperMargin(reportPage),
						   Printer = GetPrinterSettings(report)
					   };
		}

		private static PrintPaper GetPrintPaper(ReportPage reportPage)
		{
			return (reportPage != null)
					   ? new PrintPaper
							 {
								 Width = reportPage.PaperWidth,
								 Height = reportPage.PaperHeight,
								 Orientation = reportPage.Landscape ? PaperOrientation.Landscape : PaperOrientation.Portrait
							 }
					   : null;
		}

		private static PrintPaperMargin GetPrintPaperMargin(ReportPage reportPage)
		{
			return (reportPage != null)
					   ? new PrintPaperMargin
							 {
								 Left = reportPage.LeftMargin,
								 Right = reportPage.RightMargin,
								 Top = reportPage.TopMargin,
								 Bottom = reportPage.BottomMargin,
								 MirrorOnEvenPages = reportPage.MirrorMargins
							 }
					   : null;
		}

		private static PrinterSettings GetPrinterSettings(Report report)
		{
			return (report.PrintSettings != null)
					   ? new PrinterSettings
							 {
								 PrintMode = PrintModes[report.PrintSettings.PrintMode],
								 PaperHeight = report.PrintSettings.PrintOnSheetWidth,
								 PaperWidth = report.PrintSettings.PrintOnSheetHeight
							 }
					   : null;
		}

		private static readonly Dictionary<global::FastReport.PrintMode, PrintMode> PrintModes
			= new Dictionary<global::FastReport.PrintMode, PrintMode>
				  {
					  { global::FastReport.PrintMode.Default, PrintMode.Default },
					  { global::FastReport.PrintMode.Split, PrintMode.Split },
					  { global::FastReport.PrintMode.Scale, PrintMode.Scale }
				  };
	}
}