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

        public MetadataManagerConfiguration(IDataReader metadataReader)
        {
            _metadataReader = metadataReader;
        }

        private void SetConfiguration(string name, dynamic metadataConfig)
        {
            RestQueryApi.QueryPostRaw("SystemConfig", "metadata", "changemetadata", name, metadataConfig).ToDynamic();
        }

        /// <summary>
        ///   Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildConfiguration(name, name, name);
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
                                                                              objectToCreate.Description);

            metadataConfig.Id = objectToCreate.Id;

            SetConfiguration(updatingConfiguration.Name, metadataConfig);

            return metadataConfig;


        }

        private void InsertCommonDocument(string configurationId)
        {
            var manager = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();

            var document = MetadataBuilderExtensions.BuildDocument("Common", "Common options", "Common options", "Common");

            manager.InsertItem(document);
        }

        private void InsertRegistersCommonDocument(string configurationId)
        {
            // В конфигурации должен быть один документ, хранящий общие сведения по всем регистрам
            var manager = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();
            var document = MetadataBuilderExtensions.BuildDocument(
                configurationId + RegisterConstants.RegistersCommonInfo,
                "Registers common options",
                "Storage for register's common information (e.g. actual date)",
                configurationId + RegisterConstants.RegistersCommonInfo);

            document.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            document.Versioning = 4;

            manager.InsertItem(document);
        }

        /// <summary>
        ///   Удалить метаданные указанной конфигурации
        /// </summary>
        /// <param name="metadataObject"></param>
        public void DeleteItem(dynamic metadataObject)
        {

            var configHeader =
                MetadataExtensions.GetStoredMetadata(ManagerFactoryConfiguration.BuildConfigurationMetadataReader(),
                                                     metadataObject);

            if (configHeader != null)
            {
                var managerConfig = new ManagerFactoryConfiguration(configHeader.Name);
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
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "deletemetadata", configHeader.Name, null);
            }
        }

        /// <summary>
        ///   Применить изменения метаданных конфигурации
        /// </summary>
        /// <param name="metadataName">Наименование объекта метаданных</param>
        /// <param name="eventDefinitions">События для применения к метаданным</param>
        public void ApplyMetadataChanges(string metadataName, IEnumerable<object> eventDefinitions)
        {
            foreach (var @event in eventDefinitions)
            {
                //имзменение метаданных описывается только с использованием механизма событий
                var result = RestQueryApi.QueryPostRaw("SystemConfig", "metadata", "changemetadata", metadataName, @event);
                if (!result.IsAllOk)
                {
                    throw new ArgumentException(result.Content);
                }
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
