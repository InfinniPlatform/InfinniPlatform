using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для выбора SQL-таблицы.
    /// </summary>
    sealed partial class SqlTableSelectView : UserControl
    {
        private readonly ISqlMetadataProvider _metadataProvider;

        public SqlTableSelectView()
            : this(null)
        {
        }

        public SqlTableSelectView(ISqlMetadataProvider metadataProvider)
        {
            InitializeComponent();

            _metadataProvider = metadataProvider;

            Text = Resources.DataSourceTableSelectView;
        }

        // HANDLERS

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TablesEdit.SelectedNode = e.Node;
        }

        private void OnSelectDatabase(object sender, EventArgs e)
        {
            string connectionString;

            if (_metadataProvider.TryGetConnectionString(out connectionString))
            {
                ConnectionStringEdit.Text = connectionString;
                MainToolTip.SetToolTip(ConnectionStringEdit, connectionString);

                this.AsyncAction(() => _metadataProvider.GetTableNames(connectionString), FillTables,
                    () => FillTables(null));
            }
        }

        private void FillTables(IEnumerable<string> tables)
        {
            TablesEdit.Nodes.Clear();

            if (tables != null)
            {
                foreach (var table in tables)
                {
                    TablesEdit.Nodes.Add(table, table);
                }
            }
        }

        // METHODS

        public override bool ValidateChildren()
        {
            if (TablesEdit.SelectedNode == null)
            {
                Resources.SelectTable.ShowError();
                return false;
            }

            return true;
        }

        public void AsyncLoadDataSourceInfo(Control control, Action<DataSourceInfo> result)
        {
            var tableNode = TablesEdit.SelectedNode;

            if (tableNode != null)
            {
                var dataSourceInfo = tableNode.Tag as DataSourceInfo;

                if (dataSourceInfo != null)
                {
                    result(dataSourceInfo);
                }
                else
                {
                    var connectionString = ConnectionStringEdit.Text;

                    control.AsyncAction(() => _metadataProvider.GetTableDataSource(connectionString, tableNode.Name),
                        info =>
                        {
                            tableNode.Tag = info;
                            result(info);
                        });
                }
            }
            else
            {
                result(null);
            }
        }
    }
}