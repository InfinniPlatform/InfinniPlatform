using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Elements;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportBandTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportBand()
		{
			// Given

			var borderBuilder = new Mock<IReportObjectTemplateBuilder<FastReport.Templates.Borders.Border>>();
			var elementBuilder = new Mock<IReportObjectTemplateBuilder<IElement>>();
			var reportBandPrintSetupBuilder = new Mock<IReportObjectTemplateBuilder<ReportBandPrintSetup>>();

			_context.RegisterBuilder(borderBuilder.Object);
			_context.RegisterBuilder(elementBuilder.Object);
			_context.RegisterBuilder(reportBandPrintSetupBuilder.Object);

			var bandObject = new DataBand
								 {
									 Name = "DataBand1",
									 Height = 100 * Units.Millimeters,
									 CanGrow = true,
									 CanShrink = true,
									 Border = new Border()
								 };

			bandObject.AddChild(new TextObject());

			// When
			var target = new ReportBandTemplateBuilder();
			var result = target.BuildTemplate(_context, bandObject);

			// Then

			Assert.IsNotNull(result);
			Assert.AreEqual("DataBand1", result.Name);
			Assert.AreEqual(100, result.Height);
			Assert.AreEqual(true, result.CanGrow);
			Assert.AreEqual(true, result.CanShrink);

			borderBuilder.Verify(m => m.BuildTemplate(_context, bandObject.Border));
			elementBuilder.Verify(m => m.BuildTemplate(_context, bandObject.ChildObjects[0]));
			reportBandPrintSetupBuilder.Verify(m => m.BuildTemplate(_context, bandObject));
		}
	}
}