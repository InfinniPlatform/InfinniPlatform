using System.Collections.Generic;
using System.Windows.Controls;

using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Layout.Core;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.DockTabPanel
{
	public sealed partial class DockTabPanelControl : UserControl
	{
		public DockTabPanelControl()
		{
			InitializeComponent();
		}


		// Events

		public event DockItemActivatedEventHandler SelectedPageChanged
		{
			add { TabContainer.AddHandler(DockLayoutManager.DockItemActivatedEvent, value); }
			remove { TabContainer.RemoveHandler(DockLayoutManager.DockItemActivatedEvent, value); }
		}

		public event DockItemCancelEventHandler PageClosing
		{
			add { TabContainer.AddHandler(DockLayoutManager.DockItemClosingEvent, value); }
			remove { TabContainer.RemoveHandler(DockLayoutManager.DockItemClosingEvent, value); }
		}


		// HeaderLocation

		private CaptionLocation _headerLocation;

		public CaptionLocation HeaderLocation
		{
			get
			{
				return _headerLocation;
			}
			set
			{
				if (!Equals(_headerLocation, value))
				{
					_headerLocation = value;

					if (TabContainer.LayoutRoot.Items.Count == 1)
					{
						var group = TabContainer.LayoutRoot.Items[0] as DocumentGroup;

						if (group != null)
						{
							group.CaptionLocation = value;
						}
					}
				}
			}
		}


		// HeaderOrientation

		private Orientation _headerOrientation;

		public Orientation HeaderOrientation
		{
			get
			{
				return _headerOrientation;
			}
			set
			{
				if (!Equals(_headerOrientation, value))
				{
					_headerOrientation = value;

					if (TabContainer.LayoutRoot.Items.Count == 1)
					{
						var group = TabContainer.LayoutRoot.Items[0] as DocumentGroup;

						if (group != null)
						{
							group.CaptionOrientation = value;
						}
					}
				}
			}
		}


		// SelectedPage

		public BaseLayoutItem SelectedPage
		{
			get
			{
				BaseLayoutItem selectedPage = null;

				if (TabContainer.LayoutRoot.Items.Count == 1)
				{
					var group = TabContainer.LayoutRoot.Items[0] as DocumentGroup;

					if (group != null)
					{
						selectedPage = group.SelectedItem;
					}
				}

				if (selectedPage == null)
				{
					selectedPage = TabContainer.ActiveDockItem;
				}

				if (!_pages.Contains(selectedPage))
				{
					selectedPage = null;
				}

				return selectedPage;
			}
			set
			{
				TabContainer.ActiveDockItem = value;
			}
		}


		// Pages

		private readonly List<BaseLayoutItem> _pages
			= new List<BaseLayoutItem>();

		public void AddPage(BaseLayoutItem page)
		{
			if (!_pages.Contains(page))
			{
				page.AllowHide = false;

				if (TabContainer.LayoutRoot.Items.Count <= 0)
				{
					TabContainer.LayoutRoot.Items.Add(new DocumentGroup
													  {
														  MDIStyle = MDIStyle.Tabbed,
														  ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders,
														  CaptionLocation = _headerLocation,
														  CaptionOrientation = _headerOrientation,
													  });
				}

				_pages.Add(page);

				TabContainer.DockController.AddItem(page, TabContainer.LayoutRoot.Items[0], DockType.Fill);
			}
		}

		public void RemovePage(BaseLayoutItem page)
		{
			if (_pages.Remove(page))
			{
				TabContainer.DockController.RemoveItem(page);
			}
		}

		public IEnumerable<BaseLayoutItem> GetPages()
		{
			return _pages.AsReadOnly();
		}
	}
}