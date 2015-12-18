using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет импортировать данные всех справочников
    /// </summary>
    public sealed class UploadClassifiersDataMigration : IConfigurationMigration
    {
        public UploadClassifiersDataMigration(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();
        private readonly RestQueryApi _restQueryApi;
        private IMetadataConfiguration _metadataConfiguration;

        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration allows to import classifiers data from folder"; }
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

            dynamic item = new DynamicWrapper();

            try
            {
                item.Configuration = "classifierstorage";
                var pathToFolder = parameters[0].ToString();

                foreach (
                    var classifier in Directory.EnumerateFiles(pathToFolder, "*.zip", SearchOption.TopDirectoryOnly))
                {
                    var containerName = Path.GetFileNameWithoutExtension(classifier.ToLowerInvariant())
                                            .Replace("classifierstorage_", "");

                    if (_metadataConfiguration.Documents.Select(c => c.ToLowerInvariant()).Contains(containerName))
                    {
                        item.Metadata = containerName;
                        item.FileContent = Convert.ToBase64String(File.ReadAllBytes(classifier));

                        _restQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "ImportDataFromJson", null, item);
                    }
                    else
                    {
                        resultMessage.AppendLine(string.Format("Classifier {0} metadata not found.", containerName));
                    }
                }

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Classifiers import completed");
            }
            catch (Exception e)
            {
                resultMessage.AppendLine("Migration failed due to: " + e.Message);
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
            _parameters.Add(new MigrationParameter { Caption = "Path to folder" });

            var configObject =
                context.GetComponent<IConfigurationMediatorComponent>()
                       .ConfigurationBuilder.GetConfigurationObject("classifierstorage");

            if (configObject != null)
            {
                _metadataConfiguration = configObject.MetadataConfiguration;
            }
        }
    }
}