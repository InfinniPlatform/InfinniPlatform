using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            IEnumerable<dynamic> menuList = configuration.Menu;
            IEnumerable<dynamic> registerList = configuration.Registers;
            IEnumerable<dynamic> documentList = configuration.Documents;

            _metadataApi.AddMenu(menuList);
            _metadataApi.AddRegisters(registerList);

            foreach (var document in documentList)
            {
                var processes = document.Processes as IEnumerable<dynamic>;

                if (processes != null)
                {
                    var defaultProcess = processes.FirstOrDefault(i => string.Equals(i.Name, "Default", StringComparison.OrdinalIgnoreCase));

                    if (defaultProcess != null)
                    {
                        var transitions = defaultProcess.Transitions as IEnumerable<dynamic>;

                        if (transitions != null)
                        {
                            document.Events = transitions.FirstOrDefault();
                        }
                    }
                }
            }

            _metadataApi.AddDocuments(documentList);

            foreach (var document in documentList)
            {
                string documentName = document.Name;

                _metadataApi.AddActions(documentName, document.Scenarios);
                _metadataApi.AddViews(documentName, document.Views);
                _metadataApi.AddPrintViews(documentName, document.PrintViews);
            }
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
            dynamic configuration = new DynamicWrapper { { "Name", Path.GetDirectoryName(configDirectory) } };

            configuration.Menu = LoadItemsMetadata(configDirectory, "Menu");
            configuration.Registers = LoadItemsMetadata(configDirectory, "Registers");
            configuration.Documents = LoadDocumentsMetadata(configDirectory);

            return configuration;
        }

        private static IEnumerable<object> LoadDocumentsMetadata(string configDirectory)
        {
            var documentsDirectory = Path.Combine(configDirectory, "Documents");

            if (Directory.Exists(documentsDirectory))
            {
                return Directory.EnumerateFiles(documentsDirectory)
                                .Select(LoadItemMetadata)
                                .ToArray();
            }

            return Enumerable.Empty<object>();
        }

        private static IEnumerable<object> LoadItemsMetadata(string documentDirectory, string itemsContainer, object documentId = null)
        {
            var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

            if (Directory.Exists(itemsDirectory))
            {
                var itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
                                             .Select(LoadItemMetadata)
                                             .ToArray();

                foreach (dynamic item in itemsMetadata)
                {
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