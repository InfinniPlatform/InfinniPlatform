using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using FastReport;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.DataSources;
using InfinniPlatform.ReportDesigner.Views.Events;
using InfinniPlatform.ReportDesigner.Views.Parameters;
using InfinniPlatform.ReportDesigner.Views.Totals;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    /// <summary>
    ///     Представление для отображения и редактирования источников данных, параметров и итогов отчета.
    /// </summary>
    sealed partial class ReportDataView : UserControl
    {
        private bool _allowEdit;
        private ICollection<DataSourceInfo> _dataSources;
        private ICollection<ParameterInfo> _parameters;
        private ICollection<TotalInfo> _totals;
        public Func<IEnumerable<DesignerDataBand>> GetDataBandsFunc;
        public Func<IEnumerable<DesignerPrintBand>> GetPrintBandsFunc;
        private readonly DataSourceWizard _dataSourceDialog;
        private readonly DialogView<ParameterView> _parameterDialog;
        private readonly DialogView<TotalView> _totalDialog;

        public ReportDataView()
        {
            InitializeComponent();

            _dataSourceDialog = new DataSourceWizard();
            _parameterDialog = new DialogView<ParameterView>();
            _totalDialog = new DialogView<TotalView>();

            AllowEdit = false;
        }

        /// <summary>
        ///     Шаблон отчета FastReport.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Report Report { get; set; }

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
        ///     Источники данных отчета.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<DataSourceInfo> DataSources
        {
            get
            {
                if (_dataSources == null)
                {
                    _dataSources = new List<DataSourceInfo>();
                }

                return _dataSources;
            }
            set
            {
                _dataSources = value;

                RefreshDataSources();
                SetAllowedActions();
            }
        }

        /// <summary>
        ///     Параметры отчета.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<ParameterInfo> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new List<ParameterInfo>();
                }

                return _parameters;
            }
            set
            {
                _parameters = value;

                RefreshParameters();
                SetAllowedActions();
            }
        }

        /// <summary>
        ///     Итоги отчета.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection<TotalInfo> Totals
        {
            get
            {
                if (_totals == null)
                {
                    _totals = new List<TotalInfo>();
                }

                return _totals;
            }
            set
            {
                _totals = value;

                RefreshTotals();
                SetAllowedActions();
            }
        }

        // DATA SOURCES

        public event CreatedEventHandler<DataSourceInfo> DataSourceCreated;
        public event ChangedEventHandler<DataSourceInfo> DataSourceChanged;
        public event DeletedEventHandler<DataSourceInfo> DataSourceDeleted;

        private void OnCreateDataSource(object sender, EventArgs e)
        {
            CreateDataSource();
        }

        public void CreateDataSource()
        {
            if (AllowEdit)
            {
                _dataSourceDialog.DataSourceInfo = null;

                if (_dataSourceDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var newDataSourceInfo = _dataSourceDialog.DataSourceInfo;

                    DataSources.Add(newDataSourceInfo);

                    UpdateDataSourceNode(newDataSourceInfo, null);

                    InvokeDataSourceCreated(newDataSourceInfo);
                }
            }
        }

        private void InvokeDataSourceCreated(DataSourceInfo dataSourceInfo)
        {
            if (DataSourceCreated != null)
            {
                DataSourceCreated(this, new ValueEventArgs<DataSourceInfo>(dataSourceInfo));
            }
        }

        private void OnEditDataSource(object sender, EventArgs e)
        {
            var dataSourceNode = DataTree.SelectedNode;

            if (dataSourceNode != null)
            {
                var dataSourceInfo = DataSources.FirstOrDefault(i => i.Name == dataSourceNode.Name);

                if (dataSourceInfo != null)
                {
                    _dataSourceDialog.DataSourceInfo = dataSourceInfo;

                    if (_dataSourceDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newDataSourceInfo = _dataSourceDialog.DataSourceInfo;

                        DataSources.Replace(dataSourceInfo, newDataSourceInfo);

                        UpdateDataSourceNode(newDataSourceInfo, dataSourceNode);

                        InvokeDataSourceChanged(dataSourceInfo, newDataSourceInfo);
                    }
                }
            }
        }

        private void InvokeDataSourceChanged(DataSourceInfo dataSourceInfo, DataSourceInfo newDataSourceInfo)
        {
            if (DataSourceChanged != null)
            {
                DataSourceChanged(this, new ChangedEventArgs<DataSourceInfo>(dataSourceInfo, newDataSourceInfo));
            }
        }

        private void OnDeleteDataSource(object sender, EventArgs e)
        {
            var dataSourceNode = DataTree.SelectedNode;

            if (dataSourceNode != null)
            {
                var dataSourceInfo = DataSources.FirstOrDefault(i => i.Name == dataSourceNode.Name);

                if (dataSourceInfo != null)
                {
                    if (Resources.DeleteDataSourceQuestion.ShowQuestion(dataSourceNode.Name))
                    {
                        DataSources.Remove(dataSourceInfo);

                        dataSourceNode.Remove();

                        InvokeDataSourceDeleted(dataSourceInfo);
                    }
                }
            }
        }

        private void InvokeDataSourceDeleted(DataSourceInfo dataSourceInfo)
        {
            if (DataSourceDeleted != null)
            {
                DataSourceDeleted(this, new ValueEventArgs<DataSourceInfo>(dataSourceInfo));
            }
        }

        private void RefreshDataSources()
        {
            DataTree.Nodes["DataSources"].Nodes.Clear();

            foreach (var dataSourceInfo in DataSources)
            {
                UpdateDataSourceNode(dataSourceInfo, null);
            }

            DataTree.SelectedNode = null;
        }

        private void UpdateDataSourceNode(DataSourceInfo dataSourceInfo, TreeNode dataSourceNode)
        {
            if (dataSourceNode != null)
            {
                dataSourceNode.Nodes.Clear();
            }
            else
            {
                dataSourceNode = CreateNode(DataTree.Nodes["DataSources"], dataSourceInfo.Name, "DataSource",
                    DataSourceMenu);
            }

            if (dataSourceInfo.Schema != null)
            {
                RenderDataSourceSchemaTree(dataSourceNode, dataSourceInfo.Schema);
            }

            dataSourceNode.Name = dataSourceInfo.Name;
            dataSourceNode.Text = dataSourceNode.Name;

            dataSourceNode.Collapse();
            dataSourceNode.Parent.Expand();

            DataTree.SelectedNode = dataSourceNode;
        }

        private static void RenderDataSourceSchemaTree(TreeNode parent, DataSchema dataSchema)
        {
            foreach (var property in dataSchema.Properties)
            {
                var propertyName = property.Key;
                var propertySchema = property.Value;
                var propertyType = propertySchema.Type;

                var propertyNode = CreateNode(parent, propertyName, propertyType.ToString());

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

        // PARAMETERS

        public event CreatedEventHandler<ParameterInfo> ParameterCreated;
        public event ChangedEventHandler<ParameterInfo> ParameterChanged;
        public event DeletedEventHandler<ParameterInfo> ParameterDeleted;

        private void OnCreateParameter(object sender, EventArgs e)
        {
            CreateParameter();
        }

        public void CreateParameter()
        {
            if (AllowEdit)
            {
                _parameterDialog.View.DataSources = DataSources;
                _parameterDialog.View.ParameterInfo = null;

                if (_parameterDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var newParameterInfo = _parameterDialog.View.ParameterInfo;

                    Parameters.Add(newParameterInfo);

                    UpdateParameterNode(newParameterInfo, null);

                    InvokeParameterCreated(newParameterInfo);
                }
            }
        }

        private void InvokeParameterCreated(ParameterInfo parameterInfo)
        {
            if (ParameterCreated != null)
            {
                ParameterCreated(this, new ValueEventArgs<ParameterInfo>(parameterInfo));
            }
        }

        private void OnEditParameter(object sender, EventArgs e)
        {
            var parameterNode = DataTree.SelectedNode;

            if (parameterNode != null)
            {
                var parameterInfo = Parameters.FirstOrDefault(i => i.Name == parameterNode.Name);

                if (parameterInfo != null)
                {
                    _parameterDialog.View.DataSources = DataSources;
                    _parameterDialog.View.ParameterInfo = parameterInfo;

                    if (_parameterDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newParameterInfo = _parameterDialog.View.ParameterInfo;

                        Parameters.Replace(parameterInfo, newParameterInfo);

                        UpdateParameterNode(newParameterInfo, parameterNode);

                        InvokeParameterChanged(parameterInfo, newParameterInfo);
                    }
                }
            }
        }

        private void InvokeParameterChanged(ParameterInfo parameterInfo, ParameterInfo newParameterInfo)
        {
            if (ParameterChanged != null)
            {
                ParameterChanged(this, new ChangedEventArgs<ParameterInfo>(parameterInfo, newParameterInfo));
            }
        }

        private void OnDeleteParameter(object sender, EventArgs e)
        {
            var parameterNode = DataTree.SelectedNode;

            if (parameterNode != null)
            {
                var parameterInfo = Parameters.FirstOrDefault(i => i.Name == parameterNode.Name);

                if (parameterInfo != null)
                {
                    if (Resources.DeleteParameterQuestion.ShowQuestion(parameterNode.Name))
                    {
                        Parameters.Remove(parameterInfo);

                        parameterNode.Remove();

                        InvokeParameterDeleted(parameterInfo);
                    }
                }
            }
        }

        private void InvokeParameterDeleted(ParameterInfo parameterInfo)
        {
            if (ParameterDeleted != null)
            {
                ParameterDeleted(this, new ValueEventArgs<ParameterInfo>(parameterInfo));
            }
        }

        private void RefreshParameters()
        {
            DataTree.Nodes["Parameters"].Nodes.Clear();

            foreach (var parameterInfo in Parameters)
            {
                UpdateParameterNode(parameterInfo, null);
            }

            DataTree.SelectedNode = null;
        }

        private void UpdateParameterNode(ParameterInfo parameterInfo, TreeNode parameterNode)
        {
            if (parameterNode == null)
            {
                parameterNode = CreateNode(DataTree.Nodes["Parameters"], parameterInfo.Name, "Parameter", ParameterMenu);
            }

            parameterNode.Name = parameterInfo.Name;
            parameterNode.Text = parameterInfo.Name;
            parameterNode.ImageKey = parameterInfo.Type.ToString();
            parameterNode.SelectedImageKey = parameterInfo.Type.ToString();

            parameterNode.Parent.Expand();

            DataTree.SelectedNode = parameterNode;
        }

        // TOTALS

        public event CreatedEventHandler<TotalInfo> TotalCreated;
        public event ChangedEventHandler<TotalInfo> TotalChanged;
        public event DeletedEventHandler<TotalInfo> TotalDeleted;

        private void OnCreateTotal(object sender, EventArgs e)
        {
            CreateTotal();
        }

        public void CreateTotal()
        {
            if (AllowEdit)
            {
                _totalDialog.View.Report = Report;
                _totalDialog.View.DataBands = GetDataBands();
                _totalDialog.View.PrintBands = GetPrintBands();
                _totalDialog.View.TotalInfo = null;

                if (_totalDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var newTotalInfo = _totalDialog.View.TotalInfo;

                    Totals.Add(newTotalInfo);

                    UpdateTotalNode(newTotalInfo, null);

                    InvokeTotalCreated(newTotalInfo);
                }
            }
        }

        private void InvokeTotalCreated(TotalInfo totalInfo)
        {
            if (TotalCreated != null)
            {
                TotalCreated(this, new ValueEventArgs<TotalInfo>(totalInfo));
            }
        }

        private void OnEditTotal(object sender, EventArgs e)
        {
            var totalNode = DataTree.SelectedNode;

            if (totalNode != null)
            {
                var totalInfo = Totals.FirstOrDefault(i => i.Name == totalNode.Name);

                if (totalInfo != null)
                {
                    _totalDialog.View.Report = Report;
                    _totalDialog.View.DataBands = GetDataBands();
                    _totalDialog.View.PrintBands = GetPrintBands();
                    _totalDialog.View.TotalInfo = totalInfo;

                    if (_totalDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var newTotalInfo = _totalDialog.View.TotalInfo;

                        Totals.Replace(totalInfo, newTotalInfo);

                        UpdateTotalNode(newTotalInfo, totalNode);

                        InvokeTotalChanged(totalInfo, newTotalInfo);
                    }
                }
            }
        }

        private void InvokeTotalChanged(TotalInfo totalInfo, TotalInfo newTotalInfo)
        {
            if (TotalChanged != null)
            {
                TotalChanged(this, new ChangedEventArgs<TotalInfo>(totalInfo, newTotalInfo));
            }
        }

        private void OnDeleteTotal(object sender, EventArgs e)
        {
            var totalNode = DataTree.SelectedNode;

            if (totalNode != null)
            {
                var totalInfo = Totals.FirstOrDefault(i => i.Name == totalNode.Name);

                if (totalInfo != null)
                {
                    if (Resources.DeleteTotalQuestion.ShowQuestion(totalNode.Name))
                    {
                        Totals.Remove(totalInfo);

                        totalNode.Remove();

                        InvokeTotalDeleted(totalInfo);
                    }
                }
            }
        }

        private void InvokeTotalDeleted(TotalInfo totalInfo)
        {
            if (TotalDeleted != null)
            {
                TotalDeleted(this, new ValueEventArgs<TotalInfo>(totalInfo));
            }
        }

        private void RefreshTotals()
        {
            DataTree.Nodes["Totals"].Nodes.Clear();

            foreach (var totalInfo in Totals)
            {
                UpdateTotalNode(totalInfo, null);
            }

            DataTree.SelectedNode = null;
        }

        private void UpdateTotalNode(TotalInfo totalInfo, TreeNode totalNode)
        {
            if (totalNode == null)
            {
                totalNode = CreateNode(DataTree.Nodes["Totals"], totalInfo.Name, "Total", TotalMenu);
            }

            totalNode.Name = totalInfo.Name;
            totalNode.Text = totalInfo.Name;

            totalNode.Parent.Expand();

            DataTree.SelectedNode = totalNode;
        }

        private IEnumerable<DesignerDataBand> GetDataBands()
        {
            return (GetDataBandsFunc != null) ? GetDataBandsFunc() : null;
        }

        private IEnumerable<DesignerPrintBand> GetPrintBands()
        {
            return (GetPrintBandsFunc != null) ? GetPrintBandsFunc() : null;
        }

        // MOVE

        private void OnMoveUp(object sender, EventArgs e)
        {
            var selectedNode = DataTree.SelectedNode;

            if (IsDataSourceNode(selectedNode))
            {
                var dataSourceInfo = DataSources.FirstOrDefault(i => i.Name == selectedNode.Name);
                DataSources.MoveUp(dataSourceInfo);
            }
            else if (IsParameterNode(selectedNode))
            {
                var parameterInfo = Parameters.FirstOrDefault(i => i.Name == selectedNode.Name);
                Parameters.MoveUp(parameterInfo);
            }
            else if (IsTotalNode(selectedNode))
            {
                var totalInfo = Totals.FirstOrDefault(i => i.Name == selectedNode.Name);
                Totals.MoveUp(totalInfo);
            }

            selectedNode.MoveUp();
        }

        private void OnMoveDown(object sender, EventArgs e)
        {
            var selectedNode = DataTree.SelectedNode;

            if (IsDataSourceNode(selectedNode))
            {
                var dataSourceInfo = DataSources.FirstOrDefault(i => i.Name == selectedNode.Name);
                DataSources.MoveDown(dataSourceInfo);
            }
            else if (IsParameterNode(selectedNode))
            {
                var parameterInfo = Parameters.FirstOrDefault(i => i.Name == selectedNode.Name);
                Parameters.MoveDown(parameterInfo);
            }
            else if (IsTotalNode(selectedNode))
            {
                var totalInfo = Totals.FirstOrDefault(i => i.Name == selectedNode.Name);
                Totals.MoveDown(totalInfo);
            }

            selectedNode.MoveDown();
        }

        // EVENTS

        private void OnMenuOpening(object sender, CancelEventArgs e)
        {
            e.Cancel = (AllowEdit == false);
        }

        private void OnActions(object sender, EventArgs e)
        {
            ActionsButton.ShowDropDown();
        }

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataTree.SelectedNode = e.Node;
        }

        private void OnSelectedNodeChanged(object sender, TreeViewEventArgs e)
        {
            SetAllowedActions();
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            var selectedNode = DataTree.SelectedNode;

            if (IsDataSourceNode(selectedNode))
            {
                OnEditDataSource(sender, e);
            }
            else if (IsParameterNode(selectedNode))
            {
                OnEditParameter(sender, e);
            }
            else if (IsTotalNode(selectedNode))
            {
                OnEditTotal(sender, e);
            }
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var selectedNode = DataTree.SelectedNode;

            if (IsDataSourceNode(selectedNode))
            {
                OnDeleteDataSource(sender, e);
            }
            else if (IsParameterNode(selectedNode))
            {
                OnDeleteParameter(sender, e);
            }
            else if (IsTotalNode(selectedNode))
            {
                OnDeleteTotal(sender, e);
            }
        }

        private void SetAllowedActions()
        {
            var selectedNode = DataTree.SelectedNode;
            var allowEditNode = AllowEdit && (selectedNode != null) && (selectedNode.Level == 1);

            // Main Menu
            CreateDataSourceButton.Enabled = AllowEdit;
            CreateParameterButton.Enabled = AllowEdit;
            CreateTotalButton.Enabled = AllowEdit;
            EditButton.Enabled = allowEditNode;
            DeleteButton.Enabled = allowEditNode;
            MoveUpButton.Enabled = allowEditNode;
            MoveDownButton.Enabled = allowEditNode;

            // Data Sources
            CreateDataSourceMenuItem.Enabled = AllowEdit;
            EditDataSourceMenuItem.Enabled = allowEditNode && IsDataSourceNode(selectedNode);
            DeleteDataSourceMenuItem.Enabled = allowEditNode && IsDataSourceNode(selectedNode);

            // Parameters
            CreateParameterMenuItem.Enabled = AllowEdit;
            EditParameterMenuItem.Enabled = allowEditNode && IsParameterNode(selectedNode);
            DeleteParameterMenuItem.Enabled = allowEditNode && IsParameterNode(selectedNode);

            // Totals
            CreateTotalMenuItem.Enabled = AllowEdit;
            EditTotalMenuItem.Enabled = allowEditNode && IsTotalNode(selectedNode);
            DeleteTotalMenuItem.Enabled = allowEditNode && IsTotalNode(selectedNode);
        }

        private bool IsDataSourceNode(TreeNode node)
        {
            return (node != null && node.Level == 1 && node.Parent == DataTree.Nodes["DataSources"]);
        }

        private bool IsParameterNode(TreeNode node)
        {
            return (node != null && node.Level == 1 && node.Parent == DataTree.Nodes["Parameters"]);
        }

        private bool IsTotalNode(TreeNode node)
        {
            return (node != null && node.Level == 1 && node.Parent == DataTree.Nodes["Totals"]);
        }

        private static TreeNode CreateNode(TreeNode parent, string name, string image, ContextMenuStrip menu = null)
        {
            var node = parent.Nodes.Add(name, name, image, image);
            node.ContextMenuStrip = menu;
            return node;
        }

        public override bool ValidateChildren()
        {
            if (DataSources.HasDuplicates(i => i.Name))
            {
                Resources.DataSourceNameMustBeUnique.ShowError();
                return false;
            }

            if (Parameters.HasDuplicates(i => i.Name))
            {
                Resources.ParameteNameMustBeUnique.ShowError();
                return false;
            }

            if (Totals.HasDuplicates(i => i.Name))
            {
                Resources.TotalNameMustBeUnique.ShowError();
                return false;
            }

            return true;
        }
    }
}