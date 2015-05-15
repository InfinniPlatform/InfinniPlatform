using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Data;
using InfinniPlatform.FastReport.Templates.Data;

using Moq;

using NUnit.Framework;

using FRReport = FastReport.Report;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TotalInfoBuilderTest
	{
		[Test]
		public void ShouldBuildTotalForDataBand()
		{
			// Given

			var template = new TotalInfo
							   {
								   Name = "Total1",
								   DataBand = "DataBand1",
								   PrintBand = "SummaryBand1",
								   TotalFunc = TotalFunc.Sum,
								   Expression = new PropertyBind
													{
														DataSource = "Order",
														Property = "Cost"
													}
							   };

			var context = new FrReportObjectBuilderContext();
			var dataBindBuilder = new Mock<IReportObjectBuilder<PropertyBind>>();
			context.RegisterBuilder(dataBindBuilder.Object);

			var target = new TotalInfoBuilder();

			var report = new FRReport();

			var reportPage = new ReportPage();
			report.Pages.Add(reportPage);

			var dataBand = new DataBand { Name = "DataBand1" };
			reportPage.Bands.Add(dataBand);

			var summaryBand = new ReportSummaryBand { Name = "SummaryBand1" };
			reportPage.ReportSummary = summaryBand;

			// When
			target.BuildObject(context, template, report);

			// Then

			Assert.AreEqual(1, report.Dictionary.Totals.Count);
			Assert.AreEqual("Total1", report.Dictionary.Totals[0].Name);
			Assert.AreEqual(dataBand, report.Dictionary.Totals[0].Evaluator);
			Assert.AreEqual(summaryBand, report.Dictionary.Totals[0].PrintOn);
			Assert.AreEqual(TotalType.Sum, report.Dictionary.Totals[0].TotalType);

			dataBindBuilder.Verify(m => m.BuildObject(context, (PropertyBind)template.Expression, report.Dictionary.Totals[0]));
		}
	}
}