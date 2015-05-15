using System;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Xpf.Core;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.TabPanel
{
	/// <summary>
	/// Контейнер элементов представления в виде набора закладок для страниц.
	/// </summary>
	public sealed class TabPanelElement : BaseElement<DXTabControl>, ITabPanel
	{
		public TabPanelElement(View view)
			: base(view)
		{
			Control.View = new TabControlScrollView
						   {
							   AllowHideTabItems = true,
							   RemoveTabItemsOnHiding = true
						   };

			SetHeaderLocation(TabHeaderLocation.Top);
			SetHeaderOrientation(TabHeaderOrientation.Horizontal);

			Control.SelectionChanged += OnSelectionChangedTabPanel;
		}

		private void OnSelectionChangedTabPanel(object sender, TabControlSelectionChangedEventArgs e)
		{
			this.InvokeScript(OnSelectionChanged);
		}


		// HeaderLocation

		private TabHeaderLocation _headerLocation;

		/// <summary>
		/// Возвращает расположение закладок.
		/// </summary>
		public TabHeaderLocation GetHeaderLocation()
		{
			return _headerLocation;
		}

		/// <summary>
		/// Устанавливает расположение закладок.
		/// </summary>
		public void SetHeaderLocation(TabHeaderLocation value)
		{
			_headerLocation = value;

			var tabView = Control.View;

			if (tabView != null)
			{
				switch (value)
				{
					case TabHeaderLocation.None:
						tabView.HeaderLocation = HeaderLocation.None;
						break;
					case TabHeaderLocation.Left:
						tabView.HeaderLocation = HeaderLocation.Left;
						break;
					case TabHeaderLocation.Top:
						tabView.HeaderLocation = HeaderLocation.Top;
						break;
					case TabHeaderLocation.Right:
						tabView.HeaderLocation = HeaderLocation.Right;
						break;
					case TabHeaderLocation.Bottom:
						tabView.HeaderLocation = HeaderLocation.Bottom;
						break;
				}
			}
		}


		// HeaderOrientation

		private TabHeaderOrientation _headerOrientation;

		/// <summary>
		/// Возвращает ориентацию закладок.
		/// </summary>
		public TabHeaderOrientation GetHeaderOrientation()
		{
			return _headerOrientation;
		}

		/// <summary>
		/// Устанавливает ориентацию закладок.
		/// </summary>
		public void SetHeaderOrientation(TabHeaderOrientation value)
		{
			_headerOrientation = value;

			var tabView = Control.View as TabControlScrollView;

			if (tabView != null)
			{
				switch (value)
				{
					case TabHeaderOrientation.Horizontal:
						tabView.HeaderOrientation = HeaderOrientation.Horizontal;
						break;
					case TabHeaderOrientation.Vertical:
						tabView.HeaderOrientation = HeaderOrientation.Vertical;
						break;
				}
			}
		}


		// SelectedPage

		/// <summary>
		/// Возвращает выделенную страницу.
		/// </summary>
		public ITabPage GetSelectedPage()
		{
			var tabPage = Control.SelectedTabItem;

			if (tabPage != null)
			{
				return (ITabPage)tabPage.Tag;
			}

			return null;
		}

		/// <summary>
		/// Устанавливает выделенную страницу.
		/// </summary>
		public void SetSelectedPage(ITabPage page)
		{
			var tabPage = page.GetControl<DXTabItem>();

			if (tabPage != null)
			{
				Control.Dispatcher.BeginInvoke((Action)(() =>
														{
															Control.SelectedItem = tabPage;
															tabPage.Focus();
														}));
			}
		}


		// Pages

		/// <summary>
		/// Создает страницу.
		/// </summary>
		public ITabPage CreatePage(View view)
		{
			return new TabPageElement(view);
		}

		/// <summary>
		/// Добавляет указанную страницу.
		/// </summary>
		public void AddPage(ITabPage page)
		{
			var tabPage = page.GetControl<DXTabItem>();
			tabPage.Tag = page;

			Control.Items.Add(tabPage);
			page.SetParent(this);
		}

		/// <summary>
		/// Удаляет указанную страницу.
		/// </summary>
		public void RemovePage(ITabPage page)
		{
			var tabPage = page.GetControl<DXTabItem>();

			Control.Items.Remove(tabPage);
			page.SetParent(null);
		}

		/// <summary>
		/// Возвращает страницу с указанным именем.
		/// </summary>
		public ITabPage GetPage(string name)
		{
			return GetPages()
				.FirstOrDefault(p => p.GetName() == name);
		}

		/// <summary>
		/// Возвращает список страниц.
		/// </summary>
		public IEnumerable<ITabPage> GetPages()
		{
			return Control.Items
				.Cast<DXTabItem>()
				.Select(p => (ITabPage)p.Tag);
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения выделенной страницы.
		/// </summary>
		public ScriptDelegate OnSelectionChanged { get; set; }


		// Elements

		public override IEnumerable<IElement> GetChildElements()
		{
			return GetPages();
		}
	}
}