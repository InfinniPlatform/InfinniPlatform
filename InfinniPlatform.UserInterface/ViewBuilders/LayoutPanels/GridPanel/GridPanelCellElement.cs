using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel
{
	/// <summary>
	/// Ячейка сетки.
	/// </summary>
	public sealed class GridPanelCellElement
	{
		public GridPanelCellElement(Grid grid, int rowIndex, int columnIndex, int columnSpan)
		{
			_grid = grid;
			_rowIndex = rowIndex;
			_columnIndex = columnIndex;
			_columnSpan = columnSpan;
		}


		private readonly Grid _grid;
		private readonly int _rowIndex;
		private readonly int _columnIndex;
		private readonly int _columnSpan;


		// ColSpan

		/// <summary>
		/// Возвращает размер ячейки в колонках.
		/// </summary>
		public int GetColumnSpan()
		{
			return _columnSpan;
		}


		// Items

		private readonly List<IElement> _items
			= new List<IElement>();

		/// <summary>
		/// Добавляет дочерний элемент.
		/// </summary>
		public void AddItem(IElement item)
		{
			_items.Add(item);

			var itemControl = item.GetControl<UIElement>();

			_grid.Children.Add(itemControl);

			Grid.SetRow(itemControl, _rowIndex);
			Grid.SetColumn(itemControl, _columnIndex);
			Grid.SetColumnSpan(itemControl, _columnSpan);
		}

		/// <summary>
		/// Удаляет дочерний элемент.
		/// </summary>
		public void RemoveItem(IElement item)
		{
			if (_items.Remove(item))
			{
				var itemControl = item.GetControl<UIElement>();

				_grid.Children.Remove(itemControl);
			}
		}

		/// <summary>
		/// Возвращает список дочерних элементов.
		/// </summary>
		public IEnumerable<IElement> GetItems()
		{
			return _items.AsReadOnly();
		}
	}
}