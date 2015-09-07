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
            var result = new TextWriterWrapper();

            var text1 = new PrintElementRun { Text = "Text11, colspan = 2" };
            var cell1 = new PrintElementTableCell
            {
                ColumnSpan = 2,
                Border = new PrintElementBorder()
                {
                    Thickness = new PrintElementThickness(1),
                    Color = "black"
                }
            };
            var par1 = new PrintElementParagraph();
            par1.Inlines.Add(text1);
            cell1.Block = par1;
            var row1 = new PrintElementTableRow();
            row1.Cells.Add(cell1);

            var text21 = new PrintElementRun { Text = "Text21" };
            var cell21 = new PrintElementTableCell
            {
                Border = new PrintElementBorder()
                    {
                        Thickness = new PrintElementThickness(1),
                        Color = "black"
                    }
            };
            var par21 = new PrintElementParagraph();
            par21.Inlines.Add(text21);
            cell21.Block = par21;
            var row2 = new PrintElementTableRow();
            row2.Cells.Add(cell21);

            var text22 = new PrintElementRun { Text = "Text22" };
            var cell22 = new PrintElementTableCell
            {
                Border = new PrintElementBorder()
                    {
                        Thickness = new PrintElementThickness(1),
                        Color = "black"
                    }
            };
            var par22 = new PrintElementParagraph();
            par22.Inlines.Add(text22);
            cell22.Block = par22;
            row2.Cells.Add(cell22);

            var column1 = new PrintElementTableColumn { Size = 100 };
            var column2 = new PrintElementTableColumn { Size = 200 };

            element.Columns.Add(column1);
            element.Columns.Add(column2);

            element.Rows.Add(row1);
            element.Rows.Add(row2);

            //When
            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildTable, result.GetText());
        }
    }
}
