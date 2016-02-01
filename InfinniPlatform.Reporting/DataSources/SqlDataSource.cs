using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using FirebirdSql.Data.FirebirdClient;

using InfinniPlatform.Core.Settings;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataProviders;
using InfinniPlatform.Reporting.Properties;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
    /// <summary>
    /// Источник данных для реляционной базы данных.
    /// </summary>
    internal sealed class SqlDataSource : IDataSource
    {
        public SqlDataSource()
        {
            ProviderType = typeof(SqlDataProviderInfo);
        }

        public Type ProviderType { get; }

        public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
        {
            var dataProviderInfo = (SqlDataProviderInfo)dataSourceInfo.Provider;

            KeyValueDataProvider dataProvider;

            if (dataProviderInfo.ServerType == SqlServerType.MsSqlServer)
            {
                dataProvider = GetData<SqlConnection, SqlCommand, SqlParameter>(dataProviderInfo, parameterInfos, parameterValues);
            }
            else if (dataProviderInfo.ServerType == SqlServerType.Firebird)
            {
                dataProvider = GetData<FbConnection, FbCommand, FbParameter>(dataProviderInfo, parameterInfos, parameterValues);
            }
            else
            {
                throw new NotSupportedException(string.Format(Resources.SqlServerTypeIsNotSupported, dataProviderInfo.ServerType));
            }

            return dataProvider.ToJsonArray(dataSourceInfo.Schema);
        }

        private static KeyValueDataProvider GetData<TConnection, TCommand, TParam>(SqlDataProviderInfo dataProviderInfo,
                                                                                   IEnumerable<ParameterInfo> parameterInfos,
                                                                                   IDictionary<string, object> parameterValues)
            where TConnection : DbConnection, new()
            where TCommand : DbCommand, new()
            where TParam : DbParameter, new()
        {
            var data = new List<IDictionary<string, object>>();

            // Строка подключения может быть обределена в конфигурации приложения, либо жестко прошита в отчете
            var connectionString = ConnectionStrings.GetConnectionString(dataProviderInfo.ConnectionString) ?? dataProviderInfo.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(Resources.ConnectionStringCannotBeNullOrWhiteSpace);
            }

            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;

                using (var command = new TCommand())
                {
                    command.Connection = connection;
                    command.CommandTimeout = Math.Max(dataProviderInfo.CommandTimeout / 1000, 1);
                    command.CommandText = dataProviderInfo.SelectCommand;

                    // Если у отчета есть параметры, они будут добавлены в параметры запроса

                    if (parameterInfos != null)
                    {
                        foreach (var parameterInfo in parameterInfos)
                        {
                            var parameterName = parameterInfo.Name;

                            object parameterValue = null;

                            if (parameterValues != null)
                            {
                                parameterValues.TryGetValue(parameterName, out parameterValue);
                            }

                            command.Parameters.Add(new TParam
                                                   {
                                                       ParameterName = parameterName,
                                                       DbType = parameterInfo.Type.ToDbType(),
                                                       Value = parameterValue ?? DBNull.Value
                                                   });
                        }
                    }

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = ReadDataRecord(reader);

                            data.Add(record);
                        }
                    }
                }
            }

            return new KeyValueDataProvider(data);
        }

        public static IDictionary<string, object> ReadDataRecord(IDataRecord dataRecord)
        {
            var result = new Dictionary<string, object>(dataRecord.FieldCount);

            for (var i = 0; i < dataRecord.FieldCount; ++i)
            {
                var propertyValue = dataRecord[i];

                result.Add(dataRecord.GetName(i), (propertyValue != DBNull.Value) ? propertyValue : null);
            }

            return result;
        }
    }
}