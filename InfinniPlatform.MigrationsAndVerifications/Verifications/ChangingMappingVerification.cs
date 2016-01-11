using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Verifications
{
    /// <summary>
    /// Верификация позволяет найти документы, маппинг которых был изменен
    /// </summary>
    public sealed class ChangingMappingVerification : IConfigurationVerification
    {
        public ChangingMappingVerification(IIndexFactory indexFactory, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            _indexFactory = indexFactory;
            _configurationObjectBuilder = configurationObjectBuilder;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly IIndexFactory _indexFactory;

        /// <summary>
        /// Конфигурация, к которой применяется правило проверки
        /// </summary>
        private string _activeConfiguration;

        /// <summary>
        /// Текстовое описание правила проверки
        /// </summary>
        public string Description
        {
            get { return "Detects changes in configuration documents mapping"; }
        }

        /// <summary>
        /// Идентификатор конфигурации, к которой применима проверка.
        /// В том случае, если идентификатор не указан (null or empty string),
        /// проверка применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        /// Версия конфигурации, к которой применимо правило проверки.
        /// В том случае, если версия не указана (null or empty string),
        /// правило применимо к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <returns>Результат выполнения проверки</returns>
        public bool Check(out string message)
        {
            var result = true;
            var resultMessage = new StringBuilder();

            var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(_activeConfiguration).MetadataConfiguration;

            if (metadataConfiguration != null)
            {
                var containers = metadataConfiguration.Documents;
                foreach (var containerId in containers)
                {
                    var versionBuilder = _indexFactory.BuildVersionBuilder(
                        metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId));

                    var schema = metadataConfiguration.GetSchemaVersion(containerId);

                    var props = new List<PropertyMapping>();

                    if (schema != null)
                    {
                        // convert document schema to index mapping
                        props = DocumentSchemaHelper.ExtractProperties(schema.Properties, _configurationObjectBuilder);
                    }

                    if (!versionBuilder.VersionExists(props.Count > 0
                        ? props
                        : null))
                    {
                        result = false;

                        resultMessage.AppendLine();
                        resultMessage.AppendFormat("Version creation required for {0} document.", containerId);

                        if (new ManagerFactoryDocument(_activeConfiguration, containerId)
                            .BuildValidationWarningsMetadataReader().GetItems().Any())
                        {
                            resultMessage.AppendLine();
                            resultMessage.AppendFormat(
                                "{0} document contains validation warnings that may be invalid due recent document schema changes.",
                                containerId);
                        }

                        if (new ManagerFactoryDocument(_activeConfiguration, containerId)
                            .BuildValidationErrorsMetadataReader().GetItems().Any())
                        {
                            resultMessage.AppendLine();
                            resultMessage.AppendFormat(
                                "{0} document contains validation errors that may be invalid due recent document schema changes.",
                                containerId);
                        }

                        foreach (var process in
                            new ManagerFactoryDocument(_activeConfiguration, containerId)
                                .BuildProcessMetadataReader()
                                .GetItems())
                        {
                            if (process.Transitions != null)
                            {
                                foreach (var transition in DynamicWrapperExtensions.ToEnumerable(process.Transitions))
                                {
                                    if (transition.SchemaPrefill != null)
                                    {
                                        resultMessage.AppendLine();
                                        resultMessage.AppendFormat(
                                            "{0} document contains schema prefill (process-{1}, transition-{2}) that may be invalid due recent document schema changes.",
                                            containerId, process.Name, transition.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (result)
            {
                resultMessage.AppendLine();
                resultMessage.AppendFormat("Check succeeded");
            }

            message = resultMessage.ToString();

            return result;
        }

        /// <summary>
        /// Устанавливает активную конфигурацию для правила проверки
        /// </summary>
        public void AssignActiveConfiguration(string configurationId)
        {
            _activeConfiguration = configurationId;
        }
    }
}