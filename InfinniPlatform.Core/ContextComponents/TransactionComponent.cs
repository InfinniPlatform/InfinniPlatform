using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Компонент выполнения транзакций в глобальном контексте
    /// </summary>
    public sealed class TransactionComponent : ITransactionComponent
    {
        public TransactionComponent(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        private readonly ITransactionManager _transactionManager;

        /// <summary>
        /// Получить менеджер транзакций
        /// </summary>
        /// <returns>Менеджер транзакций</returns>
        public ITransactionManager GetTransactionManager()
        {
            return _transactionManager;
        }
    }
}