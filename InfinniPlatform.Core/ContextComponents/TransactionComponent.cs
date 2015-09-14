using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент выполнения транзакций в глобальном контексте
    /// </summary>
    public sealed class TransactionComponent : ITransactionComponent
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionComponent(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        /// <summary>
        ///     Получить менеджер транзакций
        /// </summary>
        /// <returns>Менеджер транзакций</returns>
        public ITransactionManager GetTransactionManager()
        {
            return _transactionManager;
        }
    }
}