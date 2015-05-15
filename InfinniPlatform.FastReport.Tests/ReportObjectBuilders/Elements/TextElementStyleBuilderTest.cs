using System;
using System.Drawing;

using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Elements;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;

using NUnit.Framework;

using FontStyle = InfinniPlatform.FastReport.Templates.Font.FontStyle;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TextElementStyleBuilderTest
	{
		[Test]
		public void ShouldBuildTextElementStyle()
		{
			// Given

			var template = CreateTemplate(t => t
				.Angle(10)
				.WordWrap()
				.Foreground("Red")
				.Background("Green")
				.HorizontalAlignment(HorizontalAlignment.Center)
				.VerticalAlignment(VerticalAlignment.Center)
				.Font("Arial", 20, FontSizeUnit.Px, f => f
					.FontStyle(FontStyle.Italic)
					.FontWeight(FontWeight.Bold)
					.TextDecoration(TextDecoration.Underline)));

			var context = new FrReportObjectBuilderContext();
			var target = new TextElementStyleBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(10, parent.Angle);
			Assert.IsTrue(parent.WordWrap);
			Assert.AreEqual(Color.Red, parent.TextColor);
			Assert.AreEqual(Color.Green, parent.FillColor);
			Assert.AreEqual(HorzAlign.Center, parent.HorzAlign);
			Assert.AreEqual(VertAlign.Center, parent.VertAlign);
			Assert.AreEqual("Arial", parent.Font.FontFamily.Name);
			Assert.AreEqual(GraphicsUnit.Pixel, parent.Font.Unit);
			Assert.IsTrue(parent.Font.Italic);
			Assert.IsTrue(parent.Font.Bold);
			Assert.IsTrue(parent.Font.Underline);
		}

		private static TextElementStyle CreateTemplate(Action<TextElementStyleConfig> config)
		{
			var template = new TextElementStyle();
			config(new TextElementStyleConfig(template));
			return template;
		}
	}
}