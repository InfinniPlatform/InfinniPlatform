using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Services;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	/// <summary>
	/// Провайдер метаданных базы данных MSSQL.
	/// </summary>
	sealed class MsSqlMetadataProvider : ISqlMetadataProvider
	{
		private readonly SqlMetadataService _metadataService
			= new SqlMetadataService();

		private readonly DialogView<MsSqlDataConnectionView> _connectionDialog
			= new DialogView<MsSqlDataConnectionView>();


		public bool TryGetConnectionString(out string connectionString)
		{
			if (_connectionDialog.ShowDialog() == DialogResult.OK)
			{
				connectionString = _connectionDialog.View.ConnectionString;
				return true;
			}

			connectionString = null;
			return false;
		}

		public IEnumerable<string> GetTableNames(string connectionString)
		{
			return _metadataService.GetTableNames(connectionString);
		}

		public DataSourceInfo GetTableDataSource(string connectionString, string tableName)
		{
			return _metadataService.GetTableDataSource(connectionString, tableName);
		}
	}
}