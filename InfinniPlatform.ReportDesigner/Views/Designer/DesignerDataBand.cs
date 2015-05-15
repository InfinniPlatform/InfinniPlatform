using System.Collections.Generic;

using FastReport;
using FastReport.Data;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
	sealed class DesignerDataBand
	{
		public DesignerDataBand(DataBand dataBand)
		{
			Name = dataBand.Name;

			SetDataSource(dataBand.DataSource);
		}

		public string Name { get; private set; }
		public string DataSourceName { get; private set; }
		public string DataSourceFullName { get; private set; }

		private void SetDataSource(Column dataSource)
		{
			var dataSourcePath = new Stack<string>();

			while (dataSource != null)
			{
				var dataSourceName = dataSource.Name;

				dataSourcePath.Push(dataSourceName);

				DataSourceName = dataSourceName;

				dataSource = dataSource.Parent as Column;
			}

			DataSourceFullName = string.Join(".", dataSourcePath);
		}
	}
}