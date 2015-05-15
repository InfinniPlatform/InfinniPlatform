using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;

using NUnit.Framework;

using DrawingFont = System.Drawing.Font;
using DrawingColor = System.Drawing.Color;
using DrawingFontStyle = System.Drawing.FontStyle;
using DrawingGraphicsUnit = System.Drawing.GraphicsUnit;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TextElementStyleTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildTextElementStyle()
		{
			// Given
			var textObject = new TextObject
								 {
									 Font = new DrawingFont("Arial", 10, DrawingFontStyle.Bold | DrawingFontStyle.Italic | DrawingFontStyle.Underline, DrawingGraphicsUnit.Pixel),
									 TextColor = DrawingColor.Red,
									 FillColor = DrawingColor.Green,
									 Angle = 100,
									 VertAlign = VertAlign.Center,
									 HorzAlign = HorzAlign.Center,
									 WordWrap = true
								 };

			// When
			var target = new TextElementStyleTemplateBuilder();
			var result = target.BuildTemplate(_context, textObject);

			// Then
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Font);
			Assert.AreEqual("Arial", result.Font.FontFamily);
			Assert.AreEqual(10, result.Font.FontSize);
			Assert.AreEqual(FontSizeUnit.Px, result.Font.FontSizeUnit);
			Assert.AreEqual(FontStyle.Italic, result.Font.FontStyle);
			Assert.AreEqual(FontWeight.Bold, result.Font.FontWeight);
			Assert.AreEqual(TextDecoration.Underline, result.Font.TextDecoration);
			Assert.AreEqual("Red", result.Foreground);
			Assert.AreEqual("Green", result.Background);
			Assert.AreEqual(100, result.Angle);
			Assert.AreEqual(VerticalAlignment.Center, result.VerticalAlignment);
			Assert.AreEqual(HorizontalAlignment.Center, result.HorizontalAlignment);
			Assert.AreEqual(true, result.WordWrap);
		}
	}
}