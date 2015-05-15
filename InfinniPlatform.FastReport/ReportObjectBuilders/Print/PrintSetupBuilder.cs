using System.Collections.Generic;

using FastReport;

using InfinniPlatform.FastReport.Templates.Print;

using PrintMode = InfinniPlatform.FastReport.Templates.Print.PrintMode;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Print
{
	sealed class PrintSetupBuilder : IReportObjectBuilder<PrintSetup>
	{
		public void BuildObject(IReportObjectBuilderContext context, PrintSetup template, object parent)
		{
			var report = (Report)parent;
			var reportPage = (ReportPage)report.Pages[0];

			SetPrinterSettings(report, template);
			SetPrintSetup(reportPage, template);
		}

		private static void SetPrinterSettings(Report report, PrintSetup printSetup)
		{
			if (printSetup.Printer != null)
			{
				report.PrintSettings.PrintMode = PrintModes[printSetup.Printer.PrintMode];

				if (printSetup.Printer.PaperWidth > 0)
				{
					report.PrintSettings.PrintOnSheetWidth = printSetup.Printer.PaperHeight;
				}

				if (printSetup.Printer.PaperHeight > 0)
				{
					report.PrintSettings.PrintOnSheetHeight = printSetup.Printer.PaperWidth;
				}
			}
		}

		private static void SetPrintSetup(ReportPage reportPage, PrintSetup printSetup)
		{
			if (printSetup.Paper != null)
			{
				reportPage.PaperWidth = printSetup.Paper.Width;
				reportPage.PaperHeight = printSetup.Paper.Height;
				reportPage.Landscape = (printSetup.Paper.Orientation == PaperOrientation.Landscape);
			}

			if (printSetup.Margin != null)
			{
				reportPage.LeftMargin = printSetup.Margin.Left;
				reportPage.RightMargin = printSetup.Margin.Right;
				reportPage.TopMargin = printSetup.Margin.Top;
				reportPage.BottomMargin = printSetup.Margin.Bottom;
				reportPage.MirrorMargins = printSetup.Margin.MirrorOnEvenPages;
			}
		}

		private static readonly Dictionary<PrintMode, global::FastReport.PrintMode> PrintModes
			= new Dictionary<PrintMode, global::FastReport.PrintMode>
				  {
					  { PrintMode.Default, global::FastReport.PrintMode.Default },
					  { PrintMode.Split, global::FastReport.PrintMode.Split },
					  { PrintMode.Scale, global::FastReport.PrintMode.Scale }
				  };
	}
}