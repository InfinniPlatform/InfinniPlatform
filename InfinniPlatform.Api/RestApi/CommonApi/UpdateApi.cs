using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
    [Obsolete]
    public sealed class UpdateApi
    {
        private readonly string _version;

        public UpdateApi(string version)
        {
            _version = version;
        }

        /// <summary>
        ///     Установить JSON-конфигурацию системы
        /// </summary>
        /// <param name="configurationId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа, если необходимо</param>
        /// <param name="metadataObject">Конфигурация системы</param>
        /// <param name="metadataType">Тип метаданных системы</param>
        public void UpdateMetadataObject(string configurationId, string documentId, dynamic metadataObject,
            string metadataType)
        {
            if (metadataObject.Name == null)
            {
                throw new ArgumentException(Resources.ErrorConfigNameIsEmpty);
            }
            var request = new
            {
                ConfigurationId = configurationId,
                DocumentId = documentId,
                MetadataType = metadataType,
                MetadataObject = metadataObject,
                Version = _version
            };

            RestQueryApi.QueryPostJsonRaw("Update", "Package", "installjsonmetadata", null, request);
        }

        /// <summary>
        ///     Обновить метаданные конфигурации в JSON
        /// </summary>
        /// <param name="filePath">Путь к файлу JSON конфигурации</param>
        public dynamic UpdateConfigFromJson(string filePath)
        {
            var builder = new RestQueryBuilder("SystemConfig", "update", "updateconfigfromjson", null);

            var linkedData = new
                {
                    Version = _version
                };

            var response = builder.QueryPostFile(linkedData, filePath, null);

            if (!response.IsAllOk)
            {
                Console.WriteLine("===========package install error===============================");
                Console.WriteLine("Response content: " + response.Content);
                throw new ArgumentException(string.Format("Error update configuration: \"{0}\"  ", filePath));
            }
            var result = response.Content.ToDynamic();
            return result;
        }

        /// <summary>
        ///   Обновить метаданные решения в JSON
        /// </summary>
        /// <param name="filePath">Путь к файлу решения</param>
        /// <returns></returns>
        public dynamic UpdateSolutionFromJson(string filePath)
        {
            var builder = new RestQueryBuilder("SystemConfig", "update", "updatesolutionfromjson", null);

            var linkedData = new
            {
                Version = _version
            };

            var response = builder.QueryPostFile(linkedData, filePath, null);

            if (!response.IsAllOk)
            {
                Console.WriteLine("===========package install error===============================");
                Console.WriteLine("Response content: " + response.Content);
                throw new ArgumentException(string.Format("Error update solution: \"{0}\"  ", filePath));
            }
            var result = response.Content.ToDynamic();
            return result;
        }

        /// <summary>
        ///     Принудительно вызвать обновление конфигурации
        /// </summary>
        public void ForceReload(string configurationId)
        {
            var response = RestQueryApi.QueryPostNotify(_version, configurationId);

            if (!response.IsAllOk)
            {
                Console.WriteLine("===========package install error===============================");
                Console.WriteLine("Response content: " + response.Content);
                throw new ArgumentException(string.Format("Error reload configuration: \"{0}\"  ", configurationId));
            }
        }

        /// <summary>
        ///     Установка пакетов обновления
        ///     Необходимо переписать на выполнение запросов через высокоуровневый API системы
        /// </summary>
        /// <param name="updatePackages">Список устанавливаемых модулей</param>
        [Obsolete]
        public void InstallPackages(IEnumerable<dynamic> updatePackages)
        {
        }

        /// <summary>
        ///     Принудительно вызвать процедуру создания необходимых контейнеров для конфигурации
        /// </summary>
        public void UpdateStore(string configurationId)
        {
            // Создаем индексы под системные конфигурации в случае необходимости
            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runmigration", null,
                new
                    {
                        MigrationName = "UpdateStoreMigration",
                        ConfigurationName = configurationId
                    });
        }

        /// <summary>
        ///     Установить пакет обновления конфигурации
        /// </summary>
        /// <param name="updatePackage">Пакет обновления</param>
        private void InstallPackage(dynamic updatePackage)
        {
            //выполняем обновление метаданных с помощью пакета обновления
            var response = RestQueryApi.QueryPostJsonRaw("Update", "Package", "Install", null, updatePackage);


            if (!response.IsAllOk)
            {
                Console.WriteLine("===========package install error===============================");
                Console.WriteLine("Response content: " + response.Content);
                throw new ArgumentException(string.Format("Error install package {0} :{1} ",
                    updatePackage.PackageHeader.Value, response.Content));
            }            
        }
    }
}