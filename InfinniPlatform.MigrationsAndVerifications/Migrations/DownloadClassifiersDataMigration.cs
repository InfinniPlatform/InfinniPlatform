using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет экспортировать данные всех справочников
    /// </summary>
    public sealed class DownloadClassifiersDataMigration : IConfigurationMigration
    {
        readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();

        private IMetadataConfiguration _metadataConfiguration;
        
        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration allows to export classifiers data to folder"; }
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
                item.PathToZip = parameters[0].ToString();

                var classifiers = _metadataConfiguration.Containers.ToArray();
                
                for (int i = 0; i < classifiers.Length; i++)
                {
                    if (parameters[i].ToString() == "False") 
                        continue;

                    item.Metadata = classifiers[i];
                    RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "ExportDataToJson", null, item);
                }

                resultMessage.AppendLine();
                resultMessage.AppendFormat("Classifiers export completed");
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

            _parameters.Add(new MigrationParameter { Caption = "Path to folder" });

	        var configObject =
		        context.GetComponent<IConfigurationMediatorComponent>()
		               .ConfigurationBuilder.GetConfigurationObject("classifierstorage");

	        if (configObject != null)
	        {

		        _metadataConfiguration = configObject.MetadataConfiguration;
	        }

	        if (_metadataConfiguration != null)
            {
                foreach (var containerId in _metadataConfiguration.Containers)
                {
                    if (containerId.ToLowerInvariant() == "common")
                    {
                        continue;
                    }

                    _parameters.Add(new MigrationParameter
                    {
                        Caption = containerId, 
                        InitialValue = true
                    });
                }
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
