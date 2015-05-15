using System.Windows.Media;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

using InfinniPlatform.UserInterface.ViewBuilders.Images;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView
{
	sealed class ImagePropertySelector : TreeListNodeImageSelector
	{
		private ImagePropertySelector()
		{
		}

		public override ImageSource Select(TreeListRowData rowData)
		{
			var row = rowData.Row as TreeViewDataRow;

			if (row != null)
			{
				return ImageRepository.GetImage(row.Image as string);
			}

			return null;
		}

		public static readonly TreeListNodeImageSelector Instance = new ImagePropertySelector();
	}
}