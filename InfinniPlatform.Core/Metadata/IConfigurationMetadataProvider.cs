using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Предоставляет доступ к метаданным приложения.
    /// </summary>
    public interface IConfigurationMetadataProvider
    {
        IEnumerable<string> GetConfigurationNames();

        IConfigurationMetadata GetConfiguration(string configuration);
    }
}