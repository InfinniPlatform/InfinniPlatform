using System.Collections.Generic;

namespace InfinniPlatform.Sql
{
    /// <summary>
    ///     Исполнитель SQL-команд.
    /// </summary>
    public interface ISqlQueryExecutor
    {
        /// <summary>
        ///     Получить данные по указанному запросу.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <returns>Строка подключения.</returns>
        string GetConnectionString(string databaseName);

        /// <summary>
        ///     Получить данные, удовлетворяющие запросу.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Данные.</returns>
        IEnumerable<T> Query<T>(string databaseName, SqlQueryObject query, int? timeout = null);

        /// <summary>
        ///     Получить данные по указанному запросу.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Данные.</returns>
        IEnumerable<dynamic> Query(string databaseName, SqlQueryObject query, int? timeout = null);

        /// <summary>
        ///     Выполнить указанный запрос.
        /// </summary>
        /// <param name="databaseName">Наименование строки подключения.</param>
        /// <param name="query">Объект запроса.</param>
        /// <param name="timeout">Таймаут запроса.</param>
        /// <returns>Количество обработанных записей.</returns>
        int Execute(string databaseName, SqlQueryObject query, int? timeout = null);
    }
}