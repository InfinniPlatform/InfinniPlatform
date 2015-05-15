using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportPageBandTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportPageBand()
		{
			// Given

			var reportBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportBand>>();

			_context.RegisterBuilder(reportBandBuilder.Object);

			var reportPage = new ReportPage
								 {
									 PageHeader = new PageHeaderBand(),
									 PageFooter = new PageFooterBand()
								 };

			// When
			var target = new ReportPageBandTemplateBuilder();
			var result = target.BuildTemplate(_context, reportPage);

			// Then

			Assert.IsNotNull(result);

			reportBandBuilder.Verify(m => m.BuildTemplate(_context, reportPage.PageHeader));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, reportPage.PageFooter));
		}
	}
}