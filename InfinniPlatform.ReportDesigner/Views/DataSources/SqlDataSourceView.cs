using System;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для отображения и редактирования источника данных, представленного в виде реляционной базы данных.
    /// </summary>
    sealed partial class SqlDataSourceView : UserControl
    {
        private SqlServerType _serverType;

        public SqlDataSourceView()
        {
            InitializeComponent();
            Text = Resources.SqlDataSourceView;
        }

        /// <summary>
        ///     Представление для выбора схемы данных.
        /// </summary>
        public DialogView<SqlTableSelectView> DataSchemaSelectDialog { get; set; }

        /// <summary>
        ///     Информация о источнике данных.
        /// </summary>
        public DataSourceInfo DataSourceInfo
        {
            get
            {
                return new DataSourceInfo
                {
                    Name = NameEdit.Text,
                    Schema = DataSchemaEdit.DataSourceSchema,
                    Provider = new SqlDataProviderInfo
                    {
                        ServerType = _serverType,
                        CommandTimeout = 1000*(int) CommandTimeoutEdit.Value,
                        ConnectionString = ConnectionStringEdit.Text,
                        SelectCommand = SelectCommandEdit.Text
                    }
                };
            }
            set
            {
                if (value != null)
                {
                    NameEdit.Text = value.Name;
                    DataSchemaEdit.DataSourceSchema = value.Schema;

                    var provider = value.Provider as SqlDataProviderInfo;

                    if (provider != null)
                    {
                        _serverType = provider.ServerType;
                        CommandTimeoutEdit.Value = (provider.CommandTimeout > 0)
                            ? (decimal) provider.CommandTimeout/1000
                            : 0;
                        ConnectionStringEdit.Text = provider.ConnectionString;
                        SelectCommandEdit.Text = provider.SelectCommand;
                    }
                }
                else
                {
                    NameEdit.Text = null;
                    DataSchemaEdit.DataSourceSchema = null;
                    CommandTimeoutEdit.Value = 0;
                    ConnectionStringEdit.Text = null;
                    SelectCommandEdit.Text = null;
                }
            }
        }

        private void OnNameChanged(object sender, EventArgs e)
        {
            DataSchemaEdit.DataSourceName = NameEdit.Text;
        }

        private void OnImportDataSchema(object sender, EventArgs e)
        {
            if (DataSchemaSelectDialog != null && DataSchemaSelectDialog.ShowDialog(this) == DialogResult.OK)
            {
                DataSchemaSelectDialog.View.AsyncLoadDataSourceInfo(this,
                    dataSourceInfo => DataSourceInfo = dataSourceInfo);
            }
        }

        public void ResetDefaults()
        {
            MainTabControl.SelectTab(GeneralTabPage);
            DataSourceInfo = null;
        }

        public override bool ValidateChildren()
        {
            if (NameEdit.Text.IsValidName() == false)
            {
                Resources.EnterValidName.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                NameEdit.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ConnectionStringEdit.Text))
            {
                Resources.ConnectionStringCannotBeNullOrWhiteSpace.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                ConnectionStringEdit.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectCommandEdit.Text))
            {
                Resources.SelectCommandCannotBeNullOrWhiteSpace.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                SelectCommandEdit.Focus();
                return false;
            }

            if (DataSchemaEdit.ValidateChildren() == false)
            {
                MainTabControl.SelectTab(DataSchemaTabPage);
                DataSchemaEdit.Focus();
                return false;
            }

            return true;
        }
    }
}