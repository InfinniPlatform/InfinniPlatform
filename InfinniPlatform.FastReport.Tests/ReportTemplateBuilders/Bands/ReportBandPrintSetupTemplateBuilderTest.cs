using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportBandPrintSetupTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportBandPrintSetup()
		{
			// Given
			var bandObject = new DataBand
								 {
									 StartNewPage = true,
									 PrintOn = PrintOn.FirstPage | PrintOn.EvenPages
								 };

			// When
			var target = new ReportBandPrintSetupTemplateBuilder();
			var result = target.BuildTemplate(_context, bandObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(PrintTargets.FirstPage | PrintTargets.EvenPages, result.PrintTargets);
			Assert.IsTrue(result.IsStartNewPage);
		}
	}
}