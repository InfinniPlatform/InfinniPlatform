using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.MenuDesigner
{
    /// <summary>
    ///     Элемент представления для редактирования меню конфигурации.
    /// </summary>
    internal sealed class MenuDesignerElement : BaseElement<UserControl>
    {
        private dynamic _clipboardNode;
        // Cut, Copy, Paste Handlers

        private bool _copyMode;
        // Editor

        private LinkView _editor;
        private readonly dynamic _rootNode;
        private readonly TreeViewElement _treeView;

        public MenuDesignerElement(View view)
            : base(view)
        {
            var mainPanel = new StackPanelElement(view);

            // TreeView
            var treeView = new TreeViewElement(view);
            treeView.SetKeyProperty("Key");
            treeView.SetParentProperty("Parent");
            treeView.SetDisplayProperty("Tag.Text");
            treeView.SetImageProperty("Tag.Image");
            treeView.SetShowNodeImages(true);
            treeView.OnDoubleClick += OnEditItemHandler;
            mainPanel.AddItem(treeView);

            // Root Node
            var rootNode = CreateRootNode();

            // Main Menu
            var toolBar = new ToolBarElement(view);
            mainPanel.AddItem(toolBar);

            // ContextMenu
            var contextMenu = new ContextMenuElement(view);
            treeView.SetContextMenu(contextMenu);

            // Add

            var addButton = new ToolBarButtonItem(view);
            addButton.SetText(Resources.MenuDesignerAddButton);
            addButton.SetImage("Actions/Add_16x16");
            addButton.SetHotkey("Ctrl+N");
            addButton.OnClick += OnAddItemHandler;
            toolBar.AddItem(addButton);

            var addMenuButton = new ContextMenuItem(view);
            addMenuButton.SetText(Resources.MenuDesignerAddButton);
            addMenuButton.SetImage("Actions/Add_16x16");
            addMenuButton.SetHotkey("Ctrl+N");
            addMenuButton.OnClick += OnAddItemHandler;
            contextMenu.AddItem(addMenuButton);

            // Edit

            var editButton = new ToolBarButtonItem(view);
            editButton.SetText(Resources.MenuDesignerEditButton);
            editButton.SetImage("Actions/Edit_16x16");
            editButton.SetHotkey("Ctrl+O");
            editButton.OnClick += OnEditItemHandler;
            toolBar.AddItem(editButton);

            var editMenuButton = new ContextMenuItem(view);
            editMenuButton.SetText(Resources.MenuDesignerEditButton);
            editMenuButton.SetImage("Actions/Edit_16x16");
            editMenuButton.SetHotkey("Ctrl+O");
            editMenuButton.OnClick += OnEditItemHandler;
            contextMenu.AddItem(editMenuButton);

            // Delete

            var deleteButton = new ToolBarButtonItem(view);
            deleteButton.SetText(Resources.MenuDesignerDeleteButton);
            deleteButton.SetImage("Actions/Delete_16x16");
            deleteButton.SetHotkey("Ctrl+Delete");
            deleteButton.OnClick += OnDeleteItemHandler;
            toolBar.AddItem(deleteButton);

            var deleteMenuButton = new ContextMenuItem(view);
            deleteMenuButton.SetText(Resources.MenuDesignerDeleteButton);
            deleteMenuButton.SetImage("Actions/Delete_16x16");
            deleteMenuButton.SetHotkey("Ctrl+Delete");
            deleteMenuButton.OnClick += OnDeleteItemHandler;
            contextMenu.AddItem(deleteMenuButton);

            // Separator1

            var separator1 = new ToolBarSeparatorItem(view);
            toolBar.AddItem(separator1);

            var menuSeparator1 = new ContextMenuItemSeparator(view);
            contextMenu.AddItem(menuSeparator1);

            // Cut

            var cutButton = new ToolBarButtonItem(view);
            cutButton.SetToolTip(Resources.MenuDesignerCutButtonToolTip);
            cutButton.SetImage("Actions/Cut_16x16");
            cutButton.SetHotkey("Ctrl+X");
            cutButton.OnClick += OnCutItemHandler;
            toolBar.AddItem(cutButton);

            var cutMenuButton = new ContextMenuItem(view);
            cutMenuButton.SetText(Resources.MenuDesignerCutButton);
            cutMenuButton.SetImage("Actions/Cut_16x16");
            cutMenuButton.SetHotkey("Ctrl+X");
            cutMenuButton.OnClick += OnCutItemHandler;
            contextMenu.AddItem(cutMenuButton);

            // Copy

            var copyButton = new ToolBarButtonItem(view);
            copyButton.SetToolTip(Resources.MenuDesignerCopyButtonToolTip);
            copyButton.SetImage("Actions/Copy_16x16");
            copyButton.SetHotkey("Ctrl+C");
            copyButton.OnClick += OnCopyItemHandler;
            toolBar.AddItem(copyButton);

            var copyMenuButton = new ContextMenuItem(view);
            copyMenuButton.SetText(Resources.MenuDesignerCopyButton);
            copyMenuButton.SetImage("Actions/Copy_16x16");
            copyMenuButton.SetHotkey("Ctrl+C");
            copyMenuButton.OnClick += OnCopyItemHandler;
            contextMenu.AddItem(copyMenuButton);

            // Paste

            var pasteButton = new ToolBarButtonItem(view);
            pasteButton.SetToolTip(Resources.MenuDesignerPasteButtonToolTip);
            pasteButton.SetImage("Actions/Paste_16x16");
            pasteButton.SetHotkey("Ctrl+V");
            pasteButton.OnClick += OnPasteItemHandler;
            toolBar.AddItem(pasteButton);

            var pasteMenuButton = new ContextMenuItem(view);
            pasteMenuButton.SetText(Resources.MenuDesignerPasteButton);
            pasteMenuButton.SetImage("Actions/Paste_16x16");
            pasteMenuButton.SetHotkey("Ctrl+V");
            pasteMenuButton.OnClick += OnPasteItemHandler;
            contextMenu.AddItem(pasteMenuButton);

            // Separator2

            var separator2 = new ToolBarSeparatorItem(view);
            toolBar.AddItem(separator2);

            var menuSeparator2 = new ContextMenuItemSeparator(view);
            contextMenu.AddItem(menuSeparator2);

            // MoveUp

            var moveUpButton = new ToolBarButtonItem(view);
            moveUpButton.SetToolTip(Resources.MenuDesignerMoveUpButtonToolTip);
            moveUpButton.SetImage("Actions/MoveUp_16x16");
            moveUpButton.SetHotkey("Alt+Up");
            moveUpButton.OnClick += OnMoveUpItemHandler;
            toolBar.AddItem(moveUpButton);

            var moveUpMenuButton = new ContextMenuItem(view);
            moveUpMenuButton.SetText(Resources.MenuDesignerMoveUpButton);
            moveUpMenuButton.SetImage("Actions/MoveUp_16x16");
            moveUpMenuButton.SetHotkey("Alt+Up");
            moveUpMenuButton.OnClick += OnMoveUpItemHandler;
            contextMenu.AddItem(moveUpMenuButton);

            // MoveDown

            var moveDownButton = new ToolBarButtonItem(view);
            moveDownButton.SetToolTip(Resources.MenuDesignerMoveDownButtonToolTip);
            moveDownButton.SetImage("Actions/MoveDown_16x16");
            moveDownButton.SetHotkey("Alt+Down");
            moveDownButton.OnClick += OnMoveDownItemHandler;
            toolBar.AddItem(moveDownButton);

            var moveDownMenuButton = new ContextMenuItem(view);
            moveDownMenuButton.SetText(Resources.MenuDesignerMoveDownButton);
            moveDownMenuButton.SetImage("Actions/MoveDown_16x16");
            moveDownMenuButton.SetHotkey("Alt+Down");
            moveDownMenuButton.OnClick += OnMoveDownItemHandler;
            contextMenu.AddItem(moveDownMenuButton);

            _rootNode = rootNode;
            _treeView = treeView;

            Control.Content = mainPanel.GetControl();

            SetItems(new List<object>());
        }

        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события обновления списка.
        /// </summary>
        public ScriptDelegate OnUpdateItems { get; set; }

        // Add, Edit, Delete Handlers

        private void OnAddItemHandler(dynamic context, dynamic arguments)
        {
            var selectedNode = GetSelectedNode(false);

            if (selectedNode != null)
            {
                ViewHelper.ShowView(null,
                    () => GetEditor(),
                    childDataSource => OnInitializeAddView(childDataSource),
                    childDataSource => OnAcceptedAddView(childDataSource, selectedNode));
            }
        }

        private void OnInitializeAddView(IDataSource childDataSource)
        {
            childDataSource.SuspendUpdate();
            childDataSource.SetEditMode();
            childDataSource.ResumeUpdate();
            childDataSource.SetSelectedItem(CreateNewMenuItem());
        }

        private void OnAcceptedAddView(IDataSource childDataSource, object selectedNode)
        {
            var newItem = childDataSource.GetSelectedItem();

            if (newItem != null)
            {
                // Добавление элемента в коллекцию
                var items = GetChildMenuItems(selectedNode);
                ObjectHelper.AddItem(items, newItem);

                // Добавление элемента в дерево
                var newItemNode = CreateMenuItemNode(newItem, selectedNode);
                _treeView.AddItem(newItemNode);
                _treeView.SetSelectedItem(newItemNode);

                InvokeUpdateItems();
            }
        }

        private void OnEditItemHandler(dynamic context, dynamic arguments)
        {
            var itemEditor = GetEditor();
            var selectedNode = GetSelectedNode(true);

            if (itemEditor != null && selectedNode != null)
            {
                object editItem = selectedNode.Tag;

                ViewHelper.ShowView(editItem,
                    () => GetEditor(),
                    childDataSource => OnInitializeEditView(childDataSource, editItem),
                    childDataSource => OnAcceptedEditView(childDataSource, editItem, selectedNode));
            }
        }

        private static void OnInitializeEditView(IDataSource childDataSource, dynamic editItem)
        {
            childDataSource.SuspendUpdate();
            childDataSource.SetEditMode();
            childDataSource.ResumeUpdate();
            childDataSource.SetSelectedItem(editItem.Clone());
        }

        private void OnAcceptedEditView(IDataSource childDataSource, dynamic editItem, dynamic selectedNode)
        {
            var newItem = childDataSource.GetSelectedItem();

            if (newItem != null)
            {
                // Замена элемента в коллекции
                var items = GetChildMenuItems(selectedNode.Parent);
                ObjectHelper.ReplaceItem(items, editItem, newItem);

                // Замена элемента в дереве
                selectedNode.Tag = newItem;
                _treeView.RefreshItem(selectedNode);

                InvokeUpdateItems();
            }
        }

        private void OnDeleteItemHandler(dynamic context, dynamic arguments)
        {
            var selectedNode = GetSelectedNode(true);

            if (selectedNode != null)
            {
                var deleteItem = selectedNode.Tag;

                if (
                    MessageBox.Show(string.Format(Resources.MenuDesignerDeleteQuestion, deleteItem.Text),
                        GetView().GetText(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // Удаление элемента из коллекции
                    var items = GetChildMenuItems(selectedNode.Parent);
                    ObjectHelper.RemoveItem(items, deleteItem);

                    // Удаление элемента из дерева
                    _treeView.RemoveItem(selectedNode, true);

                    InvokeUpdateItems();
                }
            }
        }

        private object CreateNewMenuItem()
        {
            var dataSource = GetViewDataSource(GetView());

            dynamic menuItem = new DynamicWrapper();
            menuItem.Action = new DynamicWrapper();
            menuItem.Action.OpenViewAction = new DynamicWrapper();
            menuItem.Action.OpenViewAction.View = new DynamicWrapper();
            menuItem.Action.OpenViewAction.View.AutoView = new DynamicWrapper();
            menuItem.Action.OpenViewAction.View.AutoView.ConfigId = (dataSource != null)
                ? dataSource.GetConfigId() ?? ""
                : "";
            menuItem.Action.OpenViewAction.View.AutoView.DocumentId = "";
            menuItem.Action.OpenViewAction.View.AutoView.MetadataName = "ListView";
            menuItem.Action.OpenViewAction.View.AutoView.OpenMode = "Application";

            return menuItem;
        }

        private static IDataSource GetViewDataSource(View view)
        {
            var dataSources = view.GetDataSources();

            if (dataSources != null)
            {
                return dataSources.FirstOrDefault();
            }

            return null;
        }

        private void OnCutItemHandler(dynamic context, dynamic arguments)
        {
            _copyMode = false;
            _clipboardNode = GetSelectedNode(true);
        }

        private void OnCopyItemHandler(dynamic context, dynamic arguments)
        {
            _copyMode = true;
            _clipboardNode = GetSelectedNode(true);
        }

        private void OnPasteItemHandler(dynamic context, dynamic arguments)
        {
            if (_clipboardNode != null)
            {
                var newParentNode = GetSelectedNode(false);

                if (newParentNode != null && newParentNode != _clipboardNode)
                {
                    if (IsParentNode(_clipboardNode, newParentNode) == false)
                    {
                        var newParentItems = GetChildMenuItems(newParentNode);
                        var oldParentItems = GetChildMenuItems(_clipboardNode.Parent);
                        var clipboardItem = _clipboardNode.Tag;

                        if (_copyMode == false)
                        {
                            // Перемещение элемента
                            ObjectHelper.RemoveItem(oldParentItems, clipboardItem);
                            ObjectHelper.AddItem(newParentItems, clipboardItem);

                            // Перемещение элемента в дереве
                            _clipboardNode.Parent = newParentNode;

                            _treeView.RefreshItems();
                        }
                        else
                        {
                            // Копирование элемента
                            var copyItem = clipboardItem.Clone();
                            ObjectHelper.AddItem(newParentItems, copyItem);

                            // Копирование элемента в дереве
                            var copyNode = CreateMenuItemNode(copyItem, newParentNode);
                            var childNodes = new List<object> {copyNode};
                            FillChildMenuItems(copyItem, copyNode, childNodes);
                            childNodes.ForEach(node => _treeView.AddItem(node));
                        }

                        _clipboardNode = null;

                        InvokeUpdateItems();
                    }
                    else
                    {
                        MessageBox.Show(Resources.MenuDesignerCannotMoveParentToChild, GetView().GetText(),
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private static bool IsParentNode(dynamic parentNode, dynamic childNode)
        {
            var isParent = false;

            while (childNode != null)
            {
                if (childNode.Parent == parentNode)
                {
                    isParent = true;
                    break;
                }

                childNode = childNode.Parent;
            }

            return isParent;
        }

        // MoveUp, MoveDown Handlers

        private void OnMoveUpItemHandler(dynamic context, dynamic arguments)
        {
            MoveItem(-1);
        }

        private void OnMoveDownItemHandler(dynamic context, dynamic arguments)
        {
            MoveItem(+1);
        }

        private void MoveItem(int delta)
        {
            var selectedNode = GetSelectedNode(true);

            if (selectedNode != null)
            {
                // Перемещение элемента в коллекции
                var items = GetChildMenuItems(selectedNode.Parent);
                ObjectHelper.MoveItem(items, selectedNode.Tag, delta);

                // Перемещение элемента в дереве
                _treeView.MoveItem(selectedNode, delta);
            }
        }

        // Helpers

        private static IEnumerable GetChildMenuItems(dynamic parentNode)
        {
            if (parentNode.Tag.Items == null)
            {
                parentNode.Tag.Items = new List<object>();
            }

            return parentNode.Tag.Items;
        }

        private dynamic GetSelectedNode(bool nullIfRootNode)
        {
            var selectedNode = _treeView.GetSelectedItem() ?? _rootNode;

            if (nullIfRootNode && selectedNode == _rootNode)
            {
                selectedNode = null;
            }

            return selectedNode;
        }

        /// <summary>
        ///     Возвращает редактор элемента меню.
        /// </summary>
        public LinkView GetEditor()
        {
            return _editor;
        }

        /// <summary>
        ///     Устанавливает редактор элемента меню.
        /// </summary>
        public void SetEditor(LinkView value)
        {
            _editor = value;
        }

        // Items

        /// <summary>
        ///     Возвращает список элементов меню.
        /// </summary>
        public IEnumerable GetItems()
        {
            return _rootNode.Tag.Items;
        }

        /// <summary>
        ///     Устанавливает список элементов меню.
        /// </summary>
        public void SetItems(IEnumerable value)
        {
            if (Equals(_rootNode.Items, value) == false)
            {
                _rootNode.Tag.Items = value;

                FillMenuItems();
            }
        }

        private void FillMenuItems()
        {
            var items = GetItems();
            var treeViewNodes = new List<object>();

            if (items != null)
            {
                treeViewNodes.Add(_rootNode);

                foreach (dynamic menuItem in items)
                {
                    var menuItemNode = CreateMenuItemNode(menuItem, _rootNode);
                    treeViewNodes.Add(menuItemNode);

                    FillChildMenuItems(menuItem, menuItemNode, treeViewNodes);
                }
            }

            _treeView.SetItems(treeViewNodes);
            _treeView.SetSelectedItem(_rootNode);
            _treeView.ExpandItem(_rootNode);
        }

        private static void FillChildMenuItems(dynamic menuItem, dynamic parentNode, ICollection<object> treeViewNodes)
        {
            var childItems = menuItem.Items;

            if (childItems != null)
            {
                foreach (var childMenuItem in childItems)
                {
                    var childMenuItemNode = CreateMenuItemNode(childMenuItem, parentNode);
                    treeViewNodes.Add(childMenuItemNode);

                    FillChildMenuItems(childMenuItem, childMenuItemNode, treeViewNodes);
                }
            }
        }

        private static object CreateMenuItemNode(dynamic menuItem, dynamic parentNode)
        {
            dynamic menuItemNode = new DynamicWrapper();
            menuItemNode.Key = menuItemNode;
            menuItemNode.Parent = parentNode;
            menuItemNode.Tag = menuItem;
            return menuItemNode;
        }

        private static object CreateRootNode()
        {
            dynamic rootItem = new DynamicWrapper();
            rootItem.Text = Resources.MenuDesignerRootItemText;
            rootItem.Image = "System/Menu_16x16";
            return CreateMenuItemNode(rootItem, null);
        }

        private void InvokeUpdateItems()
        {
            this.InvokeScript(OnUpdateItems, a => a.Value = GetItems());
        }
    }
}