using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    /// Контракт API для получения компонента, отвечающего за работу с метаданными
    /// </summary>
    public sealed class MetadataComponent : IMetadataComponent
    {
        public MetadataComponent(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }

        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

        /// <summary>
        /// Получить метаданные первого уровня вложенности (регистр)
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор объекта метаданных (регистр)</param>
        /// <param name="metadataType">Тип объекта метаданных (MetadataType.Register)</param>
        /// <returns>Метаданные объекта первого уровня вложенности</returns>
        public IEnumerable<dynamic> GetMetadataList(string configId, string objectMetadataId, string metadataType)
        {
            if (string.IsNullOrEmpty(configId))
            {
                return GetConfigMetadata();
            }

            return GetMetadataItem(configId, objectMetadataId, metadataType, null);
        }

        /// <summary>
        /// Получить метаданные второго уровня вложенности для документа
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор объекта метаданных</param>
        /// <param name="metadataType">Тип метаданных</param>
        /// <param name="metadataName">Наименование метаданных</param>
        /// <returns>Метаданные элемента документа</returns>
        public dynamic GetMetadata(string configId, string objectMetadataId, string metadataType, string metadataName)
        {
            Func<dynamic, bool> predicate = item => item.Name == metadataName;

            return GetMetadataItem(configId, objectMetadataId, metadataType, predicate);
        }

        /// <summary>
        /// Получить список всех конфигураций
        /// </summary>
        /// <returns>Список конфигураций метаданных</returns>
        public dynamic GetConfigMetadata()
        {
            return _metadataConfigurationProvider.Configurations
                                                 .Where(c => c.ConfigurationId.ToLowerInvariant() != "update" &&
                                                             c.ConfigurationId.ToLowerInvariant() != "systemconfig" &&
                                                             c.ConfigurationId.ToLowerInvariant() != "restfulapi")
                                                 .Select(c => new
                                                              {
                                                                  Id = c.ConfigurationId,
                                                                  Name = c.ConfigurationId
                                                              }.ToDynamic()).ToList();
        }

        /// <summary>
        /// Получить метаданные объекта с указанием предиката отбора
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор объекта метаданных</param>
        /// <param name="metadataType">Идентификатор типа метаданных</param>
        /// <param name="predicate">Предикат для выборки метаданных</param>
        /// <returns>Метаданные объекта</returns>
        public dynamic GetMetadataItem(string configId, string objectMetadataId, string metadataType, Func<object, bool> predicate)
        {
            if (configId == null)
            {
                throw new ArgumentException("Configuration identifier should not be empty.");
            }

            var config = _metadataConfigurationProvider.GetMetadataConfiguration(configId);

            if (config == null)
            {
                return null;
            }

            if (metadataType == MetadataType.Register)
            {
                return predicate == null
                    ? config.GetRegisterList()
                    : config.GetRegister(objectMetadataId);
            }

            if (metadataType == MetadataType.Menu)
            {
                return predicate == null
                    ? config.GetMenuList()
                    : config.GetMenu(predicate);
            }


            if (objectMetadataId == null)
            {
                if (predicate == null)
                {
                    return config.Documents.Select(c => new
                                                        {
                                                            Id = c,
                                                            Name = c
                                                        }.ToDynamic()).ToList();
                }

                return config.Documents.Where(predicate).Select(c => new
                                                                     {
                                                                         Id = c,
                                                                         Name = c
                                                                     }.ToDynamic()).ToList();
            }


            if (metadataType == MetadataType.Schema)
            {
                return new[] { config.GetSchemaVersion(objectMetadataId) };
            }

            if (metadataType == MetadataType.View)
            {
                return predicate == null
                    ? config.GetViews(objectMetadataId)
                    : config.GetView(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.PrintView)
            {
                return predicate == null
                    ? config.GetPrintViews(objectMetadataId)
                    : config.GetPrintView(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.Generator)
            {
                return predicate == null
                    ? config.GetGenerators(objectMetadataId)
                    : config.GetGenerator(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.Scenario)
            {
                return predicate == null
                    ? config.GetScenarios(objectMetadataId)
                    : config.GetScenario(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.Process)
            {
                return predicate == null
                    ? config.GetProcesses(objectMetadataId)
                    : config.GetProcess(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.Service)
            {
                return predicate == null
                    ? config.GetServices(objectMetadataId)
                    : config.GetService(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.ValidationError)
            {
                return predicate == null
                    ? config.GetValidationErrors(objectMetadataId)
                    : config.GetValidationError(objectMetadataId, predicate);
            }
            if (metadataType == MetadataType.ValidationWarning)
            {
                return predicate == null
                    ? config.GetValidationWarnings(objectMetadataId)
                    : config.GetValidationWarning(objectMetadataId, predicate);
            }

            return null;
        }

        /// <summary>
        /// Обновить метаданные конфигурации
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="metadataType">Идентификатор типа метаданных</param>
        /// <param name="metadataName">Идентификатор изменяемых метаданных</param>
        public void UpdateMetadata(string configId, string documentId, string metadataType, string metadataName)
        {
            var config = _metadataConfigurationProvider.GetMetadataConfiguration(configId);

            if (config != null)
            {
                if (metadataType == MetadataType.Configuration)
                {
                    // Регистрация нового документа в конфигурации
                    config.RegisterDocument(metadataName);
                    return;
                }

                var metadataReader = new ManagerFactoryDocument(configId, documentId).BuildMetadataReaderByType(metadataType);

                if (metadataType == MetadataType.View)
                {
                    config.RegisterView(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.PrintView)
                {
                    config.RegisterPrintView(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.Generator)
                {
                    config.RegisterGenerator(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.Scenario)
                {
                    config.RegisterScenario(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.Process)
                {
                    config.RegisterProcess(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.Service)
                {
                    config.RegisterService(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.ValidationError)
                {
                    config.RegisterValidationError(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.ValidationWarning)
                {
                    config.RegisterValidationWarning(documentId, metadataReader.GetItem(metadataName));
                }
                else if (metadataType == MetadataType.Status)
                {
                    config.RegisterStatus(documentId, metadataReader.GetItem(metadataName));
                }
            }
        }

        /// <summary>
        /// Удалить метаданные конфигурации
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="metadataType">Идентификатор типа метаданных</param>
        /// <param name="metadataName">Идентификатор удаляемых метаданных</param>
        public void DeleteMetadata(string configId, string documentId, string metadataType, string metadataName)
        {
            var config = _metadataConfigurationProvider.GetMetadataConfiguration(configId);

            if (config != null)
            {
                if (metadataType == MetadataType.View)
                {
                    config.UnregisterView(documentId, metadataName);
                }
                else if (metadataType == MetadataType.PrintView)
                {
                    config.UnregisterPrintView(documentId, metadataName);
                }
                else if (metadataType == MetadataType.Generator)
                {
                    config.UnregisterGenerator(documentId, metadataName);
                }
                else if (metadataType == MetadataType.Scenario)
                {
                    config.UnregisterScenario(documentId, metadataName);
                }
                else if (metadataType == MetadataType.Process)
                {
                    config.UnregisterProcess(documentId, metadataName);
                }
                else if (metadataType == MetadataType.Service)
                {
                    config.UnregisterService(documentId, metadataName);
                }
                else if (metadataType == MetadataType.ValidationError)
                {
                    config.UnregisterValidationError(documentId, metadataName);
                }
                else if (metadataType == MetadataType.ValidationWarning)
                {
                    config.UnregisterValidationWarning(documentId, metadataName);
                }
                else if (metadataType == MetadataType.Status)
                {
                    config.UnregisterStatus(documentId, metadataName);
                }
            }
        }
    }
}