using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Api.SystemExtensions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.File
{
    public sealed class JsonFileConfig
    {
        private readonly string _fileName;
        private readonly ZipArchive _archiveConfig;

	    private readonly string _version = "version_jsonfile";
        private readonly string _folderName = "version_jsonfile_" + Guid.NewGuid().ToString();

        public JsonFileConfig(string fileName, ZipArchive archiveConfig)
        {
            _fileName = Path.GetFileName(fileName);
            _archiveConfig = archiveConfig;
	        ConfigObject = new ConfigurationIterator(new ZipStructure(_archiveConfig, _folderName)).ImportToConfigurationObject();
        }

        /// <summary>
        ///   Описание конфигурации в JSON
        /// </summary>
        public dynamic ConfigObject { get; private set; }

        /// <summary>
        ///   Файл архива, в котором содержится конфигурация
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }


        public override string ToString()
        {
            return ConfigObject.ToString();
        }

        /// <summary>
        ///   Получить идентификатор конфигурации
        /// </summary>
        /// <returns>Идентификатор конфигурации</returns>
        public string GetConfigurationId()
        {
            return ConfigObject != null ? ConfigObject.Name : string.Empty;
        }

        /// <summary>
        ///   Получить версию конфигурации
        /// </summary>
        /// <returns>Идентификатор версии</returns>
        public string GetVersion()
        {
            return ConfigObject != null ? ConfigObject.Version : string.Empty;
        }
    }
}
