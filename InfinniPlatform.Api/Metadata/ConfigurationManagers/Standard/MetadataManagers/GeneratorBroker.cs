using System;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.ValidationBuilders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    ///     API для действий с генераторами.
    ///     Необходим для упрощения работы с генераторами, так как действия с метаданными генераторов
    ///     охватывают несколько типов метаданных элементов (сценарии, бизнес-процессы, сервисы)
    /// </summary>
    public sealed class GeneratorBroker
    {
        private readonly string _configurationId;
        private readonly string _documentId;
        private readonly string _version;

        public GeneratorBroker(string version, string configurationId, string documentId)
        {
            _version = version;
            _configurationId = configurationId;
            _documentId = documentId;
        }

        public void CreateGenerator(dynamic generatorObject)
        {
            generatorObject = DynamicWrapperExtensions.ToDynamic(generatorObject);

            var validator = ValidationBuilder.ForObject(builder => builder.And(rules => rules
                .IsNotNullOrEmpty("GeneratorName", Resources.GeneratorNameShouldNotBeEmpty)
                .IsNotNullOrEmpty("ActionUnit", Resources.ActionUnitShouldNotBeEmpty)
                .IsNotNullOrEmpty("MetadataType", Resources.MetadataTypeForGeneratorShouldNotBeEmpty)));

            var validationResult = new ValidationResult();

            if (validator.Validate((object) generatorObject, validationResult))
            {
                var eventObject = new
                {
                    generatorObject.GeneratorName,
                    generatorObject.ActionUnit,
                    Configuration = _configurationId,
                    Metadata = _documentId,
                    generatorObject.MetadataType,
                    ContextTypeKind = ContextTypeKind.ApplyMove
                };

                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "creategenerator", null, eventObject);
            }
            else
            {
                throw new ArgumentException(validationResult.ToDynamic().ToString());
            }
        }

        public void DeleteGenerator(string generatorName)
        {
            var body = new
            {
                Configuration = _configurationId,
                Metadata = _documentId,
                GeneratorName = generatorName
            };

            RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "deletegenerator", null, body);
        }
    }
}