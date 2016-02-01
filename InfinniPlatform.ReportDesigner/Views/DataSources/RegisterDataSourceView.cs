using System;
using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.QueryDesigner.Forms;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для отображения и редактирования источника данных, представленного в виде регистра системы.
    /// </summary>
    sealed partial class RegisterDataSourceView : UserControl
    {
        public RegisterDataSourceView()
        {
            InitializeComponent();

            Text = Resources.RegisterDataSourceView;
        }

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
                    Provider = new RegisterDataProviderInfo
                    {
                        Body = BodyEdit.Text
                    }
                };
            }
            set
            {
                if (value != null)
                {
                    NameEdit.Text = value.Name;
                    DataSchemaEdit.DataSourceSchema = value.Schema;

                    var provider = value.Provider as RegisterDataProviderInfo;

                    if (provider != null)
                    {
                        BodyEdit.Text = provider.Body;
                    }
                }
                else
                {
                    NameEdit.Text = null;
                    BodyEdit.Text = null;
                    DataSchemaEdit.DataSourceSchema = null;
                }
            }
        }

        private void OnNameChanged(object sender, EventArgs e)
        {
            DataSchemaEdit.DataSourceName = NameEdit.Text;
        }

        private void OnImportDataSchema(object sender, EventArgs e)
        {
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

            if (DataSchemaEdit.ValidateChildren() == false)
            {
                MainTabControl.SelectTab(DataSchemaTabPage);
                DataSchemaEdit.Focus();
                return false;
            }

            return true;
        }

        private void ButtonEditQuery_Click(object sender, EventArgs e)
        {
            var queryDesigner = new QueryDesignerForm();
            if (queryDesigner.ShowDialog() == DialogResult.OK)
            {
                BodyEdit.Text = queryDesigner.Query;
                DataSchemaEdit.DataSourceSchema = BuildDataSchema(queryDesigner.SelectObjects);
            }
        }

        private DataSchema BuildDataSchema(IEnumerable<SchemaObject> selectObjects)
        {
            return new SchemaCreator().BuildSchema(selectObjects);
        }
    }
}