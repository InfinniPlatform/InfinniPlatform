using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDesigner
{
	/// <summary>
	/// Элемент представления для редактирования дерева конфигурации.
	/// </summary>
	sealed class ConfigDesignerElement : BaseElement<UserControl>
	{
		public ConfigDesignerElement(View view)
			: base(view)
		{
			var mainPanel = new StackPanelElement(view);

			// TreeView
			var itemsTreeView = new TreeViewElement(view);
			itemsTreeView.SetKeyProperty("Key");
			itemsTreeView.SetParentProperty("Parent");
			itemsTreeView.SetDisplayProperty("Text");
			itemsTreeView.SetImageProperty("Image");
			itemsTreeView.SetShowNodeImages(true);
			itemsTreeView.OnDoubleClick += OnEditItemHandler;
			mainPanel.AddItem(itemsTreeView);

			// ToolBar
			var toolBar = new ToolBarElement(view);
			mainPanel.AddItem(toolBar);

			// ContextMenu
			var contextMenu = new ContextMenuElement(view);
			itemsTreeView.SetContextMenu(contextMenu);

			// Update

			var updateItemsButton = new ToolBarButtonItem(view);
			updateItemsButton.SetText(Resources.DocumentDesignerRefreshButton);
			updateItemsButton.SetImage("Actions/Refresh_16x16");
			updateItemsButton.SetHotkey("F5");
			updateItemsButton.OnClick += OnUpdateItemsHandler;
			toolBar.AddItem(updateItemsButton);

			var updateItemsMenuButton = new ContextMenuItem(view);
			updateItemsMenuButton.SetText(Resources.DocumentDesignerRefreshButton);
			updateItemsMenuButton.SetImage("Actions/Refresh_16x16");
			updateItemsMenuButton.SetHotkey("F5");
			updateItemsMenuButton.OnClick += OnUpdateItemsHandler;
			contextMenu.AddItem(updateItemsMenuButton);

			// Separator

			var separator = new ToolBarSeparatorItem(view);
			toolBar.AddItem(separator);

			var menuSeparator = new ContextMenuItemSeparator(view);
			contextMenu.AddItem(menuSeparator);

			// Add

			var addItemButton = new ToolBarPopupButtonItem(view);
			addItemButton.SetText(Resources.DocumentDesignerAddButton);
			addItemButton.SetImage("Actions/Add_16x16");
			addItemButton.SetHotkey("Ctrl+N");
			toolBar.AddItem(addItemButton);

			var addItemMenuButton = new ContextMenuItem(view);
			addItemMenuButton.SetText(Resources.DocumentDesignerAddButton);
			addItemMenuButton.SetImage("Actions/Add_16x16");
			addItemMenuButton.SetHotkey("Ctrl+N");
			contextMenu.AddItem(addItemMenuButton);

			// Edit

			var editItemButton = new ToolBarButtonItem(view);
			editItemButton.SetText(Resources.DocumentDesignerEditButton);
			editItemButton.SetImage("Actions/Edit_16x16");
			editItemButton.SetHotkey("Ctrl+O");
			editItemButton.OnClick += OnEditItemHandler;
			toolBar.AddItem(editItemButton);

			var editItemMenuButton = new ContextMenuItem(view);
			editItemMenuButton.SetText(Resources.DocumentDesignerEditButton);
			editItemMenuButton.SetImage("Actions/Edit_16x16");
			editItemMenuButton.SetHotkey("Ctrl+O");
			editItemMenuButton.OnClick += OnEditItemHandler;
			contextMenu.AddItem(editItemMenuButton);

			// Delete

			var deleteItemButton = new ToolBarButtonItem(view);
			deleteItemButton.SetText(Resources.DocumentDesignerDeleteButton);
			deleteItemButton.SetImage("Actions/Delete_16x16");
			deleteItemButton.SetHotkey("Ctrl+Delete");
			deleteItemButton.OnClick += OnDeleteItemHandler;
			toolBar.AddItem(deleteItemButton);

			var deleteItemMenuButton = new ContextMenuItem(view);
			deleteItemMenuButton.SetText(Resources.DocumentDesignerDeleteButton);
			deleteItemMenuButton.SetImage("Actions/Delete_16x16");
			deleteItemMenuButton.SetHotkey("Ctrl+Delete");
			deleteItemMenuButton.OnClick += OnDeleteItemHandler;
			contextMenu.AddItem(deleteItemMenuButton);

			_itemsTreeView = itemsTreeView;
			_addItemButton = addItemButton;
			_addItemMenuButton = addItemMenuButton;

			Control.Content = mainPanel.GetControl();
		}


		private readonly TreeViewElement _itemsTreeView;
		private readonly ToolBarPopupButtonItem _addItemButton;
		private readonly ContextMenuItem _addItemMenuButton;


		// Handlers

		private void OnUpdateItemsHandler(dynamic context, dynamic arguments)
		{
			InvokeUpdateItems();
		}

		private void OnAddItemHandler(string metadataType)
		{
			dynamic treeItem = _itemsTreeView.GetSelectedItem();

			if (treeItem != null)
			{
				EditItem(null, treeItem.ConfigId, metadataType);
			}
		}

		private void OnEditItemHandler(dynamic context, dynamic arguments)
		{
			dynamic treeItem = _itemsTreeView.GetSelectedItem();

			if (treeItem != null)
			{
				EditItem(treeItem.ItemId, treeItem.ConfigId, treeItem.MetadataType);
			}
		}

		private void OnDeleteItemHandler(dynamic context, dynamic arguments)
		{
			dynamic treeItem = _itemsTreeView.GetSelectedItem();

			if (treeItem != null)
			{
				DeleteItem(treeItem.ItemId, treeItem.Text, treeItem.ConfigId, treeItem.MetadataType);
			}
		}

		private void EditItem(string itemId, string configId, string metadataType)
		{
			if (string.IsNullOrEmpty(configId) == false && string.IsNullOrEmpty(metadataType) == false)
			{
				ViewHelper.ShowView(GetChildViewKey(itemId, configId),
									() => FindLinkView(metadataType),
									childDataSource => OnInitializeChildView(childDataSource, itemId, configId),
									childDataSource => InvokeUpdateItems());
			}
		}

		private static string GetChildViewKey(string itemId, string configId)
		{
			return string.IsNullOrEmpty(itemId) ? null : (configId + itemId);
		}

		private static void OnInitializeChildView(IDataSource childDataSource, string itemId, string configId)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.SetConfigId(configId);
			childDataSource.SetIdFilter(itemId);
			childDataSource.ResumeUpdate();
		}

		private void DeleteItem(string itemId, string itemText, string configId, string metadataType)
		{
			if (string.IsNullOrEmpty(itemId) == false
				&& string.IsNullOrEmpty(configId) == false
				&& MessageBox.Show(string.Format(Resources.DocumentDesignerDeleteQuestion, itemText), GetView().GetText(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				var dataProvider = new MetadataProvider(metadataType);
				dataProvider.SetConfigId(configId);
				dataProvider.DeleteItem(itemId);

				InvokeUpdateItems();
			}
		}

		private LinkView FindLinkView(string metadataType)
		{
			var editors = GetEditors();

			if (editors != null)
			{
				var editor = editors.FirstOrDefault(i => i.MetadataType == metadataType);

				if (editor != null)
				{
					return editor.LinkView;
				}
			}

			return null;
		}

		private void InvokeUpdateItems()
		{
			this.InvokeScript(OnUpdateItems);
		}


		// Editors

		private IEnumerable<ItemEditor> _editors;

		/// <summary>
		/// Возвращает список редакторов элементов конфигурации.
		/// </summary>
		public IEnumerable<ItemEditor> GetEditors()
		{
			return _editors;
		}

		/// <summary>
		/// Устанавливает список редакторов элементов конфигурации.
		/// </summary>
		public void SetEditors(IEnumerable<ItemEditor> value)
		{
			if (Equals(_editors, value) == false)
			{
				_editors = value;

				RefreshAddItemButton();
				RefreshItemsTreeView();
			}
		}

		private void RefreshAddItemButton()
		{
			var addButtons = _addItemButton.GetItems();

			if (addButtons != null)
			{
				foreach (var addButton in addButtons.ToArray())
				{
					_addItemButton.RemoveItem(addButton);
				}
			}

			var addMenuButtons = _addItemMenuButton.GetItems();

			if (addMenuButtons != null)
			{
				foreach (var addButton in addMenuButtons.ToArray())
				{
					_addItemMenuButton.RemoveItem(addButton);
				}
			}

			var editors = GetEditors();

			if (editors != null)
			{
				foreach (var editor in editors)
				{
					var itemEditor = editor;

					var addButton = new ToolBarButtonItem(GetView());
					addButton.SetText(itemEditor.Text);
					addButton.SetImage(itemEditor.Image);
					addButton.OnClick += (c, a) => OnAddItemHandler(itemEditor.MetadataType);

					var addMenuButton = new ContextMenuItem(GetView());
					addMenuButton.SetText(itemEditor.Text);
					addMenuButton.SetImage(itemEditor.Image);
					addMenuButton.OnClick += (c, a) => OnAddItemHandler(itemEditor.MetadataType);

					_addItemButton.AddItem(addButton);
					_addItemMenuButton.AddItem(addMenuButton);
				}
			}
		}


		// Items

		private IEnumerable _items;

		/// <summary>
		/// Возвращает список конфигураций.
		/// </summary>
		public IEnumerable GetItems()
		{
			return _items;
		}

		/// <summary>
		/// Устанавливает список конфигураций.
		/// </summary>
		public void SetItems(IEnumerable value)
		{
			if (Equals(_items, value) == false)
			{
				_items = value;

				RefreshItemsTreeView();
			}
		}

		private void RefreshItemsTreeView()
		{
			var items = GetItems();
			var treeItems = new List<object>();

			if (items != null)
			{
				var key = 0;
				var editors = GetEditors();

				foreach (dynamic item in items)
				{
					dynamic treeItem = new DynamicWrapper();
					treeItem.Key = ++key;
					treeItem.Text = FormatItemText(item);
					treeItem.Image = "System/Configuration_16x16";
					treeItem.ConfigId = item.Name;
					treeItems.Add(treeItem);

					if (editors != null)
					{
						foreach (var editor in editors)
						{
							FillChildItems(ref key, treeItem.Key, item, editor, treeItems);
						}
					}
				}
			}

			var rootItem = treeItems.FirstOrDefault();
			_itemsTreeView.SetItems(treeItems);
			_itemsTreeView.SetSelectedItem(rootItem);
			_itemsTreeView.ExpandItem(rootItem);
		}

		private static void FillChildItems(ref int key, int? parent, dynamic configuration, ItemEditor editor, ICollection<object> treeItems)
		{
			// Контейнер дочерних элементов
			dynamic container = new DynamicWrapper();
			container.Key = ++key;
			container.Parent = parent;
			container.Text = editor.Container;
			container.Image = editor.Image;
			container.ConfigId = configuration.Name;
			treeItems.Add(container);

			var childItems = configuration[editor.Container] as IEnumerable;

			if (childItems != null)
			{
				var orderedChildItems = childItems.Cast<dynamic>().OrderBy(i => i.Name);

				// Список дочерних элементов контейнера
				foreach (var childItem in orderedChildItems)
				{
					dynamic treeItem = new DynamicWrapper();
					treeItem.Key = ++key;
					treeItem.Parent = container.Key;
					treeItem.Text = FormatItemText(childItem);
					treeItem.ItemId = childItem.Name;
					treeItem.ConfigId = configuration.Name;
					treeItem.MetadataType = editor.MetadataType;
					treeItems.Add(treeItem);
				}
			}
		}

		private static string FormatItemText(dynamic item)
		{
			if (string.IsNullOrEmpty(item.Name))
			{
				return item.Caption;
			}

			if (string.IsNullOrEmpty(item.Caption))
			{
				return item.Name;
			}

			return string.Format("{0} ({1})", item.Name, item.Caption);
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события обновления списка.
		/// </summary>
		public ScriptDelegate OnUpdateItems { get; set; }
	}
}