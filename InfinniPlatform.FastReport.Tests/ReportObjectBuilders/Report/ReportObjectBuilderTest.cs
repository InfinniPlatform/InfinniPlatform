using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Reports;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Print;
using InfinniPlatform.FastReport.Templates.Reports;

using Moq;

using NUnit.Framework;

using FRReport = FastReport.Report;
using FRReportPage = FastReport.ReportPage;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Report
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportObjectBuilderTest
	{
		private static readonly ReportObjectBuilder Target = new ReportObjectBuilder();


		[Test]
		[TestCase("ReportName", "", "ReportName")]
		[TestCase("ReportName", null, "ReportName")]
		[TestCase("ReportName", "ReportCaption", "ReportName")]
		public void ShouldBuildReportInfo(string reportName, string reportCaption, string expectedReportCaption)
		{
			// Given

			var template = new ReportTemplate
							   {
								   Info = new ReportInfo
											  {
												  Name = reportName,
												  Caption = reportCaption,
												  Description = "ReportDescription",
											  }
							   };

			var context = new FrReportObjectBuilderContext();

			// When
			Target.BuildObject(context, template, null);

			// Then

			var report = context.Report as FRReport;
			Assert.IsNotNull(report);

			var reportInfo = report.ReportInfo;
			Assert.IsNotNull(reportInfo);
			Assert.IsNotNull(reportInfo.Description);
			Assert.AreEqual(expectedReportCaption, reportInfo.Name);

			Assert.AreEqual(template.Info.Name, reportInfo.Name);
			Assert.AreEqual(template.Info.Description, reportInfo.Description);
		}

		[Test]
		public void ShouldBuildReport()
		{
			// Given
			var template = new ReportTemplate();
			var context = new FrReportObjectBuilderContext();

			// When
			Target.BuildObject(context, template, null);

			// Then

			var report = context.Report as FRReport;
			Assert.IsNotNull(report);

			var reportPage = report.Pages[0] as FRReportPage;
			Assert.IsNotNull(reportPage);

			Assert.IsNull(reportPage.ReportTitle);
			Assert.IsNull(reportPage.ReportSummary);
		}

		[Test]
		public void ShouldBuildReportWithTitle()
		{
			// Given
			var template = new ReportTemplate { Title = new ReportBand() };
			var context = new FrReportObjectBuilderContext();

			// When
			Target.BuildObject(context, template, null);

			// Then

			var report = context.Report as FRReport;
			Assert.IsNotNull(report);

			var reportPage = report.Pages[0] as FRReportPage;
			Assert.IsNotNull(reportPage);

			Assert.IsNotNull(reportPage.ReportTitle);
			Assert.IsNull(reportPage.ReportSummary);
		}

		[Test]
		public void ShouldBuildReportWithSummary()
		{
			// Given
			var template = new ReportTemplate { Summary = new ReportBand() };
			var context = new FrReportObjectBuilderContext();

			// When
			Target.BuildObject(context, template, null);

			// Then

			var report = context.Report as FRReport;
			Assert.IsNotNull(report);

			var reportPage = report.Pages[0] as FRReportPage;
			Assert.IsNotNull(reportPage);

			Assert.IsNull(reportPage.ReportTitle);
			Assert.IsNotNull(reportPage.ReportSummary);
		}

		[Test]
		public void ShouldBuildReportWithAllProperties()
		{
			// Given

			var context = new FrReportObjectBuilderContext();
			var parameterBuilder = new Mock<IReportObjectBuilder<ParameterInfo>>();
			context.RegisterBuilder(parameterBuilder.Object);
			var dataSourceBuilder = new Mock<IReportObjectBuilder<DataSourceInfo>>();
			context.RegisterBuilder(dataSourceBuilder.Object);
			var printSetupBuilder = new Mock<IReportObjectBuilder<PrintSetup>>();
			context.RegisterBuilder(printSetupBuilder.Object);
			var reportBandBuilder = new Mock<IReportObjectBuilder<ReportBand>>();
			context.RegisterBuilder(reportBandBuilder.Object);
			var reportPageBandBuilder = new Mock<IReportObjectBuilder<ReportPageBand>>();
			context.RegisterBuilder(reportPageBandBuilder.Object);
			var reportDataBandBuilder = new Mock<IReportObjectBuilder<ReportDataBand>>();
			context.RegisterBuilder(reportDataBandBuilder.Object);
			var totalBuilder = new Mock<IReportObjectBuilder<TotalInfo>>();
			context.RegisterBuilder(totalBuilder.Object);

			var template = new ReportTemplate
							   {
								   Parameters = new List<ParameterInfo> { new ParameterInfo() },
								   DataSources = new List<DataSourceInfo> { new DataSourceInfo() },
								   Totals = new List<TotalInfo> { new TotalInfo() },
								   PrintSetup = new PrintSetup(),
								   Title = new ReportBand(),
								   Page = new ReportPageBand(),
								   Data = new ReportDataBand(),
								   Summary = new ReportBand()
							   };

			// When
			Target.BuildObject(context, template, null);

			// Then
			parameterBuilder.Verify(m => m.BuildObject(context, template.Parameters.First(), It.IsAny<object>()));
			dataSourceBuilder.Verify(m => m.BuildObject(context, template.DataSources.First(), It.IsAny<object>()));
			printSetupBuilder.Verify(m => m.BuildObject(context, template.PrintSetup, It.IsAny<object>()));
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Title, It.IsAny<object>()));
			reportPageBandBuilder.Verify(m => m.BuildObject(context, template.Page, It.IsAny<object>()));
			reportDataBandBuilder.Verify(m => m.BuildObject(context, template.Data, It.IsAny<object>()));
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Summary, It.IsAny<object>()));
			totalBuilder.Verify(m => m.BuildObject(context, template.Totals.First(), It.IsAny<object>()));
		}
	}
}