using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет импортировать справочник из локального файла
    /// </summary>
    public sealed class ImportLocalClassifierMigration : IConfigurationMigration
    {
        readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();
        
        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration imports classifier from local xls, xml or dbf file"; }
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
                var importSource = parameters[0].ToString();
                
                item["ImportSource"] = importSource;

                if (importSource == "Dbf")
                {
                    item["ClassifierCodePage"] = 866;
                }

                item["ClassifierContent"] =
                    Convert.ToBase64String(File.ReadAllBytes(parameters[1].ToString()));

                item["ClassifierMetadata"] =
                    Convert.ToBase64String(File.ReadAllBytes(parameters[2].ToString()));
                
                item["Overwrite"] = true;

                RestQueryApi.QueryPostJsonRaw("ClassifierLoader", "classifiers", "Publish", null, item);

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Classifier {0} imported", parameters[0]);

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
        /// Устанавливает активную конфигурацию для миграции
        /// </summary>
        public void AssignActiveConfiguration(string configurationId, IGlobalContext context)
        {
            _parameters.Add(new MigrationParameter { Caption = "Import source", PossibleValues = new[] { "Dbf", "Excel", "Xml" } });

            _parameters.Add(new MigrationParameter { Caption = "Path to content file" });

            _parameters.Add(new MigrationParameter { Caption = "Path to description file" });
        }

        /// <summary>
        /// Возвращает параметры миграции
        /// </summary>
        public IEnumerable<MigrationParameter> Parameters
        {
            get { return _parameters; }
        }
    }
}
