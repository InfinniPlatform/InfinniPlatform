using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Services;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для построения строки подключения к базе данных MSSQL.
    /// </summary>
    sealed partial class MsSqlDataConnectionView : UserControl
    {
        private readonly SqlMetadataService _metadataService = new SqlMetadataService();

        public MsSqlDataConnectionView()
        {
            InitializeComponent();

            Text = Resources.MsSqlDataConnectionView;
        }

        public string ConnectionString
        {
            get { return GetConnectionString(true); }
        }

        private string GetConnectionString(bool withDatabaseName)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerNameEdit.Text,
                IntegratedSecurity = UseWindowsAuthenticationBtn.Checked
            };

            if (UserNameEdit.Enabled)
            {
                builder.UserID = UserNameEdit.Text;
            }

            if (PasswordEdit.Enabled)
            {
                builder.Password = PasswordEdit.Text;
            }

            if (withDatabaseName && DatabaseNameEdit.Enabled)
            {
                builder.InitialCatalog = DatabaseNameEdit.Text;
            }

            if (DatabaseFileEdit.Enabled)
            {
                builder.AttachDBFilename = DatabaseFileEdit.Text;
            }

            if (DatabaseLogicalNameEdit.Enabled)
            {
                builder.InitialCatalog = DatabaseLogicalNameEdit.Text;
            }

            return builder.ToString();
        }

        private void OnDropDownServerNames(object sender, EventArgs e)
        {
            if (!Equals(ServerNameEdit.Tag, true))
            {
                OnRefreshServerNames(sender, e);
            }
        }

        private void OnRefreshServerNames(object sender, EventArgs e)
        {
            this.AsyncAction(() => _metadataService.GetServerNames(), FillServerNames, () => FillServerNames(null));
        }

        private void FillServerNames(IEnumerable<string> serverNames)
        {
            ServerNameEdit.Items.Clear();

            if (serverNames != null)
            {
                foreach (var serverName in serverNames)
                {
                    ServerNameEdit.Items.Add(serverName);
                }
            }

            ServerNameEdit.Tag = true;
        }

        private void OnDropDownDatabaseNames(object sender, EventArgs e)
        {
            var connectionString = GetConnectionString(false);

            this.AsyncAction(() => _metadataService.GetDatabaseNames(connectionString), FillDatabaseNames,
                () => FillDatabaseNames(null));
        }

        private void FillDatabaseNames(IEnumerable<string> databaseNames)
        {
            DatabaseNameEdit.Items.Clear();

            if (databaseNames != null)
            {
                foreach (var serverName in databaseNames)
                {
                    DatabaseNameEdit.Items.Add(serverName);
                }
            }
        }

        private void OnSelectDatabaseFile(object sender, EventArgs e)
        {
            if (DatabaseFileOpenDialog.ShowDialog(this) == DialogResult.OK)
            {
                DatabaseFileEdit.Text = DatabaseFileOpenDialog.FileName;
            }
        }

        private void OnSelectLogOnMode(object sender, EventArgs e)
        {
            var sqlAuthentication = UseSqlServerAuthenticationBtn.Checked;
            UserNameLabel.Enabled = sqlAuthentication;
            UserNameEdit.Enabled = sqlAuthentication;
            PasswordLabel.Enabled = sqlAuthentication;
            PasswordEdit.Enabled = sqlAuthentication;
        }

        private void OnSelectDatabase(object sender, EventArgs e)
        {
            var attachDatabase = AttachDatabaseFileBtn.Checked;
            DatabaseNameEdit.Enabled = !attachDatabase;
            DatabaseFileEdit.Enabled = attachDatabase;
            DatabaseFileBtn.Enabled = attachDatabase;
            DatabaseLogicalNameEdit.Enabled = attachDatabase;
        }

        public override bool ValidateChildren()
        {
            if (string.IsNullOrWhiteSpace(ServerNameEdit.Text))
            {
                Resources.ServerNameCannotBeEmptyOrWhitesp.ShowError();
                ServerNameEdit.Focus();
                return false;
            }

            if (UserNameEdit.Enabled && string.IsNullOrWhiteSpace(UserNameEdit.Text))
            {
                Resources.UserNameCannotBeEmptyOrWhitespace.ShowError();
                UserNameEdit.Focus();
                return false;
            }

            if (PasswordEdit.Enabled && string.IsNullOrWhiteSpace(PasswordEdit.Text))
            {
                Resources.PasswordCannotBeEmptyOrWhitespace.ShowError();
                PasswordEdit.Focus();
                return false;
            }

            if (DatabaseNameEdit.Enabled && string.IsNullOrWhiteSpace(DatabaseNameEdit.Text))
            {
                Resources.DatabaseNameCannotBeEmptyOrWhitespace.ShowError();
                DatabaseNameEdit.Focus();
                return false;
            }

            if (DatabaseFileEdit.Enabled && string.IsNullOrWhiteSpace(DatabaseFileEdit.Text))
            {
                Resources.SelectDatabaseFile.ShowError();
                DatabaseFileBtn.Focus();
                return false;
            }

            if (DatabaseLogicalNameEdit.Enabled && string.IsNullOrWhiteSpace(DatabaseLogicalNameEdit.Text))
            {
                Resources.DatabaseNameCannotBeEmptyOrWhitespace.ShowError();
                DatabaseLogicalNameEdit.Focus();
                return false;
            }

            return true;
        }
    }
}