using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel
{
	/// <summary>
	/// Контейнер элементов представления в виде сетки.
	/// </summary>
	public sealed class GridPanelElement : BaseElement<Grid>, ILayoutPanel
	{
		public GridPanelElement(View view)
			: base(view)
		{
		}


		// Columns

		private int _columns;

		/// <summary>
		/// Возвращает количество колонок.
		/// </summary>
		public int GetColumns()
		{
			return _columns;
		}

		/// <summary>
		/// Устанавливает количество колонок.
		/// </summary>
		public void SetColumns(int value)
		{
			if (_columns > value)
			{
				throw new ArgumentException(Resources.GridPanelCannotDeleteColumns);
			}

			if (_columns != value)
			{
				var count = value - _columns;

				for (var i = 0; i < count; ++i)
				{
					Control.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
				}

				_columns = value;
			}
		}


		// Rows

		private readonly List<GridPanelRowElement> _rows
			= new List<GridPanelRowElement>();

		/// <summary>
		/// Добавляет строку.
		/// </summary>
		public GridPanelRowElement AddRow()
		{
			var row = new GridPanelRowElement(Control, _rows.Count);

			_rows.Add(row);

			Control.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

			return row;
		}

		/// <summary>
		/// Возвращает список строк.
		/// </summary>
		public IEnumerable<GridPanelRowElement> GetRows()
		{
			return _rows.AsReadOnly();
		}


		// Elements

		public override IEnumerable<IElement> GetChildElements()
		{
			var result = new List<IElement>();

			foreach (var row in _rows)
			{
				foreach (var cell in row.GetCells())
				{
					result.AddRange(cell.GetItems());
				}
			}

			return result;
		}
	}
}