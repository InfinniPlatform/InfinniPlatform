using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет обновить маппинг в хранилище документов после изменения схемы данных документов конфигурации
    /// </summary>
    public sealed class UpdateStoreMigration : IConfigurationMigration
    {
        public UpdateStoreMigration()
        {
            _indexFactory = new ElasticFactory();
        }

        private readonly IIndexFactory _indexFactory;

        /// <summary>
        /// Конфигурация, к которой применяется миграция
        /// </summary>
        private string _activeConfiguration;

        private IGlobalContext _context;

        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration updates store mapping after changing configuration documents data schema"; }
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

            var configObject = _context.GetComponent<IConfigurationMediatorComponent>()
                                       .ConfigurationBuilder.GetConfigurationObject(_activeConfiguration);

            IMetadataConfiguration metadataConfiguration = null;
            if (configObject != null)
            {
                metadataConfiguration = configObject.MetadataConfiguration;
            }

            if (metadataConfiguration != null)
            {
                var configurationObjectBuilder = _context.GetComponent<IConfigurationMediatorComponent>()
                                                         .ConfigurationBuilder;

                var documents = metadataConfiguration.Documents;
                foreach (var documentId in documents)
                {
                    resultMessage.AppendFormat(MigrationHelper.TryUpdateDocumentMappings(metadataConfiguration, configurationObjectBuilder, _indexFactory, documentId));
                }

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Migration completed.");
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
            get { return new MigrationParameter[0]; }
        }

        /// <summary>
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string configurationId, IGlobalContext context)
        {
            _activeConfiguration = configurationId;
            _context = context;
        }
    }
}