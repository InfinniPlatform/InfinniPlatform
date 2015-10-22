using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.MigrationsAndVerifications.Helpers;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    ///     Миграция позволяет обновить маппинг в хранилище документов после изменения схемы данных документов конфигурации
    /// </summary>
    public sealed class UpdateStoreMigration : IConfigurationMigration
    {
        private readonly IIndexFactory _indexFactory;

        public UpdateStoreMigration()
        {
            _updatedContainers = new List<string>();
			_indexFactory = new ElasticFactory();
        }


        /// <summary>
        ///     Необходимо хранить имена контейнеров, для которых уже была создана новая версия, чтобы не сделать это несколько раз
        /// </summary>
        private readonly IList<string> _updatedContainers;

        /// <summary>
        ///     Конфигурация, к которой применяется миграция
        /// </summary>
        private string _activeConfiguration;

        private IGlobalContext _context;
        private string _version;

        /// <summary>
        ///     Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration updates store mapping after changing configuration documents data schema"; }
        }

        /// <summary>
        ///     Идентификатор конфигурации, к которой применима миграция.
        ///     В том случае, если идентификатор не указан (null or empty string),
        ///     миграция применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        ///     Версия конфигурации, к которой применима миграция.
        ///     В том случае, если версия не указана (null or empty string),
        ///     миграция применима к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        ///     Признак того, что миграцию можно откатить
        /// </summary>
        public bool IsUndoable
        {
            get { return false; }
        }

        /// <summary>
        ///     Выполнить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters"></param>
        public void Up(out string message, object[] parameters)
        {
            var resultMessage = new StringBuilder();

            _updatedContainers.Clear();

            var configObject =
                _context.GetComponent<IConfigurationMediatorComponent>()
                        .ConfigurationBuilder.GetConfigurationObject(_version, _activeConfiguration);

            IMetadataConfiguration metadataConfiguration = null;
            if (configObject != null)
            {
                metadataConfiguration = configObject.MetadataConfiguration;
            }

            if (metadataConfiguration != null)
            {
                IEnumerable<string> containers = metadataConfiguration.Containers;
                foreach (string containerId in containers)
                {
                    IVersionBuilder versionBuilder = _indexFactory.BuildVersionBuilder(
                        metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId));

                    dynamic schema = metadataConfiguration.GetSchemaVersion(containerId);

                    var props = new List<PropertyMapping>();

                    if (schema != null)
                    {
                        // convert document schema to index mapping
                        props = DocumentSchemaHelper.ExtractProperties(_version, schema.Properties,
                                                                       _context
                                                                           .GetComponent
                                                                           <IConfigurationMediatorComponent>()
                                                                           .ConfigurationBuilder);
                    }

                    if (!versionBuilder.VersionExists(props.Count > 0 ? new IndexTypeMapping(props) : null) &&
                        !_updatedContainers.Contains(metadataConfiguration.ConfigurationId + "_" + containerId))
                    {
                        resultMessage.AppendLine();
                        resultMessage.AppendFormat("Created new version of {0} document.", containerId);

                        versionBuilder.CreateVersion(false, props.Count > 0 ? new IndexTypeMapping(props) : null);

                        _updatedContainers.Add(metadataConfiguration.ConfigurationId + "_" + containerId);

                        // Необходимо создать новые версии для контейнеров документов, имеющих inline ссылки на измененный документ
                        UpdateContainersWithInlineLinks(metadataConfiguration.ConfigurationId, containerId,
                                                        resultMessage);
                    }
                }

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Migration completed.");
            }

            message = resultMessage.ToString();
        }

        /// <summary>
        ///     Отменить миграцию
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
        ///     Возвращает параметры миграции
        /// </summary>
        public IEnumerable<MigrationParameter> Parameters
        {
            get { return new MigrationParameter[0]; }
        }

        /// <summary>
        ///     Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string version, string configurationId, IGlobalContext context)
        {
            _version = version;
            _activeConfiguration = configurationId;
            _context = context;
        }

        /// <summary>
        ///     Метод обновляет маппинг контейнеров, документы в которых имеют inline ссылки на документы
        ///     с изменившейся структурой
        /// </summary>
        private void UpdateContainersWithInlineLinks(string configId, string documentId,
                                                     StringBuilder messagesIntegrator)
        {
            var configList =
                _context.GetComponent<IConfigurationMediatorComponent>()
                        .ConfigurationBuilder.GetConfigurationList();
            foreach (var metadataConfiguration in configList)
            {
                var containers = metadataConfiguration.Containers;
                foreach (var containerId in containers)
                {
                    IVersionBuilder versionBuilder = _indexFactory.BuildVersionBuilder(
                        metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId));

                    var schema = metadataConfiguration.GetSchemaVersion(containerId);

                    if (schema != null)
                    {
                        // Проверяем, имеется ли в схеме данных документа inline ссылка 
                        // на документ с documentId из конфигурации configId
                        if (DocumentSchemaHelper.CheckObjectForSpecifiedInline(schema, configId, documentId))
                        {
                            // convert document schema to index mapping
                            List<PropertyMapping> props = DocumentSchemaHelper.ExtractProperties(_version,
                                                                                                 schema.Properties,
                                                                                                 _context
                                                                                                     .GetComponent
                                                                                                     <
                                                                                                     IConfigurationMediatorComponent
                                                                                                     >()
                                                                                                     .ConfigurationBuilder);

                            if (!_updatedContainers.Contains(metadataConfiguration.ConfigurationId + "_" + containerId))
                            {
                                versionBuilder.CreateVersion(false, props.Count > 0 ? new IndexTypeMapping(props) : null);

                                _updatedContainers.Add(metadataConfiguration.ConfigurationId + "_" + containerId);

                                messagesIntegrator.AppendLine();
                                messagesIntegrator.AppendFormat(
                                    "Created new version of {0} document from configuration {1} due online link on other changed document.",
                                    containerId, ConfigurationId);
                            }
                        }
                    }
                }
            }
        }
    }
}