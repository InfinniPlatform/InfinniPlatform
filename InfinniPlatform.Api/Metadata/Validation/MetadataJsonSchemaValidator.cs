using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace InfinniPlatform.Api.Metadata.Validation
{
    /// <summary>
    ///     Сервис проверки корректности JSON-схемы объекта метаданных.
    /// </summary>
    internal sealed class MetadataJsonSchemaValidator : IMetadataSchemaValidator
    {
        private static readonly MetadataJsonSchemaResolver MetadataSchemaResolver = new MetadataJsonSchemaResolver();
        private readonly bool _detailedErrors;
        private readonly JsonSchema _metadataSchema;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="metadataType">Тип метаданных.</param>
        /// <param name="detailedErrors">Детализированные ошибки.</param>
        public MetadataJsonSchemaValidator(string metadataType, bool detailedErrors)
        {
            if (string.IsNullOrWhiteSpace(metadataType))
            {
                throw new ArgumentNullException("metadataType");
            }

            _metadataSchema = MetadataSchemaResolver.GetSchema(metadataType);
            _detailedErrors = detailedErrors;
        }

        /// <summary>
        ///     Проверяет корректность схемы объекта метаданных.
        /// </summary>
        /// <param name="metadataValue">Объект метаданных.</param>
        /// <exception cref="MetadataSchemaException"></exception>
        public void Validate(object metadataValue)
        {
            if (metadataValue == null)
            {
                throw new ArgumentNullException("metadataValue");
            }

            var metadataJson = (metadataValue as JObject) ?? JObject.FromObject(metadataValue);
            var metadataErrors = new List<MetadataSchemaError>();

            try
            {
                Validate(metadataJson, _metadataSchema, (o, e) => metadataErrors.Add(CreateSchemaError(e)));
            }
            catch (Exception error)
            {
                throw new MetadataSchemaException(error.Message, null, error);
            }

            if (metadataErrors.Count > 0)
            {
                throw new MetadataSchemaException(metadataErrors[0].Message, metadataErrors);
            }
        }

        private static MetadataSchemaError CreateSchemaError(ValidationEventArgs errorInfo)
        {
            return (errorInfo.Exception != null)
                ? new MetadataSchemaError(errorInfo.Exception.Message, errorInfo.Exception.Path,
                    errorInfo.Exception.LineNumber, errorInfo.Exception.LinePosition)
                : new MetadataSchemaError(errorInfo.Message, errorInfo.Path, 0, 0);
        }

        private void Validate(JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
        {
            var sourceReader = _detailedErrors
                ? new JsonTextReader(new StringReader(source.ToString()))
                : source.CreateReader();

            using (var reader = new JsonValidatingReader(sourceReader))
            {
                reader.Schema = schema;

                if (validationEventHandler != null)
                {
                    reader.ValidationEventHandler += validationEventHandler;
                }

                while (reader.Read())
                {
                }
            }
        }
    }
}