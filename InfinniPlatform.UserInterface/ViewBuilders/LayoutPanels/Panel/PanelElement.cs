using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.Panel
{
	/// <summary>
	/// Контейнер элементов представления в виде сворачиваемой прямоугольной области.
	/// </summary>
	public sealed class PanelElement : BaseElement<PanelControl>, ILayoutPanel
	{
		public PanelElement(View view)
			: base(view)
		{
			Control.Expanded += OnPanelExpanded;
			Control.Collapsed += OnPanelCollapsed;
		}

		private void OnPanelExpanded(object sender, EventArgs e)
		{
			this.InvokeScript(OnExpanded);
		}

		private void OnPanelCollapsed(object sender, EventArgs e)
		{
			this.InvokeScript(OnCollapsed);
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);

			Control.Text = value;
		}


		// Image

		private string _image;

		/// <summary>
		/// Возвращает изображение заголовка панели.
		/// </summary>
		public string GetImage()
		{
			return _image;
		}

		/// <summary>
		/// Устанавливает изображение заголовка панели.
		/// </summary>
		public void SetImage(string value)
		{
			_image = value;

			Control.Image = ImageRepository.GetImage(value);
		}


		// Collapsible

		/// <summary>
		/// Возвращает значение, определяющее, разрешено ли сворачивание панели.
		/// </summary>
		public bool GetCollapsible()
		{
			return Control.IsCollapsible;
		}

		/// <summary>
		/// Устанавливает значение, определяющее, разрешено ли сворачивание панели.
		/// </summary>
		public void SetCollapsible(bool value)
		{
			Control.IsCollapsible = value;
		}


		// Collapsed

		/// <summary>
		/// Возвращает значение, определяющее, свернута ли панель.
		/// </summary>
		public bool GetCollapsed()
		{
			return Control.IsCollapsed;
		}

		/// <summary>
		/// Устанавливает значение, определяющее, свернута ли панель.
		/// </summary>
		public void SetCollapsed(bool value)
		{
			Control.IsCollapsed = value;
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


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события разворачивания.
		/// </summary>
		public ScriptDelegate OnExpanded { get; set; }

		/// <summary>
		/// Возвращает или устанавливает обработчик события сворачивания.
		/// </summary>
		public ScriptDelegate OnCollapsed { get; set; }


		// Elements

		public override IEnumerable<IElement> GetChildElements()
		{
			return GetItems();
		}
	}
}