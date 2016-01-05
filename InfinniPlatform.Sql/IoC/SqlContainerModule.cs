using InfinniPlatform.Core.Factories;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Sql.IoC
{
    internal sealed class SqlContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<DapperSqlQueryExecutorFactory>()
                   .As<ISqlQueryExecutorFactory>()
                   .SingleInstance();
        }
    }
}