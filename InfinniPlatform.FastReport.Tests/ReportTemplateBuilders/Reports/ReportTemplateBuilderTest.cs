using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Reports;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Print;

using Moq;

using NUnit.Framework;

using FRReport = FastReport.Report;
using FRReportPage = FastReport.ReportPage;
using ReportInfo = InfinniPlatform.FastReport.Templates.Reports.ReportInfo;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Reports
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportInfo()
		{
			// Given

			var expectedReportInfo = new ReportInfo
										 {
											 Name = "ReportName",
											 Description = "ReportDescription"
										 };

			var report = new FRReport
							 {
								 ReportInfo =
									 {
										 Name = expectedReportInfo.Name,
										 Description = expectedReportInfo.Description
									 }
							 };

			report.Pages.Add(new FRReportPage());

			// When
			var target = new ReportTemplateBuilder();
			var result = target.BuildTemplate(_context, report);

			// Then
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Info);
			Assert.AreEqual(expectedReportInfo.Name, result.Info.Name);
			Assert.AreEqual(expectedReportInfo.Description, result.Info.Description);
		}

		[Test]
		public void ShouldBuildReportInfoWhenDescriptionIsNull()
		{
			var report = new FRReport
							 {
								 ReportInfo =
									 {
										 Name = "Untitled1",
										 Description = string.Empty
									 }
							 };

			report.Pages.Add(new FRReportPage());

			// When
			var target = new ReportTemplateBuilder();
			var result = target.BuildTemplate(_context, report);

			// Then
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Info);
			Assert.AreEqual("Untitled1", result.Info.Name);
		}

		[Test]
		public void ShouldBuildReportTemplate()
		{
			// Given

			var printSetupBuilder = new Mock<IReportObjectTemplateBuilder<PrintSetup>>();
			var reportBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportBand>>();
			var reportPageBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportPageBand>>();
			var reportDataBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportDataBand>>();

			_context.RegisterBuilder(printSetupBuilder.Object);
			_context.RegisterBuilder(reportBandBuilder.Object);
			_context.RegisterBuilder(reportPageBandBuilder.Object);
			_context.RegisterBuilder(reportDataBandBuilder.Object);

			var report = new FRReport();
			var reportPage = new FRReportPage();
			report.Pages.Add(reportPage);
			var dataBand = new DataBand();
			reportPage.Bands.Add(dataBand);
			reportPage.ReportTitle = new ReportTitleBand();
			reportPage.ReportSummary = new ReportSummaryBand();

			// When
			var target = new ReportTemplateBuilder();
			var result = target.BuildTemplate(_context, report);

			// Then

			Assert.IsNotNull(result);

			printSetupBuilder.Verify(m => m.BuildTemplate(_context, report));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, reportPage.ReportTitle));
			reportPageBandBuilder.Verify(m => m.BuildTemplate(_context, reportPage));
			reportDataBandBuilder.Verify(m => m.BuildTemplate(_context, dataBand));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, reportPage.ReportSummary));
		}
	}
}