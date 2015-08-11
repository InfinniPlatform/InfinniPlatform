using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementTableBuilder : IFlowElementBuilderBase<PrintElementTable>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementTable element)
        {
            var elementContent = new Table
            {
                CellSpacing = 0,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 0, 0),
                RowGroups = { new TableRowGroup() }
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            foreach (var column in element.Columns)
            {
                var columnContent = new TableColumn { Width = new GridLength(column.Size ?? 1) };

                elementContent.Columns.Add(columnContent);
            }

            foreach (var row in element.Rows)
            {
                var rowContent = new TableRow();

                FlowElementBuilderHelper.ApplyRowStyles(rowContent, row);

                foreach (var cell in row.Cells)
                {
                    var cellContent = new TableCell
                    {
                        ColumnSpan = 1,
                        RowSpan = 1,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0, 1, 1)
                    };

                    //Check ColSpan and RowSpan

                    var colIndex = row.Cells.IndexOf(cell);

                    if (cell.ColumnSpan != null && cell.ColumnSpan > (element.Columns.Count - colIndex))
                    {
                        cell.ColumnSpan = element.Columns.Count - colIndex;
                    }

                    FlowElementBuilderHelper.ApplyCellStyles(cellContent, cell);

                    var blockContent = context.Build<Block>(cell.Block);

                    if (blockContent != null)
                    {
                        cellContent.Blocks.Add(blockContent);
                    }

                    rowContent.Cells.Add(cellContent);
                }

                elementContent.RowGroups[0].Rows.Add(rowContent);
            }

            return elementContent;
        }
    }
}
