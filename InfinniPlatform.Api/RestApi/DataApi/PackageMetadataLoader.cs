using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    /// <summary>
    /// Кэш метаданных для конфигуратора (UserInterface.exe).
    /// </summary>
    public class PackageMetadataLoader
    {
        /// <summary>
        /// k
        /// Кэш конфигураций.
        /// </summary>
        private static Dictionary<string, object> Configurations { get; set; } = LoadConfigsMetadata();

        /// <summary>
        /// Обновить кэш конфигураций.
        /// </summary>
        public static void UpdateCache(string metadataPath = null)
        {
            Configurations = LoadConfigsMetadata(metadataPath);
        }

        private static Dictionary<string, object> LoadConfigsMetadata(string metadataDirectory = null)
        {
            if (metadataDirectory == null)
            {
                metadataDirectory = Path.GetFullPath(AppSettings.GetValue("ContentDirectory", Path.Combine("..", "Assemblies", "content")));
            }

            var metadataDirectories = Directory.EnumerateDirectories(metadataDirectory)
                                               .Select(d => Path.Combine(d, "metadata"))
                                               .Where(Directory.Exists)
                                               .ToArray();

            IEnumerable<dynamic> loadConfigsMetadata = metadataDirectories
                .SelectMany(Directory.EnumerateDirectories)
                .Select(LoadConfigMetadata);

            var dictionary = loadConfigsMetadata.ConvertToDictionary();

            return dictionary;
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

        private static Dictionary<string, object> LoadDocumentsMetadata(string configDirectory, object configId)
        {
            var documentsDirectory = Path.Combine(configDirectory, "Documents");

            if (Directory.Exists(documentsDirectory))
            {
                IEnumerable<dynamic> enumerable = Directory.EnumerateDirectories(documentsDirectory)
                                                           .Select(d => LoadDocumentMetadata(d, configId));

                return enumerable.ConvertToDictionary();
            }

            return new Dictionary<string, object>();
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

        private static Dictionary<string, object> LoadItemsMetadata(string documentDirectory, string itemsContainer, object configId, object documentId = null)
        {
            var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

            if (Directory.Exists(itemsDirectory))
            {
                dynamic[] itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
                                                   .Select(LoadItemMetadata)
                                                   .ToArray();

                foreach (var item in itemsMetadata)
                {
                    item.ConfigId = configId;
                    item.DocumentId = documentId;
                }

                return itemsMetadata.ConvertToDictionary();
            }

            return new Dictionary<string, object>();
        }

        private static object LoadItemMetadata(string filePath)
        {
            using (var reader = File.OpenRead(filePath))
            {
                dynamic loadItemMetadata = new DynamicWrapper();
                loadItemMetadata.FilePath = filePath;
                loadItemMetadata.Content = JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));

                return loadItemMetadata;
            }
        }

        //Configurations

        public static object GetConfigurationContent(string configId)
        {
            return Configurations.TryGetItemContent(configId);
        }

        public static object GetConfiguration(string configId)
        {
            return Configurations.TryGetItem(configId);
        }

        public static IEnumerable<object> GetConfigurations()
        {
            Dictionary<string, dynamic>.ValueCollection valueCollection = Configurations.Values;
            return valueCollection.Select(o => o.Content);
        }

        public static string GetConfigurationPath(string configId, string subfolder = "InfinniPlatform")
        {
            dynamic oldConfiguration = GetConfiguration(configId);

            if (oldConfiguration != null)
            {
                return oldConfiguration.FilePath;
            }

            var contentDirectory = AppSettings.GetValue("ContentDirectory", Path.Combine("..", "Assemblies", "content"));
            var configurationDirectory = Path.Combine(contentDirectory, subfolder, "metadata", configId);
            Directory.CreateDirectory(configurationDirectory);
            return Path.Combine(configurationDirectory, "Configuration.json");
        }

        //Documents

        public static object GetDocument(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            return MetadataLoaderHelper.TryGetItem(configuration.Documents, documentId);
        }

        public static object GetDocumentContent(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            return MetadataLoaderHelper.TryGetItemContent(configuration.Documents, documentId);
        }

        public static IEnumerable<object> GetDocuments(string configId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> documents = configuration.Documents;
            return documents.Values.Select(o => o.Content);
        }

        public static string GetDocumentPath(string configId, string documentId)
        {
            dynamic oldDocument = GetDocument(configId, documentId);

            if (oldDocument != null)
            {
                return oldDocument.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId);
            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(documentId, ".json"));
        }

        //Views

        public static object GetViewContent(string configId, string documentId, string viewId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItemContent(document.Views, viewId);
        }

        public static object GetView(string configId, string documentId, string viewId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItem(document.Views, viewId);
        }

        public static IEnumerable<object> GetViews(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> views = configuration.Documents[documentId].Views;
            return views.Values.Select(o => o.Content);
        }

        public static string GetViewPath(string configId, string documentId, string viewId)
        {
            dynamic oldView = GetView(configId, documentId, viewId);

            if (oldView != null)
            {
                return oldView.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId, "Views");

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(viewId, ".json"));
        }

        //Assemblies

        public static object GetAssembly(string configId, string assemblyId)
        {
            dynamic configuration = Configurations[configId];
            IEnumerable<dynamic> assemblies = configuration.Content.Assemblies;
            return assemblies.FirstOrDefault(assembly => assembly.Name == assemblyId);
        }

        public static IEnumerable<dynamic> GetAssemblies(string configId)
        {
            dynamic configuration = Configurations[configId];
            return configuration.Content.Assemblies;
        }

        //Menus

        public static object GetMenu(string configId, string menuId)
        {
            dynamic configuration = Configurations[configId];
            return MetadataLoaderHelper.TryGetItem(configuration.Menu, menuId);
        }

        public static object GetMenuContent(string configId, string menuId)
        {
            dynamic configuration = Configurations[configId];
            return MetadataLoaderHelper.TryGetItemContent(configuration.Menu, menuId);
        }

        public static IEnumerable<object> GetMenus(string configId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> documents = configuration.Menu;
            return documents.Values.Select(o => o.Content);
        }

        public static string GetMenuPath(string configId, string menuId)
        {
            dynamic oldMenu = GetMenu(configId, menuId);

            if (oldMenu != null)
            {
                return oldMenu.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Menu", menuId);

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(menuId, ".json"));
        }

        //PrintViews

        public static object GetPrintView(string configId, string documentId, string printViewId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItemContent(document.PrintViews, printViewId);
        }

        public static IEnumerable<object> GetPrintViews(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> printViews = configuration.Documents[documentId].PrintViews;
            return printViews.Values.Select(o => o.Content);
        }

        public static string GetPrintViewPath(string configId, string documentId, string printViewId)
        {
            dynamic oldView = GetPrintView(configId, documentId, printViewId);

            if (oldView != null)
            {
                return oldView.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId, "PrintViews");

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(printViewId, ".json"));
        }

        //Processes

        public static object GetProcess(string configId, string documentId, string processId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItemContent(document.Processes, processId);
        }

        public static IEnumerable<object> GetProcesses(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> processes = configuration.Documents[documentId].Processes;
            return processes.Values.Select(o => o.Content);
        }

        public static string GetProcessPath(string configId, string documentId, string processId)
        {
            dynamic oldProcess = GetProcess(configId, documentId, processId);

            if (oldProcess != null)
            {
                return oldProcess.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId, "Processes");

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(processId, ".json"));
        }

        //Registers

        public static object GetRegister(string configId, string registerId)
        {
            dynamic configuration = Configurations[configId];
            return MetadataLoaderHelper.TryGetItemContent(configuration.Registers, registerId);
        }

        public static IEnumerable<object> GetRegisters(string configId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> documents = configuration.Registers;
            return documents.Values.Select(o => o.Content);
        }

        public static string GetRegisterPath(string configId, string registerId)
        {
            dynamic oldRegister = GetRegister(configId, registerId);

            if (oldRegister != null)
            {
                return oldRegister.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Registers", registerId);

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(registerId, ".json"));
        }

        //Services

        public static object GetService(string configId, string documentId, string serviceId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItemContent(document.Services, serviceId);
        }

        public static IEnumerable<object> GetServices(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> processes = configuration.Documents[documentId].Services;
            return processes.Values.Select(o => o.Content);
        }

        public static string GetServicePath(string configId, string documentId, string serviceId)
        {
            var services = GetService(configId, documentId, serviceId);

            if (services != null)
            {
                dynamic oldServices = services;
                return oldServices.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId, "Services");

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(serviceId, ".json"));
        }

        //Scenarios

        public static object GetScenario(string configId, string documentId, string scenarioId)
        {
            dynamic document = GetDocument(configId, documentId);
            return MetadataLoaderHelper.TryGetItemContent(document.Scenarios, scenarioId);
        }

        public static IEnumerable<object> GetScenarios(string configId, string documentId)
        {
            dynamic configuration = Configurations[configId];
            Dictionary<string, dynamic> processes = configuration.Documents[documentId].Scenarios;
            return processes.Values.Select(o => o.Content);
        }

        public static string GetScenarioPath(string configId, string documentId, string scenarioId)
        {
            dynamic oldScenario = GetScenario(configId, documentId, scenarioId);
            if (oldScenario != null)
            {
                return oldScenario.FilePath;
            }

            var directoryPath = Path.Combine(Path.GetDirectoryName(GetConfigurationPath(configId)), "Documents", documentId, "Scenarios");

            Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, string.Concat(scenarioId, ".json"));
        }
    }
}