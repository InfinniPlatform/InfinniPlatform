using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
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
            dynamic result = new DynamicWrapper();

            result.Name = name;
            result.Caption = name;
            result.Description = name;
            result.Menu = new List<dynamic>();
            result.Registers = new List<dynamic>();
            result.Documents = new List<dynamic>();
            result.Assemblies = new List<dynamic>();
            result.Reports = new List<dynamic>();

            return result;
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

            dynamic metadataConfig = new DynamicWrapper();
            metadataConfig.Id = objectToCreate.Id;
            metadataConfig.Name = objectToCreate.Name;
            metadataConfig.Caption = objectToCreate.Caption;
            metadataConfig.Description = objectToCreate.Description;
            metadataConfig.Menu = new List<dynamic>();
            metadataConfig.Registers = new List<dynamic>();
            metadataConfig.Documents = new List<dynamic>();
            metadataConfig.Assemblies = new List<dynamic>();
            metadataConfig.Reports = new List<dynamic>();

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

            dynamic document = new DynamicWrapper();

            document.Id = Guid.NewGuid().ToString();
            document.Name = "Common";
            document.Caption = "Common";
            document.Description = "Common";
            document.Versioning = 2;
            document.MetadataIndex = "Common";
            document.Services = new List<dynamic>();
            document.Processes = new List<dynamic>();
            document.Scenarios = new List<dynamic>();
            document.Generators = new List<dynamic>();
            document.Views = new List<dynamic>();
            document.PrintViews = new List<dynamic>();
            document.ValidationWarnings = new List<dynamic>();
            document.ValidationErrors = new List<dynamic>();

            manager.InsertItem(document);
        }

        private void InsertRegistersCommonDocument(string configurationId)
        {
            // В конфигурации должен быть один документ, хранящий общие сведения по всем регистрам
            var manager = new ManagerFactoryConfiguration(configurationId).BuildDocumentManager();

            var registerName = configurationId + RegisterConstants.RegistersCommonInfo;

            dynamic registerDocument = new DynamicWrapper();
            registerDocument.Id = Guid.NewGuid().ToString();
            registerDocument.Name = registerName;
            registerDocument.Caption = registerName;
            registerDocument.Description = registerName;
            registerDocument.Versioning = 2;
            registerDocument.MetadataIndex = registerName;
            registerDocument.Services = new List<dynamic>();
            registerDocument.Processes = new List<dynamic>();
            registerDocument.Scenarios = new List<dynamic>();
            registerDocument.Generators = new List<dynamic>();
            registerDocument.Views = new List<dynamic>();
            registerDocument.PrintViews = new List<dynamic>();
            registerDocument.ValidationWarnings = new List<dynamic>();
            registerDocument.ValidationErrors = new List<dynamic>();
            registerDocument.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            registerDocument.Versioning = 4;

            manager.InsertItem(registerDocument);
        }
    }
}