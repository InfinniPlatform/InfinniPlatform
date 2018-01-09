namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Factory for creating <see cref="ISharedCache"/> instance.
    /// </summary>
    public interface ISharedCacheFactory
    {
        /// <summary>
        /// Returns instance of <see cref="ISharedCache"/>.
        /// </summary>
        ISharedCache Create();
    }
}