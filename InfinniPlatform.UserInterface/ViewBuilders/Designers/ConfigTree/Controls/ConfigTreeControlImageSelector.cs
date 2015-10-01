using System.Windows.Media;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using InfinniPlatform.UserInterface.ViewBuilders.Images;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
    internal sealed class ConfigTreeControlImageSelector : TreeListNodeImageSelector
    {
        public static readonly ConfigTreeControlImageSelector Instance = new ConfigTreeControlImageSelector();

        private ConfigTreeControlImageSelector()
        {
        }

        public override ImageSource Select(TreeListRowData rowData)
        {
            ImageSource result = null;

            var row = rowData.Row as ConfigElementNode;

            if (row != null)
            {
                if (!string.IsNullOrEmpty(row.ElementType))
                {
                    result = ImageRepository.GetImage("System/" + row.ElementType);
                }
            }

            if (result == null)
            {
                result = ImageRepository.GetImage(rowData.Node.IsExpanded ? "System/FolderOpen" : "System/FolderClosed");
            }

            return result;
        }
    }
}