using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ElementLayoutTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildElementLayout()
		{
			// Given
			var textObject = new TextObject
								 {
									 Top = 10 * Units.Millimeters,
									 Left = 20 * Units.Millimeters,
									 Width = 30 * Units.Millimeters,
									 Height = 40 * Units.Millimeters,
								 };

			// When
			var target = new ElementLayoutTemplateBuilder();
			var result = target.BuildTemplate(_context, textObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.Top);
			Assert.AreEqual(20, result.Left);
			Assert.AreEqual(30, result.Width);
			Assert.AreEqual(40, result.Height);
		}
	}
}