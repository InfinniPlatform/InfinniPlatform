using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.SystemConfig.Metadata;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Загружает метаданные прикладных конфигураций из JSON-файлов текущего пакета приложения.
    /// </summary>
    internal sealed class PackageJsonConfigurationsInitializer : ApplicationEventHandler
    {
        public PackageJsonConfigurationsInitializer(MetadataApi metadataApi, MetadataSettings metadataSettings, ILog log) : base(0)
        {
            _metadataApi = metadataApi;
            _log = log;

            _contentDirectory = metadataSettings.ContentDirectory;
            _configurations = new Lazy<IEnumerable<DynamicWrapper>>(LoadConfigsMetadata);

            if (metadataSettings.EnableFileSystemWatcher)
            {
                var watcher = new FileSystemWatcher(_contentDirectory, "*.json")
                              {
                                  IncludeSubdirectories = true
                              };

                watcher.Changed += (sender, args) => { UpdateConfigsMetadata(args); };
                watcher.Created += (sender, args) => { UpdateConfigsMetadata(args); };
                watcher.Deleted += (sender, args) => { UpdateConfigsMetadata(args); };

                watcher.EnableRaisingEvents = true;
            }
        }

        private readonly Lazy<IEnumerable<DynamicWrapper>> _configurations;

        private readonly string _contentDirectory;
        private readonly ILog _log;

        private readonly MetadataApi _metadataApi;

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
            dynamic configuration = new DynamicWrapper
                                    {
                                        { "Name", Path.GetDirectoryName(configDirectory) }
                                    };

            configuration.ItemsMetadata = LoadItemsMetadata(configDirectory);

            return configuration;
        }

        private static Dictionary<MetadataUniqueName, DynamicWrapper> LoadItemsMetadata(string metadataDirectory)
        {
            var itemsMetadataCache = new Dictionary<MetadataUniqueName, DynamicWrapper>();

            var itemsDirectories = Directory.EnumerateDirectories(metadataDirectory);

            foreach (var itemsDir in itemsDirectories)
            {
                var defaultNamespace = Path.GetFileNameWithoutExtension(itemsDir);

                var itemsMetadata = Directory.EnumerateFiles(itemsDir, "*.json", SearchOption.AllDirectories)
                                             .Select(LoadItemMetadata)
                                             .ToArray();

                foreach (var item in itemsMetadata)
                {
                    var ns = (string)item["Namespace"];

                    if (string.IsNullOrEmpty(ns))
                    {
                        ns = defaultNamespace;
                    }

                    var name = (string)item["Name"];
                    var metadataName = new MetadataUniqueName(ns, name);

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