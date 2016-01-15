using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Загружает метаданные прикладных конфигураций из JSON-файлов текущего пакета приложения.
    /// </summary>
    internal sealed class PackageJsonConfigurationsInitializer : IStartupInitializer
    {
        public PackageJsonConfigurationsInitializer(IMetadataConfigurationProvider metadataConfigurationProvider, IAppConfiguration appConfiguration)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;

            _contentDirectory = appConfiguration.GetSection("metadata")?.ContentDirectory as string ?? "content";
            _configurations = new Lazy<IEnumerable<DynamicWrapper>>(LoadConfigsMetadata);

            var watcher = new FileSystemWatcher(_contentDirectory, "*.json")
            {
                IncludeSubdirectories = true
            };

            watcher.Changed += (sender, args) => { UpdateConfigsMetadata(args); };
            watcher.Created += (sender, args) => { UpdateConfigsMetadata(args); };
            watcher.Deleted += (sender, args) => { UpdateConfigsMetadata(args); };

            watcher.EnableRaisingEvents = true;
        }


        private readonly string _contentDirectory;
        private readonly Lazy<IEnumerable<DynamicWrapper>> _configurations;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


        public int Order => 0;


        public void OnStart()
        {
            // Получение списка всех установленных конфигураций
            var configurations = _configurations.Value;

            // Загрузка и кэширование метаданных каждой конфигурации
            foreach (dynamic configuration in configurations)
            {
                string configurationId = configuration.Name;

                InstallConfiguration(configuration, configurationId);
            }
        }


        private void UpdateConfigsMetadata(FileSystemEventArgs args)
        {
            if (FileHistoryHelper.IsChanged(args.FullPath))
            {
                try
                {
#if DEBUG
                    Console.WriteLine(@"[{1}] File {0} changed.", args.Name, DateTime.Now.TimeOfDay);
#endif

                    foreach (dynamic configuration in _configurations.Value)
                    {
                        RemoveConfiguration(configuration.Name);
                    }

                    var updatedConfigurations = LoadConfigsMetadata();

                    foreach (dynamic configuration in updatedConfigurations)
                    {
                        InstallConfiguration(configuration, configuration.Name);
                    }

#if DEBUG
                    Console.WriteLine(@"[{0}] Configurations successfully updated.", DateTime.Now.TimeOfDay);
#endif
                }
                catch (Exception e)
                {
                    Logger.Log.Error("Error during metadata update.", null, e);
                }
            }
        }

        private void InstallConfiguration(object configuration, string configId)
        {
            // Загрузка метаданных конфигурации для кэширования
            var metadataCacheFiller = LoadConfigurationMetadata(configuration);

            // Создание менеджера кэша метаданных конфигураций
            var metadataCacheManager = _metadataConfigurationProvider.AddConfiguration(configId, false);

            // Загрузка метаданных конфигурации в кэш
            metadataCacheFiller.InstallConfiguration(metadataCacheManager);
        }

        private void RemoveConfiguration(string configurationName)
        {
            // Удаление метаданных конфигурации из кэша
            _metadataConfigurationProvider.RemoveConfiguration(configurationName);
        }

        private static PackageJsonConfigurationInstaller LoadConfigurationMetadata(dynamic configuration)
        {
            IEnumerable<dynamic> menu = configuration.Menu;
            IEnumerable<dynamic> registers = configuration.Registers;
            IEnumerable<dynamic> documents = configuration.Documents;
            IEnumerable<dynamic> scenarios = documents.SelectMany(i => (IEnumerable<dynamic>)i.Scenarios).ToArray();
            IEnumerable<dynamic> views = documents.SelectMany(i => (IEnumerable<dynamic>)i.Views).ToArray();
            IEnumerable<dynamic> printViews = documents.SelectMany(i => (IEnumerable<dynamic>)i.PrintViews).ToArray();

            return new PackageJsonConfigurationInstaller(
                menu,
                registers,
                documents,
                scenarios,
                views,
                printViews);
        }

        private IEnumerable<DynamicWrapper> LoadConfigsMetadata()
        {
            var metadataDirectories = Directory.EnumerateDirectories(_contentDirectory)
                                               .Select(d => Path.Combine(d, "metadata"))
                                               .Where(Directory.Exists)
                                               .ToArray();

            return metadataDirectories
                .SelectMany(Directory.EnumerateDirectories)
                .Select(LoadConfigMetadata)
                .ToArray();
        }

        private static DynamicWrapper LoadConfigMetadata(string configDirectory)
        {
            var configFile = Path.Combine(configDirectory, "Configuration.json");

            dynamic configuration = LoadItemMetadata(configFile);

            object configId = configuration.Name;

            configuration.Version = null;
            configuration.Menu = LoadItemsMetadata(configDirectory, "Menu", configId);
            configuration.Registers = LoadItemsMetadata(configDirectory, "Registers", configId);
            configuration.Documents = LoadDocumentsMetadata(configDirectory, configId);

            return configuration;
        }

        private static IEnumerable<object> LoadDocumentsMetadata(string configDirectory, object configId)
        {
            var documentsDirectory = Path.Combine(configDirectory, "Documents");

            if (Directory.Exists(documentsDirectory))
            {
                return Directory.EnumerateDirectories(documentsDirectory)
                                .Select(d => LoadDocumentMetadata(d, configId))
                                .ToArray();
            }

            return Enumerable.Empty<object>();
        }

        private static object LoadDocumentMetadata(string documentDirectory, object configId)
        {
            var documentFile = Directory.EnumerateFiles(documentDirectory, "*.json").FirstOrDefault();

            dynamic document = LoadItemMetadata(documentFile);

            object documentId = document.Name;

            document.ConfigId = configId;
            document.Views = LoadItemsMetadata(documentDirectory, "Views", configId, documentId);
            document.PrintViews = LoadItemsMetadata(documentDirectory, "PrintViews", configId, documentId);
            document.Scenarios = LoadItemsMetadata(documentDirectory, "Scenarios", configId, documentId);
            document.Processes = LoadItemsMetadata(documentDirectory, "Processes", configId, documentId);
            document.Services = LoadItemsMetadata(documentDirectory, "Services", configId, documentId);
            document.Generators = LoadItemsMetadata(documentDirectory, "Generators", configId, documentId);
            document.ValidationWarnings = LoadItemsMetadata(documentDirectory, "ValidationWarnings", configId, documentId);
            document.ValidationErrors = LoadItemsMetadata(documentDirectory, "ValidationErrors", configId, documentId);
            document.DocumentStatuses = LoadItemsMetadata(documentDirectory, "DocumentStatuses", configId, documentId);

            return document;
        }

        private static IEnumerable<object> LoadItemsMetadata(string documentDirectory, string itemsContainer, object configId, object documentId = null)
        {
            var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

            if (Directory.Exists(itemsDirectory))
            {
                var itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
                                             .Select(LoadItemMetadata)
                                             .ToArray();

                foreach (dynamic item in itemsMetadata)
                {
                    item.ConfigId = configId;
                    item.DocumentId = documentId;
                }

                return itemsMetadata;
            }

            return Enumerable.Empty<object>();
        }

        private static object LoadItemMetadata(string fileName)
        {
            using (var reader = File.OpenRead(fileName))
            {
                return JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));
            }
        }
    }
}