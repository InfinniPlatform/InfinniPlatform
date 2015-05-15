using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;
using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

using Moq;

using NUnit.Framework;

using FRBorder = FastReport.Border;
using FRFromat = FastReport.Format.DateFormat;
using FRTextObject = FastReport.TextObject;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TextElementTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildTextElement()
		{
			// Given

			var borderBuilder = new Mock<IReportObjectTemplateBuilder<Border>>();
			var elementLayoutBuilder = new Mock<IReportObjectTemplateBuilder<ElementLayout>>();
			var textElementStyleBuilder = new Mock<IReportObjectTemplateBuilder<TextElementStyle>>();
			var formatBuilder = new Mock<IReportObjectTemplateBuilder<IFormat>>();
			var dataBindBuilder = new Mock<IReportObjectTemplateBuilder<IDataBind>>();

			_context.RegisterBuilder(borderBuilder.Object);
			_context.RegisterBuilder(elementLayoutBuilder.Object);
			_context.RegisterBuilder(textElementStyleBuilder.Object);
			_context.RegisterBuilder(formatBuilder.Object);
			_context.RegisterBuilder(dataBindBuilder.Object);

			var textObject = new FRTextObject
								 {
									 Border = new FRBorder(),
									 Format = new FRFromat(),
									 Text = "[DataSource1.Property1]",
									 CanGrow = true,
									 CanShrink = true
								 };

			// When
			var target = new TextElementTemplateBuilder();
			var result = target.BuildTemplate(_context, textObject);

			// Then

			Assert.IsNotNull(result);
			Assert.IsTrue(result.CanGrow);
			Assert.IsTrue(result.CanShrink);

			borderBuilder.Verify(m => m.BuildTemplate(_context, textObject.Border));
			elementLayoutBuilder.Verify(m => m.BuildTemplate(_context, textObject));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, textObject));
			formatBuilder.Verify(m => m.BuildTemplate(_context, textObject.Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, textObject.Text));
		}
	}
}