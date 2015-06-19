using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.Api.ContextComponents
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