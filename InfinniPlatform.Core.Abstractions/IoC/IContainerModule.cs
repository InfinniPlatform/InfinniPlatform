namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Dependencies registration module.
    /// </summary>
    public interface IContainerModule
    {
        /// <summary>
        /// Loads module.
        /// </summary>
        /// <param name="builder">Dependency container builder.</param>
        void Load(IContainerBuilder builder);
    }
}