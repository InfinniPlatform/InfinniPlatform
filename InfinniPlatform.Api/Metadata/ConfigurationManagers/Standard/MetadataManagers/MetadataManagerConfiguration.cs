using System;

using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    /// API для управления конфигурациями метаданных JSON
    /// </summary>
    public sealed class MetadataManagerConfiguration : IDataManager
    {
        public MetadataManagerConfiguration(IDataReader metadataReader)
        {
            MetadataReader = metadataReader;
        }

        public IDataReader MetadataReader { get; }

        /// <summary>
        /// Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name">Наименование создаваемой конфигурации</param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return MetadataBuilderExtensions.BuildConfiguration(name, name, name);
        }

        /// <summary>
        /// Добавить метаданные конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемой конфигурации</param>
        public void InsertItem(dynamic objectToCreate)
        {
            var metadataConfig = InsertConfiguration(objectToCreate);
            InsertCommonDocument(metadataConfig.Name);
            InsertRegistersCommonDocument(metadataConfig.Name);
        }

        /// <summary>
        /// Удалить метаданные указанной конфигурации
        /// </summary>
        /// <param name="metadataObject">Удаляемый объект метаданных</param>
        public void DeleteItem(dynamic metadataObject)
        {
            var configHeader =
                MetadataExtensions.GetStoredMetadata(
                    ManagerFactoryConfiguration.BuildConfigurationMetadataReader(false), metadataObject);

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
            }
        }

        /// <summary>
        /// Обновить метаданные указанной конфигурации
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

        private void SetConfiguration(string name, dynamic metadataConfig)
        {
        }

        private dynamic InsertConfiguration(dynamic objectToCreate)
        {
            //изменяемая конфигурация - либо сохраненная конфигурация, либо вновь создаваемая
            var updatingConfiguration = MetadataExtensions.GetStoredMetadata(MetadataReader, objectToCreate) ??
                                        objectToCreate;

            objectToCreate = ((object)objectToCreate).ToDynamic();
            if (string.IsNullOrEmpty(objectToCreate.Name))
            {
                throw new ArgumentException(Resources.IncorrectConfigurationName);
            }

            var metadataConfig = MetadataBuilderExtensions.BuildConfiguration(objectToCreate.Name,
                objectToCreate.Caption,
                objectToCreate.Description);

            metadataConfig.Id = objectToCreate.Id;

            if (updatingConfiguration != null)
            {
                DeleteItem(updatingConfiguration);
            }
            SetConfiguration(updatingConfiguration.Name, metadataConfig);

            return metadataConfig;
        }

        private void InsertCommonDocument(string configurationId)
        {
            var manager = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();

            var document = MetadataBuilderExtensions.BuildDocument("Common", "Common options", "Common options",
                "Common");

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
    }
}