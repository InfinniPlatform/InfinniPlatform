using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    /// <summary>
    /// Миграция позволяет удалить тестовые данные из индексов 
    /// </summary>
    public sealed class ClearTestDataMigration : IConfigurationMigration
    {
        /// <summary>
        /// Конфигурация, к которой применяется миграция
        /// </summary>
        private string _activeConfiguration;

        private List<string> configurationContainers;

        readonly List<MigrationParameter> _parameters = new List<MigrationParameter>();

        /// <summary>
        /// Текстовое описание миграции
        /// </summary>
        public string Description
        {
            get { return "Migration deletes data from specified types"; }
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
            var api = new DocumentApi();

            var resultMessage = new StringBuilder();
            
            var containers = configurationContainers.ToArray();

            for (int i = 0; i < containers.Length; i++)
            {
                if (parameters[i].ToString() == "True")
                {
                    var docs = api.GetDocument(_activeConfiguration, containers[i], null, 0, 10000);

                    foreach (var doc in docs)
                    {
                        api.DeleteDocument(_activeConfiguration, containers[i], doc.Id);
                    }
                }
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
            // Теоретически можно реализовать механизм отката миграции в случае необходимости:
            // нужно сохранять старые схемы документов в отдельном словаре и при откате возвращаться к ним

            throw new NotSupportedException();
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

	        IMetadataConfiguration metadataConfiguration = null;
	        if (configObject != null)
	        {
		        metadataConfiguration = configObject.MetadataConfiguration;
	        }

	        if (metadataConfiguration != null)
            {
                configurationContainers = new List<string>(metadataConfiguration.Containers);
                foreach (var containerId in configurationContainers)
                {
                    _parameters.Add(new MigrationParameter {Caption = containerId, InitialValue = true});
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
