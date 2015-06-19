using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Transactions;

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