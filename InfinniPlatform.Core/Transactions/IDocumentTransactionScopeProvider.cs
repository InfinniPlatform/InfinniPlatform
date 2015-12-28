namespace InfinniPlatform.Transactions
{
    /// <summary>
    /// Предоставляет доступ к транзакции.
    /// </summary>
    public interface IDocumentTransactionScopeProvider
    {
        /// <summary>
        /// Возвращает транзакцию.
        /// </summary>
        IDocumentTransactionScope GetTransactionScope();
    }
}