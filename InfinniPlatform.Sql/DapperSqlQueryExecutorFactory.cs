using InfinniPlatform.Factories;

namespace InfinniPlatform.Sql
{
    /// <summary>
    ///     Фабрика для создания исполнителя SQL-команд.
    /// </summary>
    public sealed class DapperSqlQueryExecutorFactory : ISqlQueryExecutorFactory
    {
        /// <summary>
        ///     Создать исполнитель SQL-команд.
        /// </summary>
        public ISqlQueryExecutor CreateSqlQueryExecutor()
        {
            return new DapperSqlQueryExecutor();
        }
    }
}