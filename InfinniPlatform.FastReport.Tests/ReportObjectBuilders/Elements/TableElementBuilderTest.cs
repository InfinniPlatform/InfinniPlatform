using System;
using System.Linq;

using FastReport;
using FastReport.Table;
using FastReport.Utils;

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
	public sealed class TableElementBuilderTest
	{
		private static readonly TableElementBuilder Target = new TableElementBuilder();


		[Test]
		public void ShouldBuildTableElement()
		{
			// Given

			var template = CreateTemplate(t => t
				.Grid(
					rows => rows
						.Row(r => r.Height(5))
						.Row(r => r.Height(6))
						.Row(r => r.Height(7)),
					columns => columns
						.Column(c => c.Width(21))
						.Column(c => c.Width(22)),
					cells => cells
						.Cell(0, 0, c => { })
						.Cell(0, 1, c => { })
						.Cell(1, 0, c => { })
						.Cell(1, 1, c => { })
						.Cell(2, 0, c => { })
						.Cell(2, 1, c => { })));

			var context = new FrReportObjectBuilderContext();
			var parent = new ReportTitleBand();

			// When
			Target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(1, parent.ChildObjects.Count);

			var tableObject = parent.ChildObjects[0] as TableObject;
			Assert.IsNotNull(tableObject);

			Assert.AreEqual(3, tableObject.RowCount);
			Assert.AreEqual(2, tableObject.ColumnCount);

			Assert.AreEqual(5 * Units.Millimeters, tableObject.Rows[0].Height, 0.00001);
			Assert.AreEqual(false, tableObject.Rows[0].AutoSize);
			Assert.AreEqual(6 * Units.Millimeters, tableObject.Rows[1].Height, 0.00001);
			Assert.AreEqual(false, tableObject.Rows[1].AutoSize);
			Assert.AreEqual(7 * Units.Millimeters, tableObject.Rows[2].Height, 0.00001);
			Assert.AreEqual(false, tableObject.Rows[2].AutoSize);

			Assert.AreEqual(21 * Units.Millimeters, tableObject.Columns[0].Width, 0.00001);
			Assert.AreEqual(false, tableObject.Columns[0].AutoSize);
			Assert.AreEqual(22 * Units.Millimeters, tableObject.Columns[1].Width, 0.00001);
			Assert.AreEqual(false, tableObject.Columns[1].AutoSize);
		}

		[Test]
		public void ShouldBuildTableElementWithElement()
		{
			// Given

			var template = CreateTemplate(t => t
				.Border(b => { })
				.Layout(l => { })
				.Grid(
					rows => rows
						.Row(r => r.Height(5)),
					columns => columns
						.Column(c => c.Width(21)),
					cells => cells
						.Cell(0, 0, c => c
							.Border(x => { })
							.Style(x => { })
							.Format(x => x.Number())
							.Bind(x => x.Constant("1")))));

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

			borderBuilder.Verify(m => m.BuildObject(context, template.Cells.First().Border, It.IsAny<object>()));
			textElementStyleBuilder.Verify(m => m.BuildObject(context, template.Cells.First().Style, It.IsAny<object>()));
			numberFormatBuilder.Verify(m => m.BuildObject(context, (NumberFormat)template.Cells.First().Format, It.IsAny<object>()));
			constantBindBuilder.Verify(m => m.BuildObject(context, (ConstantBind)template.Cells.First().DataBind, It.IsAny<object>()));
		}

		private static TableElement CreateTemplate(Action<TableElementConfig> config)
		{
			var template = new TableElement();
			config(new TableElementConfig(template));
			return template;
		}
	}
}