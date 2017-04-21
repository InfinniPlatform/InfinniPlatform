using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Block;
using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintTableFactoryTest
    {
        [Test]
        public void ShouldBuildTable()
        {
            // Given

            var tableColumn1 = new PrintTableColumn();
            var tableColumn2 = new PrintTableColumn();
            var tableColumn3 = new PrintTableColumn();

            var tableRow1 = new PrintTableRow();
            var tableCell11 = CreateTableCellByText("11");
            var tableCell12 = CreateTableCellByText("12");
            var tableCell13 = CreateTableCellByText("13");
            tableRow1.Cells.Add(tableCell11);
            tableRow1.Cells.Add(tableCell12);
            tableRow1.Cells.Add(tableCell13);

            var tableRow2 = new PrintTableRow();
            var tableCell21 = CreateTableCellByText("21");
            var tableCell22 = CreateTableCellByText("22");
            var tableCell23 = CreateTableCellByText("23");
            tableRow2.Cells.Add(tableCell21);
            tableRow2.Cells.Add(tableCell22);
            tableRow2.Cells.Add(tableCell23);

            var tableRow3 = new PrintTableRow();
            var tableCell31 = CreateTableCellByText("31");
            var tableCell32 = CreateTableCellByText("32");
            var tableCell33 = CreateTableCellByText("33");
            tableRow3.Cells.Add(tableCell31);
            tableRow3.Cells.Add(tableCell32);
            tableRow3.Cells.Add(tableCell33);

            var template = new PrintTable { ShowHeader = false };
            template.Columns.Add(tableColumn1);
            template.Columns.Add(tableColumn2);
            template.Columns.Add(tableColumn3);
            template.Rows.Add(tableRow1);
            template.Rows.Add(tableRow2);
            template.Rows.Add(tableRow3);

            // When
            var element = BuildTestHelper.BuildElement<PrintTable>(template);

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

            object dataSource = new[]
                                {
                                    new { Property1 = "11", Property2 = "12", Property3 = "13" },
                                    new { Property1 = "21", Property2 = "22", Property3 = "23" },
                                    new { Property1 = "31", Property2 = "32", Property3 = "33" }
                                };

            var tableColumn1 = new PrintTableColumn
                               {
                                   Header = CreateTableCellByText("Header1"),
                                   CellTemplate = CreateTableCellBySource("Property1")
                               };

            var tableColumn2 = new PrintTableColumn
                               {
                                   Header = CreateTableCellByText("Header2"),
                                   CellTemplate = CreateTableCellBySource("Property2")
                               };

            var tableColumn3 = new PrintTableColumn
                               {
                                   Header = CreateTableCellByText("Header3"),
                                   CellTemplate = CreateTableCellBySource("Property3")
                               };

            var template = new PrintTable { ShowHeader = true, Source = "$" };
            template.Columns.Add(tableColumn1);
            template.Columns.Add(tableColumn2);
            template.Columns.Add(tableColumn3);

            // When
            var element = BuildTestHelper.BuildElement<PrintTable>(template, dataSource);

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

            var tableColumn1 = new PrintTableColumn { Size = 10, SizeUnit = PrintSizeUnit.Px };
            var tableColumn2 = new PrintTableColumn { Size = 20, SizeUnit = PrintSizeUnit.Px };
            var tableColumn3 = new PrintTableColumn { Size = null, SizeUnit = null };

            var template = new PrintTable { ShowHeader = false };
            template.Columns.Add(tableColumn1);
            template.Columns.Add(tableColumn2);
            template.Columns.Add(tableColumn3);

            // When
            var element = BuildTestHelper.BuildElement<PrintTable>(template, initContext: c => { c.ElementWidth = 60; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(3, element.Columns.Count);
            Assert.AreEqual(10, element.Columns[0].Size, 0.1);
            Assert.AreEqual(20, element.Columns[1].Size, 0.1);
            Assert.AreEqual(28.7, element.Columns[2].Size, 0.1);
        }


        private static PrintTableCell CreateTableCellByText(string cellText)
        {
            return CreateTableCell(cellText, null);
        }

        private static PrintTableCell CreateTableCellBySource(string cellSource)
        {
            return CreateTableCell(null, cellSource);
        }

        private static PrintTableCell CreateTableCell(string cellText, string cellSource)
        {
            return new PrintTableCell
                   {
                       Block = new PrintParagraph
                               {
                                   Inlines = new List<PrintInline>
                                             {
                                                 new PrintRun
                                                 {
                                                     Text = cellText,
                                                     Source = cellSource
                                                 }
                                             }
                               }
                   };
        }


        private static void AssertTableCell(PrintTable table, int row, int column, string text)
        {
            Assert.AreEqual(text, ((PrintRun)((PrintParagraph)table.Rows[row].Cells[column].Block).Inlines.First()).Text);
        }
    }
}