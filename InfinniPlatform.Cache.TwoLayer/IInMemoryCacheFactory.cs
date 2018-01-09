namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Factory for creating <see cref="IInMemoryCache"/> instance.
    /// </summary>
    public interface IInMemoryCacheFactory
    {
        /// <summary>
        /// Returns instance of <see cref="IInMemoryCache"/>.
        /// </summary>
        IInMemoryCache Create();
    }
}