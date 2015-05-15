using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel
{
	/// <summary>
	/// Контейнер элементов представления в виде стека.
	/// </summary>
	public sealed class StackPanelElement : BaseElement<StackPanelControl>, ILayoutPanel
	{
		public StackPanelElement(View view)
			: base(view)
		{
			SetOrientation(StackPanelOrientation.Vertical);
		}


		// Orientation

		private StackPanelOrientation _orientation;

		/// <summary>
		/// Возвращает ориентацию стека.
		/// </summary>
		public StackPanelOrientation GetOrientation()
		{
			return _orientation;
		}

		/// <summary>
		/// Устанавливает ориентацию стека.
		/// </summary>
		public void SetOrientation(StackPanelOrientation value)
		{
			_orientation = value;

			switch (value)
			{
				case StackPanelOrientation.Horizontal:
					Control.Orientation = Orientation.Horizontal;
					break;
				case StackPanelOrientation.Vertical:
					Control.Orientation = Orientation.Vertical;
					break;
			}
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

			Control.Children.Add(item.GetControl<UIElement>());
		}

		/// <summary>
		/// Удаляет дочерний элемент.
		/// </summary>
		public void RemoveItem(IElement item)
		{
			_items.Remove(item);

			Control.Children.Remove(item.GetControl<UIElement>());
		}

		/// <summary>
		/// Возвращает дочерний элемент по имени.
		/// </summary>
		public IElement GetItem(string name)
		{
			return _items.FirstOrDefault(i => i.GetName() == name);
		}

		/// <summary>
		/// Возвращает список дочерних элементов.
		/// </summary>
		public IEnumerable<IElement> GetItems()
		{
			return _items.AsReadOnly();
		}


		// Elements

		public override IEnumerable<IElement> GetChildElements()
		{
			return GetItems();
		}
	}
}