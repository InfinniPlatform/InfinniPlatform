using System.Linq;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementTableFactoryTest
    {
        [Test]
        public void ShouldBuildTable()
        {
            // Given

            dynamic tableColumn1 = new DynamicWrapper();
            dynamic tableColumn2 = new DynamicWrapper();
            dynamic tableColumn3 = new DynamicWrapper();

            dynamic tableRow1 = new DynamicWrapper();
            dynamic tableCell11 = CreateTableCellByText("11");
            dynamic tableCell12 = CreateTableCellByText("12");
            dynamic tableCell13 = CreateTableCellByText("13");
            tableRow1.Cells = new[] {tableCell11, tableCell12, tableCell13};

            dynamic tableRow2 = new DynamicWrapper();
            dynamic tableCell21 = CreateTableCellByText("21");
            dynamic tableCell22 = CreateTableCellByText("22");
            dynamic tableCell23 = CreateTableCellByText("23");
            tableRow2.Cells = new[] {tableCell21, tableCell22, tableCell23};

            dynamic tableRow3 = new DynamicWrapper();
            dynamic tableCell31 = CreateTableCellByText("31");
            dynamic tableCell32 = CreateTableCellByText("32");
            dynamic tableCell33 = CreateTableCellByText("33");
            tableRow3.Cells = new[] {tableCell31, tableCell32, tableCell33};

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.ShowHeader = false;
            elementMetadata.Columns = new[] {tableColumn1, tableColumn2, tableColumn3};
            elementMetadata.Rows = new[] {tableRow1, tableRow2, tableRow3};

            // When
            PrintElementTable element = BuildTestHelper.BuildTable(elementMetadata);

            // Then

            Assert.IsNotNull(element);

            Assert.AreEqual(3, element.Columns.Count);
            Assert.AreEqual(3, element.Rows.Count);

            AssertTableCell(element, 0, 0, "11");
            AssertTableCell(element, 0, 1, "12");
            AssertTableCell(element, 0, 2, "13");

            AssertTableCell(element, 1, 0, "21");
            AssertTableCell(element, 1, 1, "22");
            AssertTableCell(element, 1, 2, "23");

            AssertTableCell(element, 2, 0, "31");
            AssertTableCell(element, 2, 1, "32");
            AssertTableCell(element, 2, 2, "33");
        }

        [Test]
        public void ShouldBuildTableFromSource()
        {
            // Given

            object source = new[]
            {
                new {Property1 = "11", Property2 = "12", Property3 = "13"},
                new {Property1 = "21", Property2 = "22", Property3 = "23"},
                new {Property1 = "31", Property2 = "32", Property3 = "33"}
            };

            dynamic tableColumn1 = new DynamicWrapper();
            tableColumn1.Header = CreateTableCellByText("Header1");
            tableColumn1.CellTemplate = CreateTableCellBySource("Property1");

            dynamic tableColumn2 = new DynamicWrapper();
            tableColumn2.Header = CreateTableCellByText("Header2");
            tableColumn2.CellTemplate = CreateTableCellBySource("Property2");

            dynamic tableColumn3 = new DynamicWrapper();
            tableColumn3.Header = CreateTableCellByText("Header3");
            tableColumn3.CellTemplate = CreateTableCellBySource("Property3");

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Columns = new[] {tableColumn1, tableColumn2, tableColumn3};
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildTable((object) elementMetadata, c => { c.PrintViewSource = source; });

            // Then

            Assert.IsNotNull(element);
            Assert.AreEqual(3, element.Columns.Count);
            Assert.AreEqual(4, element.Rows.Count);

            AssertTableCell(element, 0, 0, "Header1");
            AssertTableCell(element, 0, 1, "Header2");
            AssertTableCell(element, 0, 2, "Header3");

            AssertTableCell(element, 1, 0, "11");
            AssertTableCell(element, 1, 1, "12");
            AssertTableCell(element, 1, 2, "13");

            AssertTableCell(element, 2, 0, "21");
            AssertTableCell(element, 2, 1, "22");
            AssertTableCell(element, 2, 2, "23");

            AssertTableCell(element, 3, 0, "31");
            AssertTableCell(element, 3, 1, "32");
            AssertTableCell(element, 3, 2, "33");
        }

        [Test]
        public void ShouldCalcColumnWidth()
        {
            // Given

            dynamic tableColumn1 = new DynamicWrapper();
            tableColumn1.Size = "10";
            tableColumn1.SizeUnit = "Px";

            dynamic tableColumn2 = new DynamicWrapper();
            tableColumn2.Size = "20";
            tableColumn2.SizeUnit = "Px";

            dynamic tableColumn3 = new DynamicWrapper();
            tableColumn3.Size = null;
            tableColumn3.SizeUnit = null;

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.ShowHeader = false;
            elementMetadata.Columns = new[] {tableColumn1, tableColumn2, tableColumn3};

            // When
            var element = BuildTestHelper.BuildTable((object) elementMetadata, c => { c.ElementWidth = 60; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(3, element.Columns.Count);
            Assert.AreEqual(10, element.Columns[0].Size);
            Assert.AreEqual(20, element.Columns[1].Size);
            Assert.AreEqual(29, element.Columns[2].Size);
        }

        private static dynamic CreateTableCellByText(string cellText)
        {
            return CreateTableCell(cellText, null);
        }

        private static dynamic CreateTableCellBySource(string cellSource)
        {
            return CreateTableCell(null, cellSource);
        }

        private static dynamic CreateTableCell(string cellText, string cellSource)
        {
            dynamic cellRun = new DynamicWrapper();
            cellRun.Run = new DynamicWrapper();
            cellRun.Run.Text = cellText;
            cellRun.Run.Source = cellSource;

            dynamic cellParagraph = new DynamicWrapper();
            cellParagraph.Paragraph = new DynamicWrapper();
            cellParagraph.Paragraph.Inlines = new[] {cellRun};

            dynamic cell = new DynamicWrapper();
            cell.Block = cellParagraph;

            return cell;
        }

        private static void AssertTableCell(PrintElementTable table, int row, int column, string text)
        {
            Assert.AreEqual(text,
                ((PrintElementRun) ((PrintElementParagraph) table.Rows[row].Cells[column].Block).Inlines.First()).Text);
        }
    }
}