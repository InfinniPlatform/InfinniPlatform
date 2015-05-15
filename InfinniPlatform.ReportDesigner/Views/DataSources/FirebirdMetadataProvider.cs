using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Services;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	/// <summary>
	/// Провайдер метаданных базы данных Firebird.
	/// </summary>
	sealed class FirebirdMetadataProvider : ISqlMetadataProvider
	{
		private readonly FirebirdMetadataService _metadataService
			= new FirebirdMetadataService();

		private readonly DialogView<FirebirdDataConnectionView> _connectionDialog
			= new DialogView<FirebirdDataConnectionView>();


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