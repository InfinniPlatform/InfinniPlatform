using FastReport;
using FastReport.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	static class PropertyMapper
	{
		public static DataSourceBase FindDataSource(Report report, string dataSourceName, string collectionPath)
		{
			Column dataSource = report.Dictionary.DataSources.FindByName(dataSourceName);

			if (string.IsNullOrEmpty(collectionPath) == false)
			{
				var path = NormalizePath(collectionPath).Split('.');

				foreach (var propertyName in path)
				{
					dataSource = dataSource.Columns.FindByName(propertyName);

					if (dataSource == null)
					{
						break;
					}
				}
			}

			return dataSource as DataSourceBase;
		}

		public static string GetPropertyExpression(string dataSourceName, string propertyPath)
		{
			return string.Format("[{0}.{1}]", dataSourceName, NormalizePath(propertyPath));
		}

		private static string NormalizePath(string propertyPath)
		{
			return string.IsNullOrEmpty(propertyPath) ? propertyPath : propertyPath.Replace(".$", "");
		}
	}
}