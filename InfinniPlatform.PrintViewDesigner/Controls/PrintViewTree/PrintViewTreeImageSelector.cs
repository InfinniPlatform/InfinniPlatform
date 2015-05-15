﻿using System.Windows.Media;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

using InfinniPlatform.PrintViewDesigner.Images;

namespace InfinniPlatform.PrintViewDesigner.Controls.PrintViewTree
{
	sealed class PrintViewTreeImageSelector : TreeListNodeImageSelector
	{
		private PrintViewTreeImageSelector()
		{
		}

		public override ImageSource Select(TreeListRowData rowData)
		{
			var row = rowData.Row as PrintElementNode;

			if (row != null)
			{
				return ImageRepository.GetImage(row.ElementType);
			}

			return null;
		}

		public static readonly PrintViewTreeImageSelector Instance = new PrintViewTreeImageSelector();
	}
}