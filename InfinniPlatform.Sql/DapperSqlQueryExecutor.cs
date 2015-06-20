using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Sql
{
    /// <summary>
    ///     Исполнитель SQL-команд на базе Dapper.
    /// </summary>
    public sealed class DapperSqlQueryExecutor : ISqlQueryExecutor
    {
        /// <summary>
        ///     Получить данные по указанному запросу.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <returns>Строка подключения.</returns>
        public string GetConnectionString(string databaseName)
        {
            return ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
        }

        /// <summary>
        ///     Получить данные, удовлетворяющие запросу.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Данные.</returns>
        public IEnumerable<T> Query<T>(string databaseName, SqlQueryObject query, int? timeout = null)
        {
            return ExecuteConnection(databaseName, c => c.Query<T>(query.Text, query.Params, commandTimeout: timeout));
        }

        /// <summary>
        ///     Получить данные по указанному запросу.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Данные.</returns>
        public IEnumerable<dynamic> Query(string databaseName, SqlQueryObject query, int? timeout = null)
        {
            return
                ExecuteConnection(databaseName,
                    c =>
                        c.Query(query.Text, query.Params, commandTimeout: timeout)
                            .Select(r => DynamicWrapperExtensions.ToDynamic(r))).ToList();
        }

        /// <summary>
        ///     Выполнить указанный запрос.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Количество обработанных записей.</returns>
        public int Execute(string databaseName, SqlQueryObject query, int? timeout = null)
        {
            return ExecuteConnection(databaseName, c => c.Execute(query.Text, query.Params, commandTimeout: timeout));
        }

        public TResult ExecuteConnection<TResult>(string databaseName, Func<IDbConnection, TResult> action)
        {
            var connectionString = GetConnectionString(databaseName);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                return action(connection);
            }
        }
    }
}