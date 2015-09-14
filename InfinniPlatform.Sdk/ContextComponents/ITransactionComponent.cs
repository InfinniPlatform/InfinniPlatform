using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент выполнения транзакций в глоабльном контексте
    /// </summary>
    public interface ITransactionComponent
    {
        /// <summary>
        ///     Получить менеджер транзакций
        /// </summary>
        /// <returns>Менеджер транзакций</returns>
        ITransactionManager GetTransactionManager();
    }
}