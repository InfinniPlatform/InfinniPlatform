using FastReport;
using FastReport.Data;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class DataSourceInfoBuilder : IReportObjectBuilder<DataSourceInfo>
	{
		public void BuildObject(IReportObjectBuilderContext context, DataSourceInfo template, object parent)
		{
			var report = (Report)parent;

			var dataSource = new JsonObjectDataSource { Name = template.Name, ReferenceName = template.Name };
			CreateDataSourceProperties(dataSource, template.Schema);

			report.Dictionary.DataSources.Add(dataSource);
		}


		private static void CreateDataSourceProperties(Column parent, DataSchema schema)
		{
			foreach (var property in schema.Properties)
			{
				var propertyName = property.Key;
				var propertySchema = property.Value;

				Column propertyColumn;

				if (propertySchema.Type == SchemaDataType.Object)
				{
					propertyColumn = new Column { Name = propertyName, ReferenceName = propertyName, DataType = typeof(object) };

					CreateDataSourceProperties(propertyColumn, propertySchema);
				}
				else if (propertySchema.Type == SchemaDataType.Array)
				{
					propertyColumn = new JsonObjectDataSource { Name = propertyName, ReferenceName = propertyName };

					CreateDataSourceProperties(propertyColumn, propertySchema.Items);
				}
				else
				{
					propertyColumn = new Column { Name = propertyName, ReferenceName = propertyName, DataType = propertySchema.Type.ToClrType() };
				}

				parent.Columns.Add(propertyColumn);
			}
		}
	}
}