using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Registers;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    ///   API для управления конфигурациями метаданных JSON
    /// </summary>
    public sealed class MetadataManagerConfiguration : IDataManager
    {
        private readonly IDataReader _metadataReader;
        private readonly string _version;

        public MetadataManagerConfiguration(IDataReader metadataReader, string version)
        {
            _metadataReader = metadataReader;
            _version = version;
        }

        private void SetConfiguration(string name, dynamic metadataConfig)
        {
            RestQueryApi.QueryPostRaw("SystemConfig", "metadata", "changemetadata", name, metadataConfig).ToDynamic();
        }

        /// <summary>
        ///   Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name">Наименование создаваемой конфигурации</param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildConfiguration(name, name, name, _version);
        }

        /// <summary>
        ///   Добавить метаданные конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемой конфигурации</param>
        public void InsertItem(dynamic objectToCreate)
        {
            var metadataConfig = InsertConfiguration(objectToCreate);
            InsertCommonDocument(metadataConfig.Name);
            InsertRegistersCommonDocument(metadataConfig.Name);
        }

        private dynamic InsertConfiguration(dynamic objectToCreate)
        {
            //изменяемая конфигурация - либо сохраненная конфигурация, либо вновь создаваемая
            var updatingConfiguration = MetadataExtensions.GetStoredMetadata(_metadataReader, objectToCreate) ?? objectToCreate;

            objectToCreate = ((object)objectToCreate).ToDynamic();
            if (String.IsNullOrEmpty(objectToCreate.Name))
            {
                throw new ArgumentException(Resources.IncorrectConfigurationName);
            }

            var metadataConfig = MetadataBuilderExtensions.BuildConfiguration(objectToCreate.Name, objectToCreate.Caption,
                                                                              objectToCreate.Description, objectToCreate.Version);

            metadataConfig.Id = objectToCreate.Id;

            SetConfiguration(updatingConfiguration.Name, metadataConfig);

            return metadataConfig;


        }

        private void InsertCommonDocument(string configurationId)
        {
            var manager = new ManagerFactoryConfiguration(_version, configurationId).BuildDocumentManager();

            var document = MetadataBuilderExtensions.BuildDocument("Common", "Common options", "Common options", "Common",_version);

            manager.InsertItem(document);
        }

        private void InsertRegistersCommonDocument(string configurationId)
        {
            // В конфигурации должен быть один документ, хранящий общие сведения по всем регистрам
            var manager = new ManagerFactoryConfiguration(_version, configurationId).BuildDocumentManager();
            var document = MetadataBuilderExtensions.BuildDocument(
                configurationId + RegisterConstants.RegistersCommonInfo,
                "Registers common options",
                "Storage for register's common information (e.g. actual date)",
                configurationId + RegisterConstants.RegistersCommonInfo,_version);

            document.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            document.Versioning = 4;

            manager.InsertItem(document);
        }

        /// <summary>
        ///   Удалить метаданные указанной конфигурации
        /// </summary>
        /// <param name="metadataObject">Удаляемый объект метаданных</param>
        public void DeleteItem(dynamic metadataObject)
        {

            var configHeader =
                MetadataExtensions.GetStoredMetadata(ManagerFactoryConfiguration.BuildConfigurationMetadataReader(_version,false),metadataObject);

            if (configHeader != null)
            {
                var managerConfig = new ManagerFactoryConfiguration(_version, configHeader.Name);
                foreach (var configMetadataType in MetadataType.GetConfigMetadataTypes())
                {
                    var manager = managerConfig.BuildManagerByType(configMetadataType);
                    var metadataReader = managerConfig.BuildMetadataReaderByType(configMetadataType);
                    var items = metadataReader.GetItems();
                    foreach (var item in items)
                    {
                        manager.DeleteItem(item);
                    }
                }

                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "deletemetadata", configHeader.Name, null, _version);
            }
        }

        /// <summary>
        ///   Обновить метаданные указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные обновляемой конфигурации</param>
        public void MergeItem(dynamic objectToCreate)
        {
            objectToCreate = ((object)objectToCreate).ToDynamic();

            //если создаем новую конфигурацию (Id не установлен, то добавляем Common документ)
            if (objectToCreate.Id == null)
            {
                InsertItem(objectToCreate);
            }
            else
            {
                InsertConfiguration(objectToCreate);
            }
        }



    }
}
