namespace InfinniPlatform
{
    /// <summary>
    /// Represents application component options.
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Name of options section in configuration file.
        /// </summary>
        string SectionName { get; }
    }
}