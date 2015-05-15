using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSchemaDesigner
{
	/// <summary>
	/// Элемент представления для редактирования модели данных.
	/// </summary>
	sealed class DocumentSchemaDesignerElement : BaseElement<UserControl>
	{
		public DocumentSchemaDesignerElement(View view)
			: base(view)
		{
			var mainPanel = new StackPanelElement(view);

			// TreeView
			var treeView = new TreeViewElement(view);
			treeView.SetKeyProperty("Key");
			treeView.SetParentProperty("Parent");
			treeView.SetImageProperty("Image");
			treeView.SetDisplayProperty("Tag.Text");
			treeView.SetShowNodeImages(true);
			treeView.OnDoubleClick += OnEditPropertyHandler;
			mainPanel.AddItem(treeView);

			// ToolBar
			var toolBar = new ToolBarElement(view);
			mainPanel.AddItem(toolBar);

			// ContextMenu
			var contextMenu = new ContextMenuElement(view);
			treeView.SetContextMenu(contextMenu);

			// Add

			var addButton = new ToolBarPopupButtonItem(view);
			addButton.SetText(Resources.DocumentSchemaDesignerAddButton);
			addButton.SetImage("Actions/Add_16x16");
			addButton.SetHotkey("Ctrl+N");
			toolBar.AddItem(addButton);

			var addMenuButton = new ContextMenuItem(view);
			addMenuButton.SetText(Resources.DocumentSchemaDesignerAddButton);
			addMenuButton.SetImage("Actions/Add_16x16");
			addMenuButton.SetHotkey("Ctrl+N");
			contextMenu.AddItem(addMenuButton);

			// Edit

			var editButton = new ToolBarButtonItem(view);
			editButton.SetText(Resources.DocumentSchemaDesignerEditButton);
			editButton.SetImage("Actions/Edit_16x16");
			editButton.SetHotkey("Ctrl+O");
			editButton.OnClick += OnEditPropertyHandler;
			toolBar.AddItem(editButton);

			var editMenuButton = new ContextMenuItem(view);
			editMenuButton.SetText(Resources.DocumentSchemaDesignerEditButton);
			editMenuButton.SetImage("Actions/Edit_16x16");
			editMenuButton.SetHotkey("Ctrl+O");
			editMenuButton.OnClick += OnEditPropertyHandler;
			contextMenu.AddItem(editMenuButton);

			// Delete

			var deleteButton = new ToolBarButtonItem(view);
			deleteButton.SetText(Resources.DocumentSchemaDesignerDeleteButton);
			deleteButton.SetImage("Actions/Delete_16x16");
			deleteButton.SetHotkey("Ctrl+Delete");
			deleteButton.OnClick += OnDeletePropertyHandler;
			toolBar.AddItem(deleteButton);

			var deleteMenuButton = new ContextMenuItem(view);
			deleteMenuButton.SetText(Resources.DocumentSchemaDesignerDeleteButton);
			deleteMenuButton.SetImage("Actions/Delete_16x16");
			deleteMenuButton.SetHotkey("Ctrl+Delete");
			deleteMenuButton.OnClick += OnDeletePropertyHandler;
			contextMenu.AddItem(deleteMenuButton);

			// Separator1

			var separator1 = new ToolBarSeparatorItem(view);
			toolBar.AddItem(separator1);

			var menuSeparator1 = new ContextMenuItemSeparator(view);
			contextMenu.AddItem(menuSeparator1);

			// Cut

			var cutButton = new ToolBarButtonItem(view);
			cutButton.SetToolTip(Resources.DocumentSchemaDesignerCutButtonToolTip);
			cutButton.SetImage("Actions/Cut_16x16");
			cutButton.SetHotkey("Ctrl+X");
			cutButton.OnClick += OnCutPropertyHandler;
			toolBar.AddItem(cutButton);

			var cutMenuButton = new ContextMenuItem(view);
			cutMenuButton.SetText(Resources.DocumentSchemaDesignerCutButton);
			cutMenuButton.SetImage("Actions/Cut_16x16");
			cutMenuButton.SetHotkey("Ctrl+X");
			cutMenuButton.OnClick += OnCutPropertyHandler;
			contextMenu.AddItem(cutMenuButton);

			// Copy

			var copyButton = new ToolBarButtonItem(view);
			copyButton.SetToolTip(Resources.DocumentSchemaDesignerCopyButtonToolTip);
			copyButton.SetImage("Actions/Copy_16x16");
			copyButton.SetHotkey("Ctrl+C");
			copyButton.OnClick += OnCopyPropertyHandler;
			toolBar.AddItem(copyButton);

			var copyMenuButton = new ContextMenuItem(view);
			copyMenuButton.SetText(Resources.DocumentSchemaDesignerCopyButton);
			copyMenuButton.SetImage("Actions/Copy_16x16");
			copyMenuButton.SetHotkey("Ctrl+C");
			copyMenuButton.OnClick += OnCopyPropertyHandler;
			contextMenu.AddItem(copyMenuButton);

			// Paste

			var pasteButton = new ToolBarButtonItem(view);
			pasteButton.SetToolTip(Resources.DocumentSchemaDesignerPasteButtonToolTip);
			pasteButton.SetImage("Actions/Paste_16x16");
			pasteButton.SetHotkey("Ctrl+V");
			pasteButton.OnClick += OnPastePropertyHandler;
			toolBar.AddItem(pasteButton);

			var pasteMenuButton = new ContextMenuItem(view);
			pasteMenuButton.SetText(Resources.DocumentSchemaDesignerPasteButton);
			pasteMenuButton.SetImage("Actions/Paste_16x16");
			pasteMenuButton.SetHotkey("Ctrl+V");
			pasteMenuButton.OnClick += OnPastePropertyHandler;
			contextMenu.AddItem(pasteMenuButton);

			// Separator2

			var separator2 = new ToolBarSeparatorItem(view);
			toolBar.AddItem(separator2);

			var menuSeparator2 = new ContextMenuItemSeparator(view);
			contextMenu.AddItem(menuSeparator2);

			// MoveUp

			var moveUpButton = new ToolBarButtonItem(view);
			moveUpButton.SetToolTip(Resources.DocumentSchemaDesignerMoveUpButtonToolTip);
			moveUpButton.SetImage("Actions/MoveUp_16x16");
			moveUpButton.SetHotkey("Alt+Up");
			moveUpButton.OnClick += OnMoveUpPropertyHandler;
			toolBar.AddItem(moveUpButton);

			var moveUpMenuButton = new ContextMenuItem(view);
			moveUpMenuButton.SetText(Resources.DocumentSchemaDesignerMoveUpButton);
			moveUpMenuButton.SetImage("Actions/MoveUp_16x16");
			moveUpMenuButton.SetHotkey("Alt+Up");
			moveUpMenuButton.OnClick += OnMoveUpPropertyHandler;
			contextMenu.AddItem(moveUpMenuButton);

			// MoveDown

			var moveDownButton = new ToolBarButtonItem(view);
			moveDownButton.SetToolTip(Resources.DocumentSchemaDesignerMoveDownButtonToolTip);
			moveDownButton.SetImage("Actions/MoveDown_16x16");
			moveDownButton.SetHotkey("Alt+Down");
			moveDownButton.OnClick += OnMoveDownPropertyHandler;
			toolBar.AddItem(moveDownButton);

			var moveDownMenuButton = new ContextMenuItem(view);
			moveDownMenuButton.SetText(Resources.DocumentSchemaDesignerMoveDownButton);
			moveDownMenuButton.SetImage("Actions/MoveDown_16x16");
			moveDownMenuButton.SetHotkey("Alt+Down");
			moveDownMenuButton.OnClick += OnMoveDownPropertyHandler;
			contextMenu.AddItem(moveDownMenuButton);

			_treeView = treeView;
			_addButton = addButton;
			_addMenuButton = addMenuButton;

			Control.Content = mainPanel.GetControl();

			// Пустое дерево модели данных
			SetValue(null);
		}


		private readonly TreeViewElement _treeView;
		private readonly ToolBarPopupButtonItem _addButton;
		private readonly ContextMenuItem _addMenuButton;


		// Add, Edit, Delete Handlers

		private void OnAddPropertyHandler(string type)
		{
			var selectedNode = GetSelectedNode(false);

			if (selectedNode != null && !CanContainNodes(selectedNode, false))
			{
				selectedNode = selectedNode.Parent;
			}

			// Cвойства можно определять только у объектов и элементов массива
			if (selectedNode != null && CanContainNodes(selectedNode, true))
			{
				var itemEditor = FindItemEditor(type);

				if (itemEditor != null && itemEditor.LinkView != null)
				{
					ViewHelper.ShowView(null,
										() => itemEditor.LinkView,
										childDataSource => OnInitializeAddView(childDataSource),
										childDataSource => OnAcceptedAddView(childDataSource, type, selectedNode));
				}
			}
		}

		private static void OnInitializeAddView(IDataSource childDataSource)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.ResumeUpdate();
		}

		private void OnAcceptedAddView(IDataSource childDataSource, string type, object selectedNode)
		{
			dynamic newSchemaInfo = childDataSource.GetSelectedItem();

			if (newSchemaInfo != null)
			{
				newSchemaInfo.Type = type;
				UpdateSchemaInfoText(newSchemaInfo);

				// Добавление элемента в дерево
				var newSchemaNode = CreateSchemaNode(newSchemaInfo, selectedNode);
				_treeView.AddItem(newSchemaNode);
				_treeView.SetSelectedItem(selectedNode);
				_treeView.ExpandItem(selectedNode);

				InvokeValueChanged();
			}
		}


		private void OnEditPropertyHandler(dynamic context, dynamic arguments)
		{
			var selectedNode = GetSelectedNode(false);

			if (selectedNode != null)
			{
				string type = selectedNode.Tag.Type;
				var itemEditor = FindItemEditor(type);

				if (itemEditor != null && itemEditor.LinkView != null)
				{
					object schemaInfo = selectedNode.Tag;

					ViewHelper.ShowView(schemaInfo,
										() => itemEditor.LinkView,
										childDataSource => OnInitializeEditView(childDataSource, schemaInfo),
										childDataSource => OnAcceptedEditView(childDataSource, type, selectedNode));
				}
			}
		}

		private static void OnInitializeEditView(IDataSource childDataSource, dynamic schemaInfo)
		{
			childDataSource.SuspendUpdate();
			childDataSource.SetEditMode();
			childDataSource.ResumeUpdate();
			childDataSource.SetSelectedItem(schemaInfo.Clone());
		}

		private void OnAcceptedEditView(IDataSource childDataSource, string type, dynamic selectedNode)
		{
			dynamic newSchemaInfo = childDataSource.GetSelectedItem();

			if (newSchemaInfo != null)
			{
				newSchemaInfo.Type = type;
				UpdateSchemaInfoText(newSchemaInfo);

				// Замена элемента в дереве
				selectedNode.Tag = newSchemaInfo;
				_treeView.RefreshItem(selectedNode);
				_treeView.SetSelectedItem(selectedNode);
				_treeView.ExpandItem(selectedNode);

				InvokeValueChanged();
			}
		}


		private void OnDeletePropertyHandler(dynamic context, dynamic arguments)
		{
			var selectedNode = GetSelectedNode(true);

			if (selectedNode != null
				&& MessageBox.Show(string.Format(Resources.DocumentSchemaDesignerDeleteQuestion, selectedNode.Tag.Text), GetView().GetText(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				RemoveNode(selectedNode);
			}
		}


		// Cut, Copy, Paste Handlers

		private const string DataSchemaClipboardKey = "DataSchema";

		private void OnCutPropertyHandler(dynamic context, dynamic arguments)
		{
			var clipboardNode = GetSelectedNode(true);

			if (clipboardNode != null)
			{
				context.Global.Clipboard.Enqueue(DataSchemaClipboardKey, clipboardNode, (Func<object, bool>)RemoveNode);
			}
		}

		private void OnCopyPropertyHandler(dynamic context, dynamic arguments)
		{
			var clipboardNode = GetSelectedNode(true);

			if (clipboardNode != null)
			{
				context.Global.Clipboard.Enqueue(DataSchemaClipboardKey, clipboardNode);
			}
		}

		private void OnPastePropertyHandler(dynamic context, dynamic arguments)
		{
			var clipboardNode = context.Global.Clipboard.Dequeue(DataSchemaClipboardKey, (Func<object, bool>)CanInsertNode);

			if (clipboardNode != null)
			{
				clipboardNode = CloneNode(clipboardNode);

				InsertNode(clipboardNode);
			}
		}

		private bool RemoveNode(dynamic propertyNode)
		{
			// Выделенный узел
			var selectedNode = GetSelectedNode(false);

			// Удаление из родительского узла
			var oldParentNode = propertyNode.Parent;
			ObjectHelper.RemoveItem(oldParentNode.Nodes, propertyNode);
			propertyNode.Parent = null;

			// Удаление из коллекции дерева
			_treeView.RemoveItem(propertyNode, true);
			_treeView.RefreshItems();

			// Выделение родительского узла
			if (propertyNode == selectedNode)
			{
				_treeView.SetSelectedItem(oldParentNode);
				_treeView.ExpandItem(oldParentNode);
			}

			InvokeValueChanged();

			return true;
		}

		private bool CanInsertNode(dynamic propertyNode)
		{
			var newParentNode = GetSelectedNode(false);

			return (propertyNode != null)
				   && (newParentNode != null)
				   && (newParentNode != propertyNode)
				   && !IsParentNode(propertyNode, newParentNode)
				   && CanContainNodes(newParentNode, true);
		}

		private void InsertNode(dynamic propertyNode)
		{
			// Выделенный узел
			var newParentNode = GetSelectedNode(false);

			// Добавление в родительский узел
			ObjectHelper.AddItem(newParentNode.Nodes, propertyNode);
			propertyNode.Parent = newParentNode;

			// Добавление в коллекцию дерева
			var childNodes = new List<object>();
			FillChildNodes(propertyNode, childNodes);
			childNodes.ForEach(node => _treeView.AddItem(node));

			// Выделение родительского узла
			_treeView.SetSelectedItem(newParentNode);
			_treeView.ExpandItem(newParentNode);

			InvokeValueChanged();
		}

		private bool IsParentNode(dynamic parentNode, dynamic childNode)
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

			if (isParent)
			{
				MessageBox.Show(Resources.DocumentSchemaDesignerCannotMoveParentToChild, GetView().GetText(), MessageBoxButton.OK, MessageBoxImage.Warning);
			}

			return isParent;
		}

		private bool CanContainNodes(dynamic propertyNode, bool showMessage)
		{
			if (propertyNode.Tag.Type != DataType.Object.ToString() && propertyNode.Tag.Type != DataType.Array.ToString())
			{
				if (showMessage)
				{
					MessageBox.Show(Resources.DocumentSchemaDesignerCannotContainProperties, GetView().GetText(), MessageBoxButton.OK, MessageBoxImage.Warning);
				}

				return false;
			}

			return true;
		}

		private static dynamic CloneNode(dynamic propertyNode)
		{
			var result = propertyNode;

			if (propertyNode != null)
			{
				var parent = propertyNode.Parent;
				propertyNode.Parent = null;
				result = propertyNode.Clone();
				propertyNode.Parent = parent;
			}

			return result;
		}

		private static void FillChildNodes(dynamic propertyNode, ICollection<object> treeViewNodes)
		{
			treeViewNodes.Add(propertyNode);

			foreach (var childNode in propertyNode.Nodes)
			{
				FillChildNodes(childNode, treeViewNodes);
			}
		}


		// MoveUp, MoveDown Handlers

		private void OnMoveUpPropertyHandler(dynamic context, dynamic arguments)
		{
			MoveProperty(-1);
		}

		private void OnMoveDownPropertyHandler(dynamic context, dynamic arguments)
		{
			MoveProperty(+1);
		}

		private void MoveProperty(int delta)
		{
			var selectedNode = GetSelectedNode(true);

			if (selectedNode != null)
			{
				ObjectHelper.MoveItem(selectedNode.Parent.Nodes, selectedNode, delta);

				_treeView.MoveItem(selectedNode, delta);

				InvokeValueChanged();
			}
		}


		// Editors

		private IEnumerable<ItemEditor> _editors;

		/// <summary>
		/// Возвращает список редакторов свойств модели данных.
		/// </summary>
		public IEnumerable<ItemEditor> GetEditors()
		{
			return _editors;
		}

		/// <summary>
		/// Устанавливает список редакторов элементов документа.
		/// </summary>
		public void SetEditors(IEnumerable<ItemEditor> value)
		{
			if (Equals(_editors, value) == false)
			{
				_editors = value;

				RefreshAddButton();
				SchemaToTree(GetValue());
			}
		}

		private void RefreshAddButton()
		{
			var addButtons = _addButton.GetItems();

			if (addButtons != null)
			{
				foreach (var addButton in addButtons.ToArray())
				{
					_addButton.RemoveItem(addButton);
				}
			}

			var addMenuButtons = _addMenuButton.GetItems();

			if (addMenuButtons != null)
			{
				foreach (var addButton in addMenuButtons.ToArray())
				{
					_addMenuButton.RemoveItem(addButton);
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
					addButton.OnClick += (c, a) => OnAddPropertyHandler(itemEditor.MetadataType);

					var addMenuButton = new ContextMenuItem(GetView());
					addMenuButton.SetText(itemEditor.Text);
					addMenuButton.SetImage(itemEditor.Image);
					addMenuButton.OnClick += (c, a) => OnAddPropertyHandler(itemEditor.MetadataType);

					_addButton.AddItem(addButton);
					_addMenuButton.AddItem(addMenuButton);
				}
			}
		}

		private ItemEditor FindItemEditor(string type)
		{
			var editors = GetEditors();

			return (editors != null) ? editors.FirstOrDefault(i => i.MetadataType == type) : null;
		}


		// Value

		/// <summary>
		/// Возвращает модель данных.
		/// </summary>
		public object GetValue()
		{
			return SchemaFromTree();
		}

		/// <summary>
		/// Устанавливает модель данных.
		/// </summary>
		public void SetValue(object value)
		{
			SchemaToTree(value);
		}


		// Преобразование визуального дерева в модель данных

		private object SchemaFromTree()
		{
			var rootSchemaNode = GetRootSchemaNode();
			var rootSchemaInfo = rootSchemaNode.Tag;

			dynamic rootSchema = new DynamicWrapper();
			rootSchema.Type = rootSchemaInfo.Type;
			rootSchema.TypeInfo = rootSchemaInfo.TypeInfo;
			rootSchema.Caption = rootSchemaInfo.Caption;
			rootSchema.Description = rootSchemaInfo.Description;

			SchemaFromTree(rootSchema, rootSchemaNode);

			return rootSchema;
		}

		private static void SchemaFromTree(dynamic schema, dynamic parentNode)
		{
			schema.Properties = new DynamicWrapper();

			foreach (var propertyNode in parentNode.Nodes)
			{
				var schemaInfo = propertyNode.Tag;
				var propertyName = schemaInfo.Name;
				var propertySchemaType = schemaInfo.Type;

				dynamic propertySchema = new DynamicWrapper();
				propertySchema.Caption = schemaInfo.Caption;
				propertySchema.Description = schemaInfo.Description;
				propertySchema.Type = propertySchemaType;
				propertySchema.Sortable = schemaInfo.Sortable;

				if (propertySchemaType != DataType.Array.ToString())
				{
					propertySchema.TypeInfo = schemaInfo.TypeInfo;
				}

				if (propertySchemaType == DataType.Object.ToString())
				{
					SchemaFromTree(propertySchema, propertyNode);
				}
				else if (propertySchemaType == DataType.Array.ToString())
				{
					dynamic itemsSchema = new DynamicWrapper();
					itemsSchema.Type = DataType.Object.ToString();
					itemsSchema.TypeInfo = schemaInfo.TypeInfo;
					itemsSchema.Sortable = schemaInfo.Sortable;

					propertySchema.Items = itemsSchema;

					SchemaFromTree(itemsSchema, propertyNode);
				}

				schema.Properties[propertyName] = propertySchema;
			}
		}


		// Преобразование модели данных в визуальное дерево

		private void SchemaToTree(dynamic rootSchema)
		{
			if (rootSchema == null)
			{
				rootSchema = CreateEmptySchema();
			}

			var rootSchemaInfo = CreateSchemaInfo(rootSchema, null);
			var rootSchemaNode = CreateSchemaNode(rootSchemaInfo, null);
			var treeViewNodes = new List<object> { rootSchemaNode };

			SchemaToTree(rootSchema, rootSchemaNode, treeViewNodes);

			_treeView.SetItems(treeViewNodes);
			_treeView.SetSelectedItem(rootSchemaNode);
			_treeView.ExpandItem(rootSchemaNode);
		}

		private void SchemaToTree(dynamic schema, dynamic parentNode, ICollection<object> treeViewNodes)
		{
			if (schema != null && schema.Properties != null)
			{
				foreach (var property in schema.Properties)
				{
					var propertyName = property.Key;
					var propertySchema = property.Value;
					var propertySchemaType = propertySchema.Type;

					var propertySchemaInfo = CreateSchemaInfo(propertySchema, propertyName);
					var propertySchemaNode = CreateSchemaNode(propertySchemaInfo, parentNode);
					treeViewNodes.Add(propertySchemaNode);

					if (propertySchemaType == DataType.Object.ToString())
					{
						SchemaToTree(propertySchema, propertySchemaNode, treeViewNodes);
					}
					else if (propertySchemaType == DataType.Array.ToString())
					{
						SchemaToTree(propertySchema.Items, propertySchemaNode, treeViewNodes);
					}
				}
			}
		}


		private static object CreateEmptySchema()
		{
			dynamic schema = new DynamicWrapper();
			schema.Type = DataType.Object.ToString();
			schema.Caption = Resources.DocumentSchemaDesignerRootText;
			return schema;
		}

		private static object CreateSchemaInfo(dynamic schema, string schemaName)
		{
			dynamic schemaInfo = new DynamicWrapper();
			schemaInfo.Type = schema.Type;
			schemaInfo.TypeInfo = GetSchemaTypeInfo(schema);
			schemaInfo.Name = schemaName;
			schemaInfo.Caption = schema.Caption;
			schemaInfo.Description = schema.Description;
			schemaInfo.Items = schema.Items;
			schemaInfo.Sortable = schema.Sortable;
			UpdateSchemaInfoText(schemaInfo);
			return schemaInfo;
		}

		private static object GetSchemaTypeInfo(dynamic schema)
		{
			if (schema.Type != DataType.Array.ToString())
			{
				return schema.TypeInfo;
			}

			if (schema.Items != null)
			{
				return schema.Items.TypeInfo;
			}

			return null;
		}

		private object CreateSchemaNode(dynamic schemaInfo, dynamic parentNode)
		{
			dynamic schemaNode = new DynamicWrapper();
			schemaNode.Key = schemaNode;
			schemaNode.Parent = parentNode;
			schemaNode.Nodes = new List<object>();

			if (parentNode != null)
			{
				parentNode.Nodes.Add(schemaNode);
			}

			var editor = FindItemEditor(schemaInfo.Type);

			if (editor != null)
			{
				schemaNode.Image = editor.Image;
			}

			schemaNode.Tag = schemaInfo;

			return schemaNode;
		}

		private static void UpdateSchemaInfoText(dynamic schemaInfo)
		{
			if (string.IsNullOrEmpty(schemaInfo.Name))
			{
				schemaInfo.Text = schemaInfo.Caption;
			}
			else if (string.IsNullOrEmpty(schemaInfo.Caption))
			{
				schemaInfo.Text = schemaInfo.Name;
			}
			else
			{
				schemaInfo.Text = string.Format("{0} ({1})", schemaInfo.Name, schemaInfo.Caption);
			}
		}


		// Helpers

		private dynamic GetSelectedNode(bool nullIfRootNode)
		{
			var rootNode = GetRootSchemaNode();
			var selectedNode = _treeView.GetSelectedItem() ?? rootNode;

			if (nullIfRootNode && selectedNode == rootNode)
			{
				selectedNode = null;
			}

			return selectedNode;
		}

		private dynamic GetRootSchemaNode()
		{
			var treeViewNodes = _treeView.GetItems();

			if (treeViewNodes != null)
			{
				return treeViewNodes.Cast<dynamic>()
					.FirstOrDefault(i => i.Parent == null);
			}

			return null;
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения модели данных.
		/// </summary>
		public ScriptDelegate OnValueChanged { get; set; }

		private void InvokeValueChanged()
		{
			this.InvokeScript(OnValueChanged, arguments => arguments.Value = GetValue());
		}
	}
}