using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.SystemConfig.Metadata;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Загружает метаданные прикладных конфигураций из JSON-файлов текущего пакета приложения.
    /// </summary>
    internal sealed class PackageJsonConfigurationsInitializer : ApplicationEventHandler
    {
        public PackageJsonConfigurationsInitializer(MetadataApi metadataApi, IAppConfiguration appConfiguration, ILog log) : base(0)
        {
            _metadataApi = metadataApi;
            _log = log;

            dynamic metadataSection = appConfiguration.GetSection("metadata");

            _contentDirectory = ((metadataSection != null) ? metadataSection.ContentDirectory as string : null) ?? "content";
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


        private readonly MetadataApi _metadataApi;
        private readonly ILog _log;

        private readonly string _contentDirectory;
        private readonly Lazy<IEnumerable<DynamicWrapper>> _configurations;


        public override void OnBeforeStart()
        {
            // Получение списка всех установленных конфигураций
            var configurations = _configurations.Value;

            // Загрузка и кэширование метаданных каждой конфигурации
            foreach (dynamic configuration in configurations)
            {
                InstallConfiguration(configuration);
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

                    var updatedConfigurations = LoadConfigsMetadata();

                    foreach (var configuration in updatedConfigurations)
                    {
                        InstallConfiguration(configuration);
                    }

#if DEBUG
                    Console.WriteLine(@"[{0}] Configurations successfully updated.", DateTime.Now.TimeOfDay);
#endif
                }
                catch (Exception e)
                {
                    _log.Error("Error during metadata update.", null, e);
                }
            }
        }

        private void InstallConfiguration(dynamic configuration)
        {
            _metadataApi.AddItemsMetadata(configuration.ItemsMetadata);
        }

        private IEnumerable<DynamicWrapper> LoadConfigsMetadata()
        {
            var metadataDirectories = Directory.EnumerateDirectories(_contentDirectory)
                                               .Select(d => Path.Combine(d, "metadata"))
                                               .Where(Directory.Exists)
                                               .ToArray();

            return metadataDirectories.SelectMany(Directory.EnumerateDirectories)
                                      .Select(LoadConfigMetadata)
                                      .ToArray();
        }

        private static DynamicWrapper LoadConfigMetadata(string configDirectory)
        {
            dynamic configuration = new DynamicWrapper { { "Name", Path.GetDirectoryName(configDirectory) } };

            configuration.ItemsMetadata = LoadItemsMetadata(configDirectory);

            return configuration;
        }

        private static Dictionary<MetadataUniqueName, DynamicWrapper> LoadItemsMetadata(string documentDirectory)
        {
            var enumerateDirectories = Directory.EnumerateDirectories(documentDirectory);

            var itemsMetadataCache = new Dictionary<MetadataUniqueName, DynamicWrapper>();

            foreach (var dir in enumerateDirectories)
            {
                var itemsMetadata = Directory.EnumerateFiles(dir, "*.json", SearchOption.AllDirectories)
                                             .Select(LoadItemMetadata)
                                             .ToArray();

                foreach (var item in itemsMetadata)
                {
                    var metadataName = new MetadataUniqueName((string)item["Namespace"], (string)item["Name"]);

                    if (itemsMetadataCache.ContainsKey(metadataName))
                    {
                        throw new InvalidOperationException($"Metadata object '{metadataName}' is duplicate.");
                    }

                    itemsMetadataCache.Add(metadataName, item);
                }
            }

            return itemsMetadataCache;
        }

        private static DynamicWrapper LoadItemMetadata(string fileName)
        {
            using (var reader = File.OpenRead(fileName))
            {
                return JsonObjectSerializer.Default.Deserialize<DynamicWrapper>(reader);
            }
        }
    }
}