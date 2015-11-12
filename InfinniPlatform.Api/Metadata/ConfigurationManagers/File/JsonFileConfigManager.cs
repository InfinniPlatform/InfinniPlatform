using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.File.MetadataReaders;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.File
{
    /// <summary>
    /// Менеджер конфигураций, хранящихся в Zip-архивах
    /// </summary>
    public sealed class JsonFileConfigManager : IManagerIdentifiers
    {
        /// <summary>
        /// Создать менеджер файловых конфигураций JSON
        /// </summary>
        /// <param name="configDirectory">Каталог, где хранятся конфигурации</param>
        public JsonFileConfigManager(string configDirectory)
        {
            _configDirectory = Path.GetFullPath(configDirectory);
        }

        private readonly string _configDirectory;
        private readonly List<JsonFileConfig> _configList = new List<JsonFileConfig>();

        public string GetConfigurationUid(string name)
        {
            var config = GetJsonFileConfig(name);
            return config != null ? config.Id : null;
        }

        public string GetDocumentUid(string configurationId, string documentId)
        {
            var config = GetJsonFileConfig(configurationId);
            IEnumerable<dynamic> documents = config.Documents;
            return documents.Where(d => d.Name == documentId).Select(d => d.Id).FirstOrDefault();
        }

        public string GetSolutionUid(string name)
        {
            throw new NotImplementedException();
        }

        public void ReadConfigurations()
        {
            var fileNames =
                Directory.GetFiles(_configDirectory)
                         .Where(f => Path.GetExtension(f).ToLowerInvariant() == ".zip")
                         .ToList();

            foreach (var fileName in fileNames)
            {
                try
                {
                    var stream = new FileStream(Path.Combine(_configDirectory, fileName), FileMode.Open);
                    var zipArchive = stream.ReadArchive(Encoding.UTF8);
                    try
                    {
                        _configList.Add(new JsonFileConfig(fileName, zipArchive));
                    }
                    finally
                    {
                        zipArchive.Dispose();
                        stream.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error on reading json configuration file archive: {0}, Error: {1}", fileName,
                        e.Message);
                }
            }
        }

        /// <summary>
        /// Получить список существующих конфигураций
        /// </summary>
        /// <returns>Список конфигураций</returns>
        public IEnumerable<string> GetConfigurationList()
        {
            return _configList.Select(c => c.GetConfigurationId()).ToList();
        }

        /// <summary>
        /// Получить конфигурацию из JSON-файла по указанному идентификатору конфигурации
        /// </summary>
        /// <param name="configurationId">Идентификатор конфигурации</param>
        /// <returns>Конфигурация JSON</returns>
        public dynamic GetJsonFileConfig(string configurationId)
        {
            return _configList.Where(c => c.GetConfigurationId().ToLowerInvariant() == configurationId.ToLowerInvariant())
                              .Select(c => c.ConfigObject)
                              .FirstOrDefault();
        }

        public IDataReader BuildDocumentReader(string configurationId)
        {
            var jsonConfig = GetJsonFileConfig(configurationId);
            if (jsonConfig != null)
            {
                return new JsonFileDocumentReader(jsonConfig);
            }
            throw new ArgumentException(string.Format("configuration: {0} not found.", configurationId));
        }

        public IDataReader BuildRegisterReader(string configurationId)
        {
            var jsonConfig = GetJsonFileConfig(configurationId);
            if (jsonConfig != null)
            {
                return new JsonFileRegisterReader(jsonConfig);
            }
            throw new ArgumentException(string.Format("configuration: {0} not found.", configurationId));
        }

        public IDataReader BuildDocumentElementReader(string configurationId, string documentName, string metadataType)
        {
            var jsonConfig = GetJsonFileConfig(configurationId);
            if (jsonConfig != null)
            {
                IEnumerable<dynamic> documents = jsonConfig.Documents;
                dynamic document =
                    documents.FirstOrDefault(d => d.Name.ToLowerInvariant() == documentName.ToLowerInvariant());
                if (document != null)
                {
                    return new JsonFileDocumentElementReader(document, metadataType);
                }
                throw new ArgumentException(string.Format("document: {0} not found.", documentName));
            }
            throw new ArgumentException(string.Format("configuration: {0} not found.", configurationId));
        }

        /// <summary>
        /// Получить конфигурацию JSON по имени файла архива
        /// </summary>
        /// <param name="configurationFileName">Файл архива конфигурации</param>
        /// <returns>Объект конфигурации</returns>
        public dynamic GetJsonFileConfigByFileName(string configurationFileName)
        {
            return _configList.Where(c => c.FileName.ToLowerInvariant() == configurationFileName.ToLowerInvariant())
                              .Select(c => c.ConfigObject)
                              .FirstOrDefault();
        }
    }
}