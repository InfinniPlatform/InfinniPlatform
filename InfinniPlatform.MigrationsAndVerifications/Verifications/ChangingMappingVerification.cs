using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.MigrationsAndVerifications.Helpers;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;

namespace InfinniPlatform.MigrationsAndVerifications.Verifications
{
    /// <summary>
    ///     Верификация позволяет найти документы, маппинг которых был изменен
    /// </summary>
    public sealed class ChangingMappingVerification : IConfigurationVerification
    {
        private readonly IIndexFactory _indexFactory;

        /// <summary>
        ///     Конфигурация, к которой применяется правило проверки
        /// </summary>
        private string _activeConfiguration;

        private IGlobalContext _context;
        private string _version;

        public ChangingMappingVerification()
        {
            _indexFactory = new ElasticFactory(new RoutingFactoryBase());
        }


        /// <summary>
        ///     Текстовое описание правила проверки
        /// </summary>
        public string Description
        {
            get { return "Detects changes in configuration documents mapping"; }
        }

        /// <summary>
        ///     Идентификатор конфигурации, к которой применима проверка.
        ///     В том случае, если идентификатор не указан (null or empty string),
        ///     проверка применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        ///     Версия конфигурации, к которой применимо правило проверки.
        ///     В том случае, если версия не указана (null or empty string),
        ///     правило применимо к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        ///     Выполнить проверку
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <returns>Результат выполнения проверки</returns>
        public bool Check(out string message)
        {
            bool result = true;
            var resultMessage = new StringBuilder();

            var metadataConfiguration =
                _context.GetComponent<IConfigurationMediatorComponent>(_version)
                        .ConfigurationBuilder.GetConfigurationObject(_version, _activeConfiguration)
                        .MetadataConfiguration;
            //_metadataConfigurationProvider.Configurations.FirstOrDefault(
            //	c => c.ConfigurationId == _activeConfiguration);

            if (metadataConfiguration != null)
            {
                var containers = metadataConfiguration.Containers;
                foreach (var containerId in containers)
                {
                    IVersionBuilder versionBuilder = _indexFactory.BuildVersionBuilder(
                        metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId),
                        metadataConfiguration.GetSearchAbilityType(containerId));

                    var schema = metadataConfiguration.GetSchemaVersion(containerId);

                    var props = new List<PropertyMapping>();

                    if (schema != null)
                    {
                        // convert document schema to index mapping
                        props = DocumentSchemaHelper.ExtractProperties(_version, schema.Properties,
                                                                       _context
                                                                           .GetComponent
                                                                           <IConfigurationMediatorComponent>(_version)
                                                                           .ConfigurationBuilder);
                    }

                    if (!versionBuilder.VersionExists(props.Count > 0 ? new IndexTypeMapping(props) : null))
                    {
                        result = false;

                        resultMessage.AppendLine();
                        resultMessage.AppendFormat("Version creation required for {0} document.", containerId);

                        if (new ManagerFactoryDocument(_version, _activeConfiguration, containerId)
                            .BuildValidationWarningsMetadataReader().GetItems().Any())
                        {
                            resultMessage.AppendLine();
                            resultMessage.AppendFormat(
                                "{0} document contains validation warnings that may be invalid due recent document schema changes.",
                                containerId);
                        }

                        if (new ManagerFactoryDocument(_version, _activeConfiguration, containerId)
                            .BuildValidationErrorsMetadataReader().GetItems().Any())
                        {
                            resultMessage.AppendLine();
                            resultMessage.AppendFormat(
                                "{0} document contains validation errors that may be invalid due recent document schema changes.",
                                containerId);
                        }

                        foreach (dynamic process in
                            new ManagerFactoryDocument(_version, _activeConfiguration, containerId)
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
        ///     Устанавливает активную конфигурацию для правила проверки
        /// </summary>
        public void AssignActiveConfiguration(string version, string configurationId, IGlobalContext context)
        {
            _context = context;
            _activeConfiguration = configurationId;
            _version = version;
        }
    }
}