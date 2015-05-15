using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Print;
using InfinniPlatform.FastReport.Templates.Print;

using NUnit.Framework;

using FRReport = FastReport.Report;
using FRReportPage = FastReport.ReportPage;
using FRPrintMode = FastReport.PrintMode;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Print
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintSetupTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();

		[Test]
		public void ShouldBuildPrintSetup()
		{
			// Given

			var report = new FRReport();
			var reportPage = new FRReportPage();
			report.Pages.Add(reportPage);

			report.PrintSettings.PrintOnSheetHeight = 100;
			report.PrintSettings.PrintOnSheetWidth = 200;
			report.PrintSettings.PrintMode = FRPrintMode.Scale;

			reportPage.PaperHeight = 100;
			reportPage.PaperWidth = 200;
			reportPage.Landscape = true;

			reportPage.LeftMargin = 1;
			reportPage.RightMargin = 2;
			reportPage.TopMargin = 3;
			reportPage.BottomMargin = 4;
			reportPage.MirrorMargins = true;

			// When
			var target = new PrintSetupTemplateBuilder();
			var result = target.BuildTemplate(_context, report);

			// Then

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Printer);
			Assert.IsNotNull(result.Paper);
			Assert.IsNotNull(result.Margin);

			Assert.AreEqual(100, result.Printer.PaperWidth);
			Assert.AreEqual(200, result.Printer.PaperHeight);
			Assert.AreEqual(PrintMode.Scale, result.Printer.PrintMode);

			Assert.AreEqual(100, result.Paper.Width);
			Assert.AreEqual(200, result.Paper.Height);
			Assert.AreEqual(PaperOrientation.Landscape, result.Paper.Orientation);

			Assert.AreEqual(1, result.Margin.Left);
			Assert.AreEqual(2, result.Margin.Right);
			Assert.AreEqual(3, result.Margin.Top);
			Assert.AreEqual(4, result.Margin.Bottom);
		}
	}
}