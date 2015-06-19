using System;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для отображения и редактирования источника данных, представленного в виде REST-сервиса.
    /// </summary>
    sealed partial class RestDataSourceView : UserControl
    {
        public RestDataSourceView()
        {
            InitializeComponent();

            Text = Resources.RestDataSourceView;
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
                    Provider = new RestDataProviderInfo
                    {
                        RequestTimeout = 1000*(int) RequestTimeoutEdit.Value,
                        RequestUri = RequestUriEdit.Text,
                        Method = MethodEdit.Text,
                        ContentType = ContentTypeEdit.Text,
                        AcceptType = AcceptTypeEdit.Text,
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

                    var provider = value.Provider as RestDataProviderInfo;

                    if (provider != null)
                    {
                        RequestTimeoutEdit.Value = (provider.RequestTimeout > 0)
                            ? (decimal) provider.RequestTimeout/1000
                            : 0;
                        RequestUriEdit.Text = provider.RequestUri;
                        MethodEdit.Text = provider.Method;
                        ContentTypeEdit.Text = provider.ContentType;
                        AcceptTypeEdit.Text = provider.AcceptType;
                        BodyEdit.Text = provider.Body;
                    }
                }
                else
                {
                    NameEdit.Text = null;
                    DataSchemaEdit.DataSourceSchema = null;
                    RequestTimeoutEdit.Value = 0;
                    RequestUriEdit.Text = null;
                    MethodEdit.Text = null;
                    ContentTypeEdit.Text = null;
                    AcceptTypeEdit.Text = null;
                    BodyEdit.Text = null;
                }
            }
        }

        private void OnNameChanged(object sender, EventArgs e)
        {
            DataSchemaEdit.DataSourceName = NameEdit.Text;
        }

        private void OnMethodChanged(object sender, EventArgs e)
        {
            var enableContent = (MethodEdit.Text == "POST" || MethodEdit.Text == "PUT");
            ContentTypeEdit.Enabled = enableContent;
            BodyEdit.ReadOnly = !enableContent;
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

            if (string.IsNullOrWhiteSpace(RequestUriEdit.Text))
            {
                Resources.RequestUriCannotBeNullOrWhiteSpace.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                RequestUriEdit.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(MethodEdit.Text))
            {
                Resources.SelectMethod.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                MethodEdit.Focus();
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