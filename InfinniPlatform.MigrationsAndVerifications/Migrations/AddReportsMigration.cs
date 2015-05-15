using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Metadata;
using System.Collections.Generic;
using System.Text;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция добавляет контейнер Reports к конфигурации
    /// </summary>
    public sealed class AddReportsMigration : IConfigurationMigration
    {
        private string _activeConfiguration;


        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration adds \"Reports\" container to configuration"; }
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
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Выполнить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters"></param>
        public void Up(out string message, object[] parameters)
        {
            var resultMessage = new StringBuilder();

            var configReader = new MetadataReaderConfiguration();

            var configMetadata = configReader.GetItem(_activeConfiguration);

            if (configMetadata.Reports == null)
            {
                configMetadata.Reports = new List<dynamic>();
                new MetadataManagerConfiguration(configReader).MergeItem(configMetadata);

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Reports container was added.");

                UpdateApi.ForceReload(_activeConfiguration);
            }

            resultMessage.AppendLine();
            resultMessage.AppendFormat("Migration completed.");

            message = resultMessage.ToString();
        }

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <param name="parameters">Параметры миграции</param>
        public void Down(out string message, object[] parameters)
        {
            // TODO: действия по удалению метаданных отчетов

            message = "Not implemented";
        }

        /// <summary>
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string configurationId, IGlobalContext context)
        {
            _activeConfiguration = configurationId;
        }

        /// <summary>
        /// Возвращает параметры миграции
        /// </summary>
        public IEnumerable<MigrationParameter> Parameters
        {
            get { return new MigrationParameter[0]; }
        }
    }
}
