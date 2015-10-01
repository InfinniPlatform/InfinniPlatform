using System;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel
{
    internal sealed class GridPanelElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var gridPanel = new GridPanelElement(parent);
            gridPanel.ApplyElementMeatadata((object) metadata);

            var columns = (metadata.Columns != null) ? Convert.ToInt32(metadata.Columns) : 12;
            gridPanel.SetColumns(columns);

            if (metadata.Rows != null)
            {
                foreach (var rowMetadata in metadata.Rows)
                {
                    var row = gridPanel.AddRow();

                    if (rowMetadata.Cells != null)
                    {
                        foreach (var cellMetadata in rowMetadata.Cells)
                        {
                            var columnSpan = (cellMetadata.ColumnSpan != null)
                                ? Convert.ToInt32(cellMetadata.ColumnSpan)
                                : 1;
                            GridPanelCellElement cell = row.AddCell(columnSpan);

                            var items = context.BuildMany(parent, cellMetadata.Items);

                            if (items != null)
                            {
                                foreach (var item in items)
                                {
                                    cell.AddItem(item);
                                }
                            }
                        }
                    }
                }
            }

            if (parent != null && metadata.OnLoaded != null)
            {
                gridPanel.OnLoaded += parent.GetScript(metadata.OnLoaded);
            }

            return gridPanel;
        }
    }
}