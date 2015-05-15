using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

using Moq;

using NUnit.Framework;

using FRReport = FastReport.Report;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportGroupBandBuilderTest
	{
		private static readonly ReportGroupBandBuilder Target = new ReportGroupBandBuilder();


		[Test]
		public void ShouldBuildReportDataBand()
		{
			// Given

			var template = new ReportGroupBand();
			var context = new FrReportObjectBuilderContext();

			var report = new FRReport();

			var reportPage = new ReportPage();
			report.Pages.Add(reportPage);

			var dataBand = new DataBand();
			reportPage.AddChild(dataBand);

			// When
			Target.BuildObject(context, template, dataBand);

			// Then
			Assert.IsInstanceOf<GroupHeaderBand>(dataBand.Parent);
			Assert.AreEqual(reportPage, dataBand.Parent.Parent);
			Assert.AreEqual(dataBand, dataBand.Parent.ChildObjects[0]);
			Assert.AreEqual(dataBand, ((GroupHeaderBand)dataBand.Parent).Data);
		}

		[Test]
		public void ShouldBuildReportDataBandWithAllProperties()
		{
			// Given

			var context = new FrReportObjectBuilderContext();
			var reportBandBuilder = new Mock<IReportObjectBuilder<ReportBand>>();
			context.RegisterBuilder(reportBandBuilder.Object);
			var dataBindBuilder = new Mock<IReportObjectBuilder<PropertyBind>>();
			context.RegisterBuilder(dataBindBuilder.Object);

			var template = new ReportGroupBand
							   {
								   Header = new ReportBand(),
								   Footer = new ReportBand(),
								   DataBind = new PropertyBind()
							   };

			var report = new FRReport();

			var reportPage = new ReportPage();
			report.Pages.Add(reportPage);

			var dataBand = new DataBand();
			reportPage.AddChild(dataBand);

			// When
			Target.BuildObject(context, template, dataBand);

			// Then
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Header, It.IsAny<object>()));
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Footer, It.IsAny<object>()));
			dataBindBuilder.Verify(m => m.BuildObject(context, (PropertyBind)template.DataBind, It.IsAny<object>()));
		}
	}
}