using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Services
{
    internal sealed class SqlMetadataService
    {
        private static readonly Dictionary<string, SchemaDataType> DataTypes
            = new Dictionary<string, SchemaDataType>
            {
                {"bit", SchemaDataType.Boolean},
                {"int", SchemaDataType.Integer},
                {"bigint", SchemaDataType.Integer},
                {"tinyint", SchemaDataType.Integer},
                {"smallint", SchemaDataType.Integer},
                {"timestamp", SchemaDataType.Integer},
                {"real", SchemaDataType.Float},
                {"float", SchemaDataType.Float},
                {"decimal", SchemaDataType.Float},
                {"numeric", SchemaDataType.Float},
                {"money", SchemaDataType.Float},
                {"smallmoney", SchemaDataType.Float},
                {"date", SchemaDataType.DateTime},
                {"datetime", SchemaDataType.DateTime},
                {"datetime2", SchemaDataType.DateTime},
                {"datetimeoffset", SchemaDataType.DateTime},
                {"smalldatetime", SchemaDataType.DateTime},
                {"time", SchemaDataType.DateTime}
            };

        public IEnumerable<string> GetServerNames()
        {
            var result = new List<string>();

            var servers = SqlDataSourceEnumerator.Instance.GetDataSources();

            foreach (DataRow info in servers.Rows)
            {
                var serverName = info["ServerName"] as string;
                var instanceName = info["InstanceName"] as string;

                result.Add(string.IsNullOrEmpty(instanceName) ? serverName : serverName + @"\" + instanceName);
            }

            return result;
        }

        public IEnumerable<string> GetDatabaseNames(string connectionString)
        {
            var result = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
						select Name = name
						from sys.databases
						order by Name asc";

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"] as string);
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> GetTableNames(string connectionString)
        {
            var result = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
						select Name = s.name + '.' + t.name
						from sys.tables as t with(nolock)
						left join sys.schemas as s with(nolock) on t.schema_id = s.schema_id
						order by Name asc";

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"] as string);
                        }
                    }
                }
            }

            return result;
        }

        public DataSourceInfo GetTableDataSource(string connectionString, string tableName)
        {
            var properties = new Dictionary<string, DataSchema>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
						select Name = c.name, Type = t.name
						from sys.columns as c with(nolock)
						left join sys.types as t with(nolock) on t.user_type_id = c.user_type_id
						where object_id = object_id(@tableName)";

                    command.Parameters.Add(new SqlParameter("tableName", tableName));

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var propertyName = ((string) reader["Name"]).Trim();
                            var propertyType = ((string) reader["Type"]).Trim();

                            properties.Add(propertyName, new DataSchema {Type = GetDataType(propertyType)});
                        }
                    }
                }
            }

            return new DataSourceInfo
            {
                Name = GetTableName(tableName),
                Schema = new DataSchema
                {
                    Type = SchemaDataType.Object,
                    Properties = properties
                },
                Provider = new SqlDataProviderInfo
                {
                    ServerType = SqlServerType.MsSqlServer,
                    ConnectionString = connectionString,
                    SelectCommand = string.Format("select * from {0}", tableName)
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

        private static string GetTableName(string sqlTable)
        {
            return sqlTable.Substring(sqlTable.IndexOf('.') + 1);
        }
    }
}