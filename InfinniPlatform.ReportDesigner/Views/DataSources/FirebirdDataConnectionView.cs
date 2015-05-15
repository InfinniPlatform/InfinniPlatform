using System;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	/// <summary>
	/// Представление для построения строки подключения к базе данных Firebird.
	/// </summary>
	sealed partial class FirebirdDataConnectionView : UserControl
	{
		public FirebirdDataConnectionView()
		{
			InitializeComponent();

			Text = Resources.FirebirdDataConnectionView;
		}


		public string ConnectionString
		{
			get
			{
				var builder = new FbConnectionStringBuilder
							  {
								  DataSource = "localhost",
								  Database = DatabaseFileEdit.Text,
								  UserID = UserNameEdit.Text,
								  Password = PasswordEdit.Text,
								  Charset = "UTF8",
								  Dialect = 3
							  };

				return builder.ToString();
			}
		}


		private void OnSelectDatabaseFile(object sender, EventArgs e)
		{
			if (DatabaseFileOpenDialog.ShowDialog(this) == DialogResult.OK)
			{
				DatabaseFileEdit.Text = DatabaseFileOpenDialog.FileName;
			}
		}


		public override bool ValidateChildren()
		{
			if (string.IsNullOrWhiteSpace(DatabaseFileEdit.Text))
			{
				Resources.SelectDatabaseFile.ShowError();
				DatabaseFileBtn.Focus();
				return false;
			}

			return true;
		}
	}
}