using System.IO;

namespace InfinniPlatform.Watcher
{
    /// <summary>
    /// Настройки наблюдателя.
    /// </summary>
    public class WatcherSettings
    {
        public const string SectionName = "watcher";

        public WatcherSettings()
        {
            _sourceDirectory = string.Empty;
            _destinationDirectory = string.Empty;
            WatchingFileExtensions = new[] { ".json" };
            SyncOnStart = true;
        }

        private string _destinationDirectory;

        private string _sourceDirectory;

        /// <summary>
        /// Источник метаданных.
        /// </summary>
        public string SourceDirectory
        {
            get
            {
                return string.IsNullOrEmpty(_sourceDirectory)
                           ? _sourceDirectory
                           : new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _sourceDirectory)).FullName;
            }
            set { _sourceDirectory = value; }
        }

        /// <summary>
        /// Синхронизируемая папка.
        /// </summary>
        public string DestinationDirectory
        {
            get
            {
                return string.IsNullOrEmpty(_destinationDirectory)
                           ? _sourceDirectory
                           : new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _destinationDirectory)).FullName;
            }
            set { _destinationDirectory = value; }
        }

        /// <summary>
        /// Расширения синхронизируемых файлов.
        /// </summary>
        public string[] WatchingFileExtensions { get; set; }

        /// <summary>
        /// Разрешить синхронизация содежимого папок при старте приложения.
        /// </summary>
        public bool SyncOnStart { get; set; }
    }
}