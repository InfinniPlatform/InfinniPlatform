namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Provides instances of <see cref="IDocumentRequestExecutor"/>.
    /// </summary>
    public interface IDocumentRequestExecutorProvider
    {
        /// <summary>
        /// Provides instance by name of document.
        /// </summary>
        IDocumentRequestExecutor Get(string name);
    }
}