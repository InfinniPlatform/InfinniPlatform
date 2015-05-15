using FastReport;
using FastReport.Table;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;
using InfinniPlatform.FastReport.Templates.Elements;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ElementTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildElement()
		{
			// Given

			var textObject = new TextObject();
			var tableObject = new TableObject();

			var textElementBuilder = new Mock<IReportObjectTemplateBuilder<TextElement>>();
			var tableElementBuilder = new Mock<IReportObjectTemplateBuilder<TableElement>>();

			textElementBuilder.Setup(m => m.BuildTemplate(_context, textObject)).Returns(new TextElement());
			tableElementBuilder.Setup(m => m.BuildTemplate(_context, tableObject)).Returns(new TableElement());

			_context.RegisterBuilder(textElementBuilder.Object);
			_context.RegisterBuilder(tableElementBuilder.Object);

			// When
			var target = new ElementTemplateBuilder();
			var textElement = target.BuildTemplate(_context, textObject);
			var tableElement = target.BuildTemplate(_context, tableObject);

			// Then
			Assert.IsNotNull(textElement);
			Assert.IsNotNull(tableElement);
		}
	}
}