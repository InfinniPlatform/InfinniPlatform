using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет импортировать справочник из федерального источника
    /// </summary>
    public sealed class ImportFederalClassifierMigration : IConfigurationMigration
    {
        readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();

        private bool _isInitialized;
             
        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration imports federal classifier by name"; }
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

            if (!_isInitialized)
            {
                resultMessage.AppendLine(
                    "Migration initializtion failed. Check that ClassifierLoader and ClassifierStorage configurations are installed.");
            }
            else
            {
                // Первым параметром передаётся идентификатор справочника
                var classifierOid =
                    parameters[0].ToString().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)[0];

                var item = new DynamicWrapper();
                item["ClassifierIds"] = new[] {classifierOid};
                item["ImportSource"] = "Federal";
                item["Overwrite"] = true;

                RestQueryApi.QueryPostJsonRaw("ClassifierLoader", "classifiers", "Publish", null, item);


                resultMessage.AppendLine();
                resultMessage.AppendFormat("Classifier {0} imported", parameters[0]);
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
            try
            {
                var oidsresponse = RestQueryApi.QueryGetRaw("ClassifierLoader", "classifiers", "Search", null, 0, 600);

                // Необходимо получить идентификаторы всех доступных справочников

                var classifierOids =
                    oidsresponse.Content.ToDynamicList().Select(i => string.Format("{0} ({1})", i["Oid"], i["Name"]));

                _parameters.Add(new MigrationParameter{ Caption = "Classifier", PossibleValues = classifierOids});

                _isInitialized = true;
            }
            catch
            {
                // Если не удалось получить список справочников, то и не получится их импортировать
                _isInitialized = false;
            }
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
