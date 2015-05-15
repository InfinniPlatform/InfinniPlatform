using System;

using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Elements;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TextElementBuilderTest
	{
		private static readonly TextElementBuilder Target = new TextElementBuilder();


		[Test]
		public void ShouldBuildTextElement()
		{
			// Given

			var template = CreateTemplate(t => t
				.CanGrow()
				.CanShrink());

			var context = new FrReportObjectBuilderContext();
			var parent = new ReportTitleBand();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual(1, parent.ChildObjects.Count);
			Assert.IsInstanceOf<TextObject>(parent.ChildObjects[0]);
		}

		[Test]
		public void ShouldBuildTextElementWithAllProperties()
		{
			// Given

			var template = CreateTemplate(t => t
				.CanGrow()
				.CanShrink()
				.Border(x => { })
				.Layout(x => { })
				.Style(x => { })
				.Format(x => x.Number())
				.Bind(x => x.Constant("1")));

			var context = new FrReportObjectBuilderContext();
			var borderBuilder = new Mock<IReportObjectBuilder<FastReport.Templates.Borders.Border>>();
			context.RegisterBuilder(borderBuilder.Object);
			var elementLayoutBuilder = new Mock<IReportObjectBuilder<ElementLayout>>();
			context.RegisterBuilder(elementLayoutBuilder.Object);
			var textElementStyleBuilder = new Mock<IReportObjectBuilder<TextElementStyle>>();
			context.RegisterBuilder(textElementStyleBuilder.Object);
			var numberFormatBuilder = new Mock<IReportObjectBuilder<NumberFormat>>();
			context.RegisterBuilder(numberFormatBuilder.Object);
			var constantBindBuilder = new Mock<IReportObjectBuilder<ConstantBind>>();
			context.RegisterBuilder(constantBindBuilder.Object);

			var parent = new ReportTitleBand();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			borderBuilder.Verify(m => m.BuildObject(context, template.Border, It.IsAny<object>()));
			elementLayoutBuilder.Verify(m => m.BuildObject(context, template.Layout, It.IsAny<object>()));
			textElementStyleBuilder.Verify(m => m.BuildObject(context, template.Style, It.IsAny<object>()));
			numberFormatBuilder.Verify(m => m.BuildObject(context, (NumberFormat)template.Format, It.IsAny<object>()));
			constantBindBuilder.Verify(m => m.BuildObject(context, (ConstantBind)template.DataBind, It.IsAny<object>()));
		}

		private static TextElement CreateTemplate(Action<TextElementConfig> config)
		{
			var template = new TextElement();
			config(new TextElementConfig(template));
			return template;
		}
	}
}