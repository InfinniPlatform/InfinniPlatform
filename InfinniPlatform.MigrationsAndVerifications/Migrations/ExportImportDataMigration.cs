using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет импортировать и экспортировать данные, относящиеся к определенной конфигурации
    /// </summary>
    public sealed class ExportImportDataMigration : IConfigurationMigration
    {
        public ExportImportDataMigration(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();
        private readonly RestQueryApi _restQueryApi;

        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration allows to import and export data"; }
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

            var item = new DynamicWrapper();

            try
            {
                var operationType = parameters[0].ToString();

                var configuration = parameters[1].ToString().ToLowerInvariant();
                var metadata = parameters[2].ToString().ToLowerInvariant();

                item["Configuration"] = configuration;
                item["Metadata"] = metadata;

                if (operationType == "Export")
                {
                    item["PathToZip"] = parameters[3].ToString();

                    _restQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "ExportDataToJson", null, item);
                }
                else if (operationType == "Import")
                {
                    item["FileContent"] = Convert.ToBase64String(
                        File.ReadAllBytes(Path.Combine(parameters[3].ToString(),
                            string.Format("{0}_{1}.zip", configuration, metadata))));

                    _restQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "ImportDataFromJson", null, item);
                }
                else
                {
                    resultMessage.AppendLine(string.Format("Unsupported operation type: {0}", operationType));
                }

                resultMessage.AppendLine();
                resultMessage.AppendFormat("{0} operation completed", parameters[0]);
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
            _parameters.Add(new MigrationParameter { Caption = "Operation", PossibleValues = new[] { "Export", "Import" } });

            _parameters.Add(new MigrationParameter { Caption = "Configuration" });

            _parameters.Add(new MigrationParameter { Caption = "Metadata" });

            _parameters.Add(new MigrationParameter { Caption = "Path to folder" });
        }
    }
}