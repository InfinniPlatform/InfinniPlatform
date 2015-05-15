using System.Linq;

using FastReport.Utils;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;
using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

using Moq;

using NUnit.Framework;

using FRBorder = FastReport.Border;
using FRFormat = FastReport.Format.DateFormat;
using FRTableColumn = FastReport.Table.TableColumn;
using FRTableObject = FastReport.Table.TableObject;
using FRTableRow = FastReport.Table.TableRow;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class TableElementTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildTableElement()
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

			var tableObject = new FRTableObject
								  {
									  Top = 11 * Units.Millimeters,
									  Left = 12 * Units.Millimeters,
									  Width = 600 * Units.Millimeters,
									  Height = 30 * Units.Millimeters,

									  Border = new FRBorder(),

									  Columns =
						                  {
							                  new FRTableColumn { Width = 11 * Units.Millimeters, AutoSize = false },
							                  new FRTableColumn { Width = 12 * Units.Millimeters, AutoSize = true },
							                  new FRTableColumn { Width = 13 * Units.Millimeters, AutoSize = true }
						                  },

									  Rows =
						                  {
							                  new FRTableRow { Height = 21 * Units.Millimeters, AutoSize = false },
							                  new FRTableRow { Height = 22 * Units.Millimeters, AutoSize = true }
						                  }
								  };

			tableObject[0, 0].Border = new FRBorder();
			tableObject[0, 0].Format = new FRFormat();
			tableObject[0, 0].Text = "Cell00";

			tableObject[1, 0].Border = new FRBorder();
			tableObject[1, 0].Format = new FRFormat();
			tableObject[1, 0].Text = "Cell10";

			tableObject[2, 0].Border = new FRBorder();
			tableObject[2, 0].Format = new FRFormat();
			tableObject[2, 0].Text = "Cell20";

			tableObject[0, 1].Border = new FRBorder();
			tableObject[0, 1].Format = new FRFormat();
			tableObject[0, 1].Text = "Cell01";

			tableObject[1, 1].Border = new FRBorder();
			tableObject[1, 1].Format = new FRFormat();
			tableObject[1, 1].Text = "Cell11";

			tableObject[2, 1].Border = new FRBorder();
			tableObject[2, 1].Format = new FRFormat();
			tableObject[2, 1].Text = "Cell21";

			// When
			var target = new TableElementTemplateBuilder();
			var result = target.BuildTemplate(_context, tableObject);

			// Then

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Columns);
			Assert.IsNotNull(result.Rows);
			Assert.IsNotNull(result.Cells);

			Assert.AreEqual(3, result.Columns.Count);
			Assert.AreEqual(2, result.Rows.Count);
			Assert.AreEqual(6, result.Cells.Count);

			Assert.AreEqual(0, result.Columns.ElementAt(0).Index);
			Assert.AreEqual(11, result.Columns.ElementAt(0).Width, 0.00001);
			Assert.AreEqual(false, result.Columns.ElementAt(0).AutoWidth);
			Assert.AreEqual(1, result.Columns.ElementAt(1).Index);
			Assert.AreEqual(12, result.Columns.ElementAt(1).Width, 0.00001);
			Assert.AreEqual(true, result.Columns.ElementAt(1).AutoWidth);
			Assert.AreEqual(2, result.Columns.ElementAt(2).Index);
			Assert.AreEqual(13, result.Columns.ElementAt(2).Width, 0.00001);
			Assert.AreEqual(true, result.Columns.ElementAt(2).AutoWidth);

			Assert.AreEqual(0, result.Rows.ElementAt(0).Index);
			Assert.AreEqual(21, result.Rows.ElementAt(0).Height, 0.00001);
			Assert.AreEqual(false, result.Rows.ElementAt(0).AutoHeight);
			Assert.AreEqual(1, result.Rows.ElementAt(1).Index);
			Assert.AreEqual(22, result.Rows.ElementAt(1).Height, 0.00001);
			Assert.AreEqual(true, result.Rows.ElementAt(1).AutoHeight);

			var cell00 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 0 && i.RowIndex == 0);
			Assert.IsNotNull(cell00);
			Assert.AreEqual(1, cell00.ColSpan);
			Assert.AreEqual(1, cell00.RowSpan);

			var cell10 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 1 && i.RowIndex == 0);
			Assert.IsNotNull(cell10);
			Assert.AreEqual(1, cell10.ColSpan);
			Assert.AreEqual(1, cell10.RowSpan);

			var cell20 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 2 && i.RowIndex == 0);
			Assert.IsNotNull(cell20);
			Assert.AreEqual(1, cell20.ColSpan);
			Assert.AreEqual(1, cell20.RowSpan);

			var cell01 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 0 && i.RowIndex == 1);
			Assert.IsNotNull(cell01);
			Assert.AreEqual(1, cell01.ColSpan);
			Assert.AreEqual(1, cell01.RowSpan);

			var cell11 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 1 && i.RowIndex == 1);
			Assert.IsNotNull(cell11);
			Assert.AreEqual(1, cell11.ColSpan);
			Assert.AreEqual(1, cell11.RowSpan);

			var cell21 = result.Cells.FirstOrDefault(i => i.ColumnIndex == 2 && i.RowIndex == 1);
			Assert.IsNotNull(cell21);
			Assert.AreEqual(1, cell21.ColSpan);
			Assert.AreEqual(1, cell21.RowSpan);

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject.Border));
			elementLayoutBuilder.Verify(m => m.BuildTemplate(_context, tableObject));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 0].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 0]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 0].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 0].Text));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 0].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 0]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 0].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 0].Text));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 0].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 0]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 0].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 0].Text));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 1].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 1]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 1].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[0, 1].Text));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 1].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 1]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 1].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[1, 1].Text));

			borderBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 1].Border));
			textElementStyleBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 1]));
			formatBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 1].Format));
			dataBindBuilder.Verify(m => m.BuildTemplate(_context, tableObject[2, 1].Text));
		}
	}
}