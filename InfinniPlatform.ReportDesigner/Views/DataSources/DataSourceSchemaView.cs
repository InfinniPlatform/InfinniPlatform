using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для отображения и редактирования схемы источника данных.
    /// </summary>
    sealed partial class DataSourceSchemaView : UserControl
    {
        private bool _allowEdit;
        private TreeNode _clipboardPropertyNode;
        private string _dataSourceName;
        private bool _isCutProperty;
        private EventHandler _onImportDataSchema;
        private readonly DialogView<DataSourcePropertyView> _propertyDialog;

        public DataSourceSchemaView()
        {
            InitializeComponent();

            Text = Resources.DataSourceSchemaView;

            _propertyDialog = new DialogView<DataSourcePropertyView>();

            AllowEdit = false;
        }

        /// <summary>
        ///     Разрешить редактирование.
        /// </summary>
        [DefaultValue(false)]
        public bool AllowEdit
        {
            get { return _allowEdit; }
            set
            {
                _allowEdit = value;

                ControlPanel.Visible = value;

                SetAllowedActions();
            }
        }

        /// <summary>
        ///     Допустимый уровень вложенности данных.
        /// </summary>
        [DefaultValue(0)]
        public int DataNestingLevel { get; set; }

        /// <summary>
        ///     Наименование источника данных.
        /// </summary>
        public string DataSourceName
        {
            get { return _dataSourceName; }
            set
            {
                _dataSourceName = value;

                DataSourceTree.Nodes[0].Name = value;
                DataSourceTree.Nodes[0].Text = value;
            }
        }

        /// <summary>
        ///     Схема источника данных.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataSchema DataSourceSchema
        {
            get { return BuildDataSourceSchema(); }
            set { RenderDataSourceSchema(value); }
        }

        /// <summary>
        ///     Тип данных выбранного свойства источника данных.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SchemaDataType SelectedPropertyType
        {
            get { return GetNodeDataType(DataSourceTree.SelectedNode); }
        }

        /// <summary>
        ///     Путь к выбранному свойству источника данных.
        /// </summary>
        public string SelectedPropertyPath
        {
            get { return GetDataSourcePropertyPath(); }
            set { SetDataSourcePropertyPath(value); }
        }

        private DataSchema BuildDataSourceSchema()
        {
            var dataSchema = new DataSchema {Type = SchemaDataType.Object};

            BuildDataSourceSchemaTree(DataSourceTree.Nodes[0], dataSchema);

            return dataSchema;
        }

        private static void BuildDataSourceSchemaTree(TreeNode parentNode, DataSchema parentDataSchema)
        {
            parentDataSchema.Properties = new Dictionary<string, DataSchema>();

            foreach (TreeNode propertyNode in parentNode.Nodes)
            {
                var propertyName = propertyNode.Name;
                var propertyType = (SchemaDataType) propertyNode.Tag;

                var propertySchema = new DataSchema {Type = propertyType};

                if (propertyType == SchemaDataType.Object)
                {
                    BuildDataSourceSchemaTree(propertyNode, propertySchema);
                }
                else if (propertyType == SchemaDataType.Array)
                {
                    var itemsSchema = new DataSchema {Type = SchemaDataType.Object};

                    BuildDataSourceSchemaTree(propertyNode, itemsSchema);

                    propertySchema.Items = itemsSchema;
                }

                parentDataSchema.Properties.Add(propertyName, propertySchema);
            }
        }

        private void RenderDataSourceSchema(DataSchema dataSchema)
        {
            DataSourceTree.Nodes[0].Nodes.Clear();

            if (dataSchema != null)
            {
                RenderDataSourceSchemaTree(DataSourceTree.Nodes[0], dataSchema);
                DataSourceTree.ExpandAll();
            }
        }

        private static void RenderDataSourceSchemaTree(TreeNode parent, DataSchema dataSchema)
        {
            foreach (var property in dataSchema.Properties)
            {
                var propertyName = property.Key;
                var propertySchema = property.Value;
                var propertyType = propertySchema.Type;

                var propertyNode = CreatePropertyNode(parent, propertyName, propertyType);

                if (propertyType == SchemaDataType.Object)
                {
                    RenderDataSourceSchemaTree(propertyNode, propertySchema);
                }
                else if (propertyType == SchemaDataType.Array)
                {
                    RenderDataSourceSchemaTree(propertyNode, propertySchema.Items);
                }
            }
        }

        private string GetDataSourcePropertyPath()
        {
            string propertyPath = null;

            var propertyNode = DataSourceTree.SelectedNode;

            if (propertyNode != null)
            {
                var path = new Stack<string>();

                while (propertyNode != null && propertyNode.Tag != null)
                {
                    var propertyType = (SchemaDataType) propertyNode.Tag;

                    if (propertyType == SchemaDataType.Array)
                    {
                        path.Push("$");
                    }

                    path.Push(propertyNode.Name);

                    propertyNode = propertyNode.Parent;
                }

                propertyPath = string.Join(".", path);
            }

            return propertyPath;
        }

        private void SetDataSourcePropertyPath(string propertyPath)
        {
            TreeNode propertyNode = null;

            if (string.IsNullOrWhiteSpace(propertyPath) == false)
            {
                var path = propertyPath.Replace(".$", "").Split('.');

                propertyNode = DataSourceTree.Nodes[0];

                foreach (var propertyName in path)
                {
                    propertyNode = propertyNode.Nodes.Find(propertyName, false).FirstOrDefault();

                    if (propertyNode == null)
                    {
                        break;
                    }
                }
            }

            DataSourceTree.SelectedNode = propertyNode;
        }

        /// <summary>
        ///     Событие ипорта схемы источника данных.
        /// </summary>
        public event EventHandler OnImportDataSchema
        {
            add
            {
                _onImportDataSchema = (EventHandler) Delegate.Combine(_onImportDataSchema, value);

                var canImport = (value != null);
                ImportDataSchemaButton.Visible = canImport;
                Separator0Button.Visible = canImport;
            }
            remove { _onImportDataSchema = (EventHandler) Delegate.Remove(_onImportDataSchema, value); }
        }

        // IMPORT

        private void OnImportDataSchemaButtonClick(object sender, EventArgs e)
        {
            if (_onImportDataSchema != null)
            {
                _onImportDataSchema(sender, e);
            }
        }

        // CREATE

        private void OnCreatePropertyButtonClick(object sender, EventArgs e)
        {
            CreateProperty();
        }

        private void OnCreatePropertyMenuClick(object sender, EventArgs e)
        {
            CreateProperty();
        }

        private void CreateProperty()
        {
            _propertyDialog.View.PropertyName = string.Empty;
            _propertyDialog.View.PropertyType = SchemaDataType.String;

            if (_propertyDialog.ShowDialog(this) == DialogResult.OK)
            {
                var propertyName = _propertyDialog.View.PropertyName;
                var propertyType = _propertyDialog.View.PropertyType;

                var propertyNode = CreatePropertyNode(DataSourceTree.SelectedNode, propertyName, propertyType);

                DataSourceTree.SelectedNode = propertyNode;
            }
        }

        private static TreeNode CreatePropertyNode(TreeNode parent, string name, SchemaDataType type)
        {
            var propertyNode = parent.Nodes.Add(name, name, type.ToString(), type.ToString());
            propertyNode.Tag = type;
            return propertyNode;
        }

        // EDIT

        private void OnEditPropertyButtonClick(object sender, EventArgs e)
        {
            EditProperty();
        }

        private void OnEditPropertyMenuClick(object sender, EventArgs e)
        {
            EditProperty();
        }

        private void EditProperty()
        {
            var propertyNode = DataSourceTree.SelectedNode;

            _propertyDialog.View.PropertyName = propertyNode.Name;
            _propertyDialog.View.PropertyType = (SchemaDataType) propertyNode.Tag;

            if (_propertyDialog.ShowDialog(this) == DialogResult.OK)
            {
                var propertyName = _propertyDialog.View.PropertyName;
                var propertyType = _propertyDialog.View.PropertyType;

                // Объект или массив был изменен на скалярный тип
                if (propertyType != SchemaDataType.Object && propertyType != SchemaDataType.Array &&
                    propertyNode.Nodes.Count > 0)
                {
                    if (Resources.RemoveChildPropertiesQuestion.ShowQuestion())
                    {
                        propertyNode.Nodes.Clear();
                    }
                    else
                    {
                        return;
                    }
                }

                EditPropertyNode(propertyNode, propertyName, propertyType);
            }
        }

        private static void EditPropertyNode(TreeNode propertyNode, string name, SchemaDataType type)
        {
            propertyNode.Name = name;
            propertyNode.Text = name;
            propertyNode.ImageKey = type.ToString();
            propertyNode.SelectedImageKey = type.ToString();
            propertyNode.Tag = type;
        }

        // DELETE

        private void OnDeletePropertyButtonClick(object sender, EventArgs e)
        {
            DeleteProperty();
        }

        private void OnDeletePropertyMenuClick(object sender, EventArgs e)
        {
            DeleteProperty();
        }

        private void DeleteProperty()
        {
            var propertyNode = DataSourceTree.SelectedNode;

            if (Resources.DeletePropertyQuestion.ShowQuestion(propertyNode.Name))
            {
                propertyNode.Remove();
            }
        }

        // CUT

        private void OnCutPropertyButtonClick(object sender, EventArgs e)
        {
            CutProperty();
        }

        private void OnCutPropertyMenuClick(object sender, EventArgs e)
        {
            CutProperty();
        }

        private void CutProperty()
        {
            _isCutProperty = true;
            _clipboardPropertyNode = DataSourceTree.SelectedNode;
        }

        // COPY

        private void OnCopyPropertyButtonClick(object sender, EventArgs e)
        {
            CopyProperty();
        }

        private void OnCopyPropertyMenuClick(object sender, EventArgs e)
        {
            CopyProperty();
        }

        private void CopyProperty()
        {
            _isCutProperty = false;
            _clipboardPropertyNode = DataSourceTree.SelectedNode;
        }

        // PASTE

        private void OnPastePropertyButtonClick(object sender, EventArgs e)
        {
            PasteProperty();
        }

        private void OnPastePropertyMenuClick(object sender, EventArgs e)
        {
            PasteProperty();
        }

        private void PasteProperty()
        {
            var propertyNode = _clipboardPropertyNode;

            if (propertyNode != null)
            {
                var targetPropertyNode = DataSourceTree.SelectedNode;

                if (_isCutProperty)
                {
                    if (targetPropertyNode == propertyNode)
                    {
                        return;
                    }

                    if (IsChildNode(propertyNode, targetPropertyNode))
                    {
                        Resources.CannotMoveParentPropertyToChild.ShowWarning(targetPropertyNode.Name, propertyNode.Name);
                        return;
                    }

                    propertyNode.Remove();
                }
                else
                {
                    propertyNode = CloneTreeNode(propertyNode);
                }

                targetPropertyNode.Nodes.Add(propertyNode);

                propertyNode.ExpandAll();

                DataSourceTree.SelectedNode = propertyNode;

                _clipboardPropertyNode = null;
            }
        }

        private static TreeNode CloneTreeNode(TreeNode parent)
        {
            var clone = new TreeNode
            {
                Name = parent.Name,
                Text = parent.Text,
                ImageKey = parent.ImageKey,
                SelectedImageKey = parent.SelectedImageKey,
                Tag = parent.Tag
            };

            foreach (TreeNode child in parent.Nodes)
            {
                var cloneChild = CloneTreeNode(child);

                clone.Nodes.Add(cloneChild);
            }

            return clone;
        }

        private static bool IsChildNode(TreeNode parent, TreeNode child)
        {
            var isChild = false;

            while (child != null)
            {
                if (child == parent)
                {
                    isChild = true;
                    break;
                }

                child = child.Parent;
            }

            return isChild;
        }

        // SORT

        private void OnSortPropertiesButtonClick(object sender, EventArgs e)
        {
            SortProperties();
        }

        private void OnSortPropertiesMenuClick(object sender, EventArgs e)
        {
            SortProperties();
        }

        private void SortProperties()
        {
            var propertyNode = DataSourceTree.SelectedNode;

            SortProperties(propertyNode);

            propertyNode.ExpandAll();
        }

        private static void SortProperties(TreeNode parent)
        {
            if (parent.Nodes.Count > 1)
            {
                var sortedNodes = parent.Nodes.Cast<TreeNode>().OrderBy(n => n.Name).ToArray();

                parent.Nodes.Clear();
                parent.Nodes.AddRange(sortedNodes);

                foreach (var child in sortedNodes)
                {
                    SortProperties(child);
                }
            }
        }

        // MOVE UP

        private void OnMoveUpPropertyButtonClick(object sender, EventArgs e)
        {
            MoveUpProperty();
        }

        private void OnMoveUpPropertyMenuClick(object sender, EventArgs e)
        {
            MoveUpProperty();
        }

        private void MoveUpProperty()
        {
            DataSourceTree.SelectedNode.MoveUp();
        }

        // MOVE DOWN

        private void OnMoveDownPropertyButtonClick(object sender, EventArgs e)
        {
            MoveDownProperty();
        }

        private void OnMoveDownPropertyMenuClick(object sender, EventArgs e)
        {
            MoveDownProperty();
        }

        private void MoveDownProperty()
        {
            DataSourceTree.SelectedNode.MoveDown();
        }

        // TREE

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataSourceTree.SelectedNode = e.Node;
        }

        private void OnSelectedNodeChanged(object sender, TreeViewEventArgs e)
        {
            SetAllowedActions();
        }

        private void SetAllowedActions()
        {
            var selectedNode = DataSourceTree.SelectedNode;

            var allowEdit = AllowEdit && (selectedNode != null);
            var isNotRootNode = allowEdit && IsNotRootNode(selectedNode);
            var isContainerNode = allowEdit && IsContainerNode(selectedNode);

            DataSourceTree.ContextMenuStrip = allowEdit ? DataSourceTreeMenu : null;

            CreatePropertyButton.Enabled = isContainerNode;
            CreatePropertyMenuItem.Enabled = isContainerNode;

            EditPropertyButton.Enabled = isNotRootNode;
            EditPropertyMenuItem.Enabled = isNotRootNode;

            DeletePropertyButton.Enabled = isNotRootNode;
            DeletePropertyMenuItem.Enabled = isNotRootNode;

            CutPropertyButton.Enabled = isNotRootNode;
            CutPropertyMenuItem.Enabled = isNotRootNode;

            CopyPropertyButton.Enabled = isNotRootNode;
            CopyPropertyMenuItem.Enabled = isNotRootNode;

            PastePropertyButton.Enabled = isContainerNode && (_clipboardPropertyNode != null);
            PastePropertyMenuItem.Enabled = isContainerNode && (_clipboardPropertyNode != null);

            SortPropertiesButton.Enabled = isContainerNode;
            SortPropertiesMenuItem.Enabled = isContainerNode;

            MoveUpPropertyButton.Enabled = isNotRootNode;
            MoveUpPropertyMenuItem.Enabled = isNotRootNode;

            MoveDownPropertyButton.Enabled = isNotRootNode;
            MoveDownPropertyMenuItem.Enabled = isNotRootNode;
        }

        private static bool IsNotRootNode(TreeNode node)
        {
            return (node.Level > 0);
        }

        private static bool IsContainerNode(TreeNode node)
        {
            var nodeDataType = GetNodeDataType(node);
            return (nodeDataType == SchemaDataType.None || nodeDataType == SchemaDataType.Object ||
                    nodeDataType == SchemaDataType.Array);
        }

        private static SchemaDataType GetNodeDataType(TreeNode node)
        {
            return (node != null && node.Tag != null) ? (SchemaDataType) node.Tag : SchemaDataType.None;
        }

        // VALIDATE

        public override bool ValidateChildren()
        {
            return ValidateDataSourceTree(new List<string>(), DataSourceTree.Nodes[0], null);
        }

        private bool ValidateDataSourceTree(ICollection<string> properties, TreeNode parentNode, string parentPath)
        {
            var result = true;

            // Проверка, что объект содержит свойства
            if (parentNode.Nodes.Count > 0)
            {
                if (DataNestingLevel <= 0 || DataNestingLevel > parentNode.Level)
                {
                    foreach (TreeNode propertyNode in parentNode.Nodes)
                    {
                        var propertyName = propertyNode.Name;
                        var propertyType = (SchemaDataType) propertyNode.Tag;
                        var propertyPath = parentPath + "." + propertyName;

                        // Проверка уникальности имени на уровне дерева
                        if (properties.Contains(propertyPath) == false)
                        {
                            properties.Add(propertyPath);

                            if (propertyType == SchemaDataType.Object || propertyType == SchemaDataType.Array)
                            {
                                // Проверка дочерних свойств объекта или элементов массива
                                if (ValidateDataSourceTree(properties, propertyNode, propertyPath) == false)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Resources.PropertyAlreadyDeclared.ShowError(propertyNode.Name);
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    Resources.DataNestingLevelCannotExceed.ShowError(DataNestingLevel);
                    result = false;
                }
            }
            else
            {
                Resources.PropertyMustContainSubproperties.ShowError(parentNode.Name);
                result = false;
            }

            return result;
        }
    }
}