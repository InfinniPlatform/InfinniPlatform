namespace InfinniPlatform.Sdk.Environment.Transactions
{
    /// <summary>
    ///     Менеджер управления транзакциями
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        ///     Получить транзакцию с указанным идентификатором
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        /// <returns>Транзакция</returns>
        ITransaction GetTransaction(string transactionMarker);

        /// <summary>
        ///     Зафиксировать транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        void CommitTransaction(string transactionMarker);

        /// <summary>
        ///     Удалить транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        void RemoveTransaction(string transactionMarker);
    }
}