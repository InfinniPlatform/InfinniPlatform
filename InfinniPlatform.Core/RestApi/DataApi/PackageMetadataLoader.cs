using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.RestApi.DataApi
{
    /// <summary>
    /// Кэш метаданных для конфигуратора (UserInterface.exe).
    /// </summary>
    public static class PackageMetadataLoader
    {
        private static IDictionary<string, object> Configurations { get; set; } = LoadConfigsMetadata();


        /// <summary>
        /// Обновить кэш конфигураций.
        /// </summary>
        public static void UpdateCache(string metadataPath = null)
        {
            Configurations = LoadConfigsMetadata(metadataPath);
        }


        private static IDictionary<string, object> LoadConfigsMetadata(string metadataDirectory = null)
        {
            if (metadataDirectory == null)
            {
                metadataDirectory = GetContentDirectory();
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

            dynamic configuration = ReadMetadataFromFile(configFile);

            string configId = configuration.Name;

            configuration.Menu = LoadItemsMetadata(configDirectory, "Menu", configId);
            configuration.Registers = LoadItemsMetadata(configDirectory, "Registers", configId);
            configuration.Documents = LoadDocumentsMetadata(configDirectory, configId);

            return configuration;
        }

        private static IDictionary<string, object> LoadDocumentsMetadata(string configDirectory, string configId)
        {
            var documentsDirectory = Path.Combine(configDirectory, "Documents");

            if (Directory.Exists(documentsDirectory))
            {
                var enumerable = Directory.EnumerateDirectories(documentsDirectory).Select(d => LoadDocumentMetadata(d, configId));

                return enumerable.ConvertToDictionary();
            }

            return new Dictionary<string, object>();
        }

        private static object LoadDocumentMetadata(string documentDirectory, string configId)
        {
            var documentFile = Directory.EnumerateFiles(documentDirectory, "*.json").FirstOrDefault();

            dynamic document = ReadMetadataFromFile(documentFile);

            string documentId = document.Name;

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

        private static IDictionary<string, object> LoadItemsMetadata(string documentDirectory, string itemsContainer, string configId, string documentId = null)
        {
            var itemsDirectory = Path.Combine(documentDirectory, itemsContainer);

            if (Directory.Exists(itemsDirectory))
            {
                var itemsMetadata = Directory.EnumerateFiles(itemsDirectory, "*.json", SearchOption.AllDirectories)
                                             .Select(ReadMetadataFromFile)
                                             .ToArray();

                foreach (dynamic item in itemsMetadata)
                {
                    item.ConfigId = configId;
                    item.DocumentId = documentId;
                }

                return itemsMetadata.ConvertToDictionary();
            }

            return new Dictionary<string, object>();
        }

        private static object ReadMetadataFromFile(string filePath)
        {
            using (var reader = File.OpenRead(filePath))
            {
                dynamic loadItemMetadata = new DynamicWrapper();
                loadItemMetadata.FilePath = filePath;
                loadItemMetadata.Content = JsonObjectSerializer.Default.Deserialize(reader, typeof(DynamicWrapper));

                return loadItemMetadata;
            }
        }

        // Configurations

        private static object GetConfigurationInfo(string configId)
        {
            return Configurations.TryGetItem(configId);
        }

        public static object GetConfiguration(string configId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            return configurationInfo?.Content;
        }

        public static IEnumerable<object> GetConfigurations()
        {
            foreach (dynamic configurationInfo in Configurations.Values)
            {
                yield return configurationInfo.Content;
            }
        }

        public static string GetConfigurationPath(string configId, string subfolder = "InfinniPlatform")
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            if (configurationInfo != null && !string.IsNullOrEmpty(configurationInfo.FilePath))
            {
                return configurationInfo.FilePath;
            }

            var contentDirectory = GetContentDirectory();
            var configurationDirectory = Path.Combine(contentDirectory, subfolder, "metadata", configId);

            if (!Directory.Exists(configurationDirectory))
            {
                Directory.CreateDirectory(configurationDirectory);
            }

            return Path.Combine(configurationDirectory, "Configuration.json");
        }


        // Menus

        private static object GetMenuInfo(string configId, string menuId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> menu = configurationInfo.Menu;

            return menu.TryGetItem(menuId);
        }

        public static object GetMenu(string configId, string menuId)
        {
            dynamic menuInfo = GetMenuInfo(configId, menuId);

            return menuInfo?.Content;
        }

        public static IEnumerable<object> GetMenus(string configId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> menu = configurationInfo.Menu;

            foreach (dynamic menuInfo in menu.Values)
            {
                yield return menuInfo.Content;
            }
        }

        public static string GetMenuPath(string configId, string menuId)
        {
            dynamic menuInfo = GetMenuInfo(configId, menuId);

            if (menuInfo != null && !string.IsNullOrEmpty(menuInfo.FilePath))
            {
                return menuInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var menuDirectory = Path.Combine(configurationDirectory, "Menu", menuId);

            if (!Directory.Exists(menuDirectory))
            {
                Directory.CreateDirectory(menuDirectory);
            }

            return Path.Combine(menuDirectory, menuId + ".json");
        }


        // Registers

        private static object GetRegisterInfo(string configId, string registerId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> registers = configurationInfo.Registers;

            return registers.TryGetItem(registerId);
        }

        public static object GetRegister(string configId, string registerId)
        {
            dynamic registerInfo = GetRegisterInfo(configId, registerId);

            return registerInfo?.Content;
        }

        public static IEnumerable<object> GetRegisters(string configId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> registers = configurationInfo.Registers;

            foreach (dynamic registerInfo in registers.Values)
            {
                yield return registerInfo.Content;
            }
        }

        public static string GetRegisterPath(string configId, string registerId)
        {
            dynamic registerInfo = GetRegisterInfo(configId, registerId);

            if (registerInfo != null && !string.IsNullOrEmpty(registerInfo.FilePath))
            {
                return registerInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var registerDirectory = Path.Combine(configurationDirectory, "Registers", registerId);

            if (!Directory.Exists(registerDirectory))
            {
                Directory.CreateDirectory(registerDirectory);
            }

            return Path.Combine(registerDirectory, registerId + ".json");
        }

        // Documents

        private static object GetDocumentInfo(string configId, string documentId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> documents = configurationInfo.Documents;

            return documents.TryGetItem(documentId);
        }

        public static object GetDocument(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            return documentInfo?.Content;
        }

        public static IEnumerable<object> GetDocuments(string configId)
        {
            dynamic configurationInfo = GetConfigurationInfo(configId);

            IDictionary<string, object> documents = configurationInfo.Documents;

            foreach (dynamic documentInfo in documents.Values)
            {
                yield return documentInfo.Content;
            }
        }

        public static string GetDocumentPath(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            if (documentInfo != null && !string.IsNullOrEmpty(documentInfo.FilePath))
            {
                return documentInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var documentDirectory = Path.Combine(configurationDirectory, "Documents", documentId);

            if (!Directory.Exists(documentDirectory))
            {
                Directory.CreateDirectory(documentDirectory);
            }

            return Path.Combine(documentDirectory, documentId + ".json");
        }

        // Views

        private static object GetViewInfo(string configId, string documentId, string viewId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> views = documentInfo.Views;

            return views.TryGetItem(viewId);
        }

        public static object GetView(string configId, string documentId, string viewId)
        {
            dynamic viewInfo = GetViewInfo(configId, documentId, viewId);

            return viewInfo?.Content;
        }

        public static IEnumerable<object> GetViews(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> views = documentInfo.Views;

            foreach (dynamic viewInfo in views.Values)
            {
                yield return viewInfo.Content;
            }
        }

        public static string GetViewPath(string configId, string documentId, string viewId)
        {
            dynamic viewInfo = GetViewInfo(configId, documentId, viewId);

            if (viewInfo != null && !string.IsNullOrEmpty(viewInfo.FilePath))
            {
                return viewInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var viewDirectory = Path.Combine(configurationDirectory, "Documents", documentId, "Views");

            if (!Directory.Exists(viewDirectory))
            {
                Directory.CreateDirectory(viewDirectory);
            }

            return Path.Combine(viewDirectory, viewId + ".json");
        }

        // PrintViews

        private static object GetPrintViewInfo(string configId, string documentId, string printViewId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> printViews = documentInfo.PrintViews;

            return printViews.TryGetItem(printViewId);
        }

        public static object GetPrintView(string configId, string documentId, string printViewId)
        {
            dynamic printViewInfo = GetPrintViewInfo(configId, documentId, printViewId);

            return printViewInfo?.Content;
        }

        public static IEnumerable<object> GetPrintViews(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> printViews = documentInfo.PrintViews;

            foreach (dynamic printViewInfo in printViews.Values)
            {
                yield return printViewInfo.Content;
            }
        }

        public static string GetPrintViewPath(string configId, string documentId, string printViewId)
        {
            dynamic printViewInfo = GetPrintViewInfo(configId, documentId, printViewId);

            if (printViewInfo != null && !string.IsNullOrEmpty(printViewInfo.FilePath))
            {
                return printViewInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var printViewDirectory = Path.Combine(configurationDirectory, "Documents", documentId, "PrintViews");

            if (!Directory.Exists(printViewDirectory))
            {
                Directory.CreateDirectory(printViewDirectory);
            }

            return Path.Combine(printViewDirectory, printViewId + ".json");
        }

        // Processes

        private static object GetProcessInfo(string configId, string documentId, string processId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> processes = documentInfo.Processes;

            return processes.TryGetItem(processId);
        }

        public static object GetProcess(string configId, string documentId, string processId)
        {
            dynamic processInfo = GetProcessInfo(configId, documentId, processId);

            return processInfo?.Content;
        }

        public static IEnumerable<object> GetProcesses(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> processes = documentInfo.Processes;

            foreach (dynamic processInfo in processes.Values)
            {
                yield return processInfo.Content;
            }
        }

        public static string GetProcessPath(string configId, string documentId, string processId)
        {
            dynamic processInfo = GetProcessInfo(configId, documentId, processId);

            if (processInfo != null && !string.IsNullOrEmpty(processInfo.FilePath))
            {
                return processInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var processDirectory = Path.Combine(configurationDirectory, "Documents", documentId, "Processes");

            if (!Directory.Exists(processDirectory))
            {
                Directory.CreateDirectory(processDirectory);
            }

            return Path.Combine(processDirectory, processId + ".json");
        }

        // Services

        private static object GetServiceInfo(string configId, string documentId, string serviceId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> services = documentInfo.Services;

            return services.TryGetItem(serviceId);
        }

        public static object GetService(string configId, string documentId, string serviceId)
        {
            dynamic serviceInfo = GetServiceInfo(configId, documentId, serviceId);

            return serviceInfo?.Content;
        }

        public static IEnumerable<object> GetServices(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> services = documentInfo.Services;

            foreach (dynamic serviceInfo in services.Values)
            {
                yield return serviceInfo.Content;
            }
        }

        public static string GetServicePath(string configId, string documentId, string serviceId)
        {
            dynamic serviceInfo = GetServiceInfo(configId, documentId, serviceId);

            if (serviceInfo != null && !string.IsNullOrEmpty(serviceInfo.FilePath))
            {
                return serviceInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var serviceDirectory = Path.Combine(configurationDirectory, "Documents", documentId, "Services");

            if (!Directory.Exists(serviceDirectory))
            {
                Directory.CreateDirectory(serviceDirectory);
            }

            return Path.Combine(serviceDirectory, serviceId + ".json");
        }

        // Scenarios

        private static object GetScenarioInfo(string configId, string documentId, string scenarioId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> scenarios = documentInfo.Scenarios;

            return scenarios.TryGetItem(scenarioId);
        }

        public static object GetScenario(string configId, string documentId, string scenarioId)
        {
            dynamic scenarioInfo = GetScenarioInfo(configId, documentId, scenarioId);

            return scenarioInfo?.Content;
        }

        public static IEnumerable<object> GetScenarios(string configId, string documentId)
        {
            dynamic documentInfo = GetDocumentInfo(configId, documentId);

            IDictionary<string, object> scenarios = documentInfo.Scenarios;

            foreach (dynamic scenarioInfo in scenarios.Values)
            {
                yield return scenarioInfo.Content;
            }
        }

        public static string GetScenarioPath(string configId, string documentId, string scenarioId)
        {
            dynamic serviceInfo = GetScenarioInfo(configId, documentId, scenarioId);

            if (serviceInfo != null && !string.IsNullOrEmpty(serviceInfo.FilePath))
            {
                return serviceInfo.FilePath;
            }

            var configurationPath = GetConfigurationPath(configId);
            var configurationDirectory = Path.GetDirectoryName(configurationPath);
            var scenarioDirectory = Path.Combine(configurationDirectory, "Documents", documentId, "Scenarios");

            if (!Directory.Exists(scenarioDirectory))
            {
                Directory.CreateDirectory(scenarioDirectory);
            }

            return Path.Combine(scenarioDirectory, scenarioId + ".json");
        }


        private static string GetContentDirectory()
        {
            var contentDirectory = ConfigurationManager.AppSettings["ContentDirectory"];

            if (string.IsNullOrEmpty(contentDirectory))
            {
                contentDirectory = Path.Combine("..", "Assemblies", "content");
            }

            return Path.GetFullPath(contentDirectory);
        }
    }
}