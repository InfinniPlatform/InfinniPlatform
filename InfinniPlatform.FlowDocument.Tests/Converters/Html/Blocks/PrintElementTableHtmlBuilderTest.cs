using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Converters.Html.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementTableHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildTable()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementTable();
            var run = new PrintElementRun();
            var par = new PrintElementParagraph();
            var cell = new PrintElementTableCell();
            var row = new PrintElementTableRow();
            var result = new TextWriterWrapper();

            run.Text = "Text";
            par.Inlines.Add(run);
            cell.Block = par;
            cell.Border = new PrintElementBorder
            {
                Thickness = new PrintElementThickness(1),
                Color = "black"
            };
            cell.Padding = new PrintElementThickness(5);

            row.Cells.Add(cell);
            row.Cells.Add(cell);
            row.Cells.Add(cell);

            element.Rows.Add(row);
            element.Rows.Add(row);

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildTable, result.GetText());
        }

        [Test]
        public void ShouldBuildTableWithColRowSpan()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementTable();
            var result = new TextWriterWrapper();

            var cells = new List<PrintElementTableCell>();

            for (var i = 0; i < 16; i++)
            {
                cells.Add(new PrintElementTableCell());
            }

            cells[0].ColumnSpan = 2;
            cells[0].RowSpan = 2;

            cells[10].ColumnSpan = 2;

            for (var i = 0; i < 16; i++)
            {
                cells[i].Border = new PrintElementBorder
                {
                    Thickness = new PrintElementThickness(1),
                    Color = "black"
                };
                cells[i].Padding = new PrintElementThickness(50);
            }

            var rows = new PrintElementTableRow[4];

            for (var i = 0; i < 4; i++)
            {
                rows[i] = new PrintElementTableRow();

                for (var j = i * 4; j < i * 4 + 4; j++)
                {
                    rows[i].Cells.Add(cells[j]);
                }

                element.Rows.Add(rows[i]);
            }

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildTableWithColRowSpan, result.GetText());
        }
    }
}
