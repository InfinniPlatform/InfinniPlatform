using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportPageBandBuilderTest
	{
		private static readonly ReportPageBandBuilder Target = new ReportPageBandBuilder();


		[Test]
		public void ShouldBuildWhenNoPageBands()
		{
			// Given
			var template = new ReportPageBand();
			var context = new FrReportObjectBuilderContext();
			var parent = new ReportPage();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.IsNull(parent.PageHeader);
			Assert.IsNull(parent.PageFooter);
		}

		[Test]
		public void ShouldBuildWhenPageHeader()
		{
			// Given
			var template = new ReportPageBand { Header = new ReportBand() };
			var context = new FrReportObjectBuilderContext();
			var parent = new ReportPage();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.IsNotNull(parent.PageHeader);
			Assert.IsNull(parent.PageFooter);
		}

		[Test]
		public void ShouldBuildWhenPageFooter()
		{
			// Given
			var template = new ReportPageBand { Footer = new ReportBand() };
			var context = new FrReportObjectBuilderContext();
			var parent = new ReportPage();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.IsNull(parent.PageHeader);
			Assert.IsNotNull(parent.PageFooter);
		}

		[Test]
		public void ShouldBuildWhenPageHeaderAndFooter()
		{
			// Given
			var template = new ReportPageBand { Header = new ReportBand(), Footer = new ReportBand() };
			var context = new FrReportObjectBuilderContext();
			var parent = new ReportPage();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.IsNotNull(parent.PageHeader);
			Assert.IsNotNull(parent.PageHeader);
		}

		[Test]
		public void ShouldBuildReportPageBandWithAllProperties()
		{
			// Given

			var context = new FrReportObjectBuilderContext();
			var reportBandBuilder = new Mock<IReportObjectBuilder<ReportBand>>();
			context.RegisterBuilder(reportBandBuilder.Object);

			var template = new ReportPageBand { Header = new ReportBand(), Footer = new ReportBand() };

			var parent = new ReportPage();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Header, It.IsAny<object>()));
			reportBandBuilder.Verify(m => m.BuildObject(context, template.Footer, It.IsAny<object>()));
		}
	}
}