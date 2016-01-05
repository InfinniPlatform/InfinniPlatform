using InfinniPlatform.Core.Sql;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания исполнителя SQL-команд.
    /// </summary>
    public interface ISqlQueryExecutorFactory
    {
        /// <summary>
        ///     Создать исполнитель SQL-команд.
        /// </summary>
        ISqlQueryExecutor CreateSqlQueryExecutor();
    }
}