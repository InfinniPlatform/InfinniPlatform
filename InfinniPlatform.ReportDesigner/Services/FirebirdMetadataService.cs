using System.Collections.Generic;

using FirebirdSql.Data.FirebirdClient;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Services
{
	sealed class FirebirdMetadataService
	{
		public IEnumerable<string> GetTableNames(string connectionString)
		{
			var result = new List<string>();

			using (var connection = new FbConnection(connectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandText = @"
						select rdb$relation_name as Name
						from rdb$relations
						where rdb$view_blr is null and (rdb$system_flag is null or rdb$system_flag = 0)
						order by Name asc;";

					connection.Open();

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(((string)reader["Name"]).Trim());
						}
					}
				}
			}

			return result;
		}

		public DataSourceInfo GetTableDataSource(string connectionString, string tableName)
		{
			var properties = new Dictionary<string, DataSchema>();

			using (var connection = new FbConnection(connectionString))
			{
				using (var command = connection.CreateCommand())
				{
					command.CommandText = @"
						select
							tables.rdb$field_name as name,
							case columns.rdb$field_type
								when 7 then
									case columns.rdb$field_sub_type
										when 0 then 'Integer'
										else 'Float'
									end
								when 8 then
									case columns.rdb$field_sub_type
										when 0 then 'Integer'
										else 'Float'
									end
								when 10 then 'Float'
								when 12 then 'Datetime'
								when 13 then 'Datetime'
								when 14 then 'String'
								when 16 then
									case columns.rdb$field_sub_type
										when 0 then 'Integer'
										else 'Float'
									end
								when 27 then 'Float'
								when 35 then 'Integer'
								when 37 then 'String'
								when 40 then 'String'
								else 'None'
							end as type
						from rdb$relation_fields tables
						join rdb$fields columns on (columns.rdb$field_name = tables.rdb$field_source)
						where (tables.rdb$relation_name = @tableName) and (coalesce(tables.rdb$system_flag, 0) = 0)
						order by tables.rdb$field_position;";

					command.Parameters.Add(new FbParameter("tableName", tableName));

					connection.Open();

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var propertyName = ((string)reader["Name"]).Trim();
							var propertyType = ((string)reader["Type"]).Trim();

							properties.Add(propertyName, new DataSchema { Type = GetDataType(propertyType) });
						}
					}
				}
			}

			return new DataSourceInfo
				   {
					   Name = tableName,

					   Schema = new DataSchema
								{
									Type = SchemaDataType.Object,
									Properties = properties
								},

					   Provider = new SqlDataProviderInfo
								  {
									  ServerType = SqlServerType.Firebird,
									  ConnectionString = connectionString,
									  SelectCommand = string.Format("select * from \"{0}\"", tableName)
								  }
				   };
		}

		private static SchemaDataType GetDataType(string sqlType)
		{
			SchemaDataType result;

			if (DataTypes.TryGetValue(sqlType, out result) == false)
			{
				result = SchemaDataType.String;
			}

			return result;
		}


		private static readonly Dictionary<string, SchemaDataType> DataTypes
			= new Dictionary<string, SchemaDataType>
			  {
				  { "String", SchemaDataType.String },
				  { "Float", SchemaDataType.Float },
				  { "Integer", SchemaDataType.Integer },
				  { "Boolean", SchemaDataType.Boolean },
				  { "DateTime", SchemaDataType.DateTime }
			  };
	}
}