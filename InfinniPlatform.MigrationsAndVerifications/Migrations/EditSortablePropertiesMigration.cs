using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет указать по каким полям документа можно выполнять сортировку
    /// </summary>
    public sealed class EditSortablePropertiesMigration : IConfigurationMigration
    {
        private readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();

        /// <summary>
        /// Конфигурация, к которой применяется миграция
        /// </summary>
        private string _activeConfiguration;

        private IMetadataConfiguration _metadataConfiguration;

        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration allows to customize which properties are sortable"; }
        }

        /// <summary>
        /// Идентификатор конфигурации, к которой применима миграция.
        /// В том случае, если идентификатор не указан (null or empty string),
        /// миграция применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        /// Версия конфигурации, к которой применима миграция.
        /// В том случае, если версия не указана (null or empty string),
        /// миграция применима к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        /// Признак того, что миграцию можно откатить
        /// </summary>
        public bool IsUndoable
        {
            get { return false; }
        }

        /// <summary>
        /// Выполнить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters"></param>
        public void Up(out string message, object[] parameters)
        {
            var resultMessage = new StringBuilder();

            var managerDocument =
                new ManagerFactoryConfiguration(_activeConfiguration).BuildDocumentManager();

            if (_metadataConfiguration != null)
            {
                var containers = _metadataConfiguration.Documents;

                var propertyIndex = 0;

                foreach (var containerId in containers)
                {
                    dynamic schema = _metadataConfiguration.GetSchemaVersion(containerId);

                    if (schema != null)
                    {
                        propertyIndex = AssignSortableProperty(schema.Properties, parameters, propertyIndex);

                        dynamic document = managerDocument.MetadataReader.GetItem(containerId);
                        document.Schema = schema;
                        managerDocument.MergeItem(document);
                    }
                }

                new UpdateApi().ForceReload(_activeConfiguration);

                var responce = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runmigration",
                    null,
                    new
                    {
                        MigrationName =
                            "UpdateStoreMigration",
                        ConfigurationName =
                            _activeConfiguration
                    });

                var updateStoreMigrationLines = responce.Content
                                                        .Replace("\\r", "\r")
                                                        .Replace("\\n", "\n")
                                                        .Replace("\"", "")
                                                        .Split(new[] { "\r\n", "\n" },
                                                               StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in updateStoreMigrationLines)
                {
                    resultMessage.AppendLine(line);
                }
            }

            message = resultMessage.ToString();
        }

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters">Параметры миграции</param>
        public void Down(out string message, object[] parameters)
        {
            // Теоретически можно реализовать механизм отката миграции в случае необходимости:
            // нужно сохранять старые схемы документов в отдельном словаре и при откате возвращаться к ним

            throw new NotSupportedException();
        }

        /// <summary>
        /// Возвращает параметры миграции
        /// </summary>
        public IEnumerable<MigrationParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string configurationId, IGlobalContext context)
        {
            _activeConfiguration = configurationId;
            var configObject =
                context.GetComponent<IConfigurationMediatorComponent>()
                       .ConfigurationBuilder.GetConfigurationObject(_activeConfiguration);

            if (configObject != null)
            {
                _metadataConfiguration = configObject.MetadataConfiguration;
            }

            if (_metadataConfiguration != null)
            {
                var containers = _metadataConfiguration.Documents;
                foreach (var containerId in containers)
                {
                    dynamic schema = _metadataConfiguration.GetSchemaVersion(containerId);

                    if (schema != null)
                    {
                        _parameters.AddRange(ExtractObjectChildProperties("", schema.Properties, containerId));
                    }
                }
            }
        }

        private static IEnumerable<MigrationParameter> ExtractObjectChildProperties(string rootName, dynamic properties,
                                                                                    string documentName)
        {
            var parameters = new List<MigrationParameter>();

            if (properties != null)
            {
                foreach (var propertyMapping in properties)
                {
                    string formattedPropertyName = string.IsNullOrEmpty(rootName)
                        ? string.Format("{0}", propertyMapping.Key)
                        : string.Format("{0}.{1}", rootName, propertyMapping.Key);

                    if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                    {
                        parameters.AddRange(ExtractObjectChildProperties(formattedPropertyName,
                            propertyMapping.Value.Properties, documentName));
                    }
                    else if (propertyMapping.Value.Type.ToString() == DataType.Array.ToString())
                    {
                        if (propertyMapping.Value.Items != null)
                        {
                            parameters.AddRange(ExtractObjectChildProperties(formattedPropertyName,
                                propertyMapping.Value.Items.Properties,
                                documentName));
                        }
                    }
                    else
                    {
                        var isSortingField = false;

                        if (propertyMapping.Value.Sortable != null)
                        {
                            isSortingField = propertyMapping.Value.Sortable;
                        }

                        parameters.Add(new MigrationParameter
                                       {
                                           Caption = string.Format("{0}: {1}", documentName, formattedPropertyName),
                                           InitialValue = isSortingField
                                       });
                    }
                }
            }

            return parameters.ToArray();
        }

        private static int AssignSortableProperty(dynamic properties, IList<object> isSortable, int currentIndex)
        {
            var propertyIndex = currentIndex;

            if (properties == null)
            {
                return propertyIndex;
            }

            foreach (var propertyMapping in properties)
            {
                if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                {
                    propertyIndex = AssignSortableProperty(propertyMapping.Value.Properties, isSortable, propertyIndex);
                }
                else if (propertyMapping.Value.Type.ToString() == DataType.Array.ToString())
                {
                    if (propertyMapping.Value.Items != null)
                    {
                        propertyIndex = AssignSortableProperty(propertyMapping.Value.Items.Properties, isSortable,
                            propertyIndex);
                    }
                }
                else
                {
                    propertyMapping.Value.Sortable = isSortable[propertyIndex++].ToString() == "True";
                }
            }

            return propertyIndex;
        }
    }
}