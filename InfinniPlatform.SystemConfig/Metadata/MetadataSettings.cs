namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class MetadataSettings
    {
        /// <summary>
        /// Настройки хранения метаданных.
        /// </summary>
        public const string SectionName = "metadata";

        public MetadataSettings()
        {
            ContentDirectory = "content";
            EnableFileSystemWatcher = false;
        }

        /// <summary>
        /// Каталог с файлами метаданных.
        /// </summary>
        public string ContentDirectory { get; set; }

        /// <summary>
        /// Разрешает обновление кэша метаданных на сервере при их изменении на диске.
        /// </summary>
        public bool EnableFileSystemWatcher { get; set; }
    }
}