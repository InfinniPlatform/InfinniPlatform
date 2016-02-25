namespace InfinniPlatform.Sdk.Documents.Transactions
{
    /// <summary>
    /// Предоставляет интерфейс для создания экземпляров <see cref="IUnitOfWork"/>.
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Создает экземпляр <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork Create();
    }
}