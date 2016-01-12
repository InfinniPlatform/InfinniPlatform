using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using InfinniPlatform.Core.Properties;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace InfinniPlatform.Core.Metadata.Validation
{
    /// <summary>
    /// Предоставляет метод для получения JSON-схемы объекта метаданных.
    /// </summary>
    internal sealed class MetadataJsonSchemaResolver : JsonSchemaResolver
    {
        private static readonly Assembly SchemaAssembly;
        private static readonly string SchemaNamespace;
        private static volatile JsonSchema _aggregatedSchema;

        static MetadataJsonSchemaResolver()
        {
            // Схемы метаданных берутся из ресурсов текущей сборки

            SchemaAssembly = Assembly.GetExecutingAssembly();
            SchemaNamespace = $"{SchemaAssembly.GetName().Name}..schema.";
        }

        /// <summary>
        /// Возвращает агрегированную схему.
        /// </summary>
        private static JsonSchema AggregatedSchema
        {
            get
            {
                if (_aggregatedSchema == null)
                {
                    lock (typeof(MetadataJsonSchemaResolver))
                    {
                        if (_aggregatedSchema == null)
                        {
                            _aggregatedSchema = LoadAggregatedSchema();
                        }
                    }
                }

                return _aggregatedSchema;
            }
        }

        /// <summary>
        /// Возаращет схему объекта метаданных по ссылке.
        /// </summary>
        public override JsonSchema GetSchema(string reference)
        {
            var schemaId = GetSchemaIdByReference(reference);
            return AggregatedSchema.Properties[schemaId];
        }

        /// <summary>
        /// Возвращает идентификатор схемы по ссылке.
        /// </summary>
        private static string GetSchemaIdByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException("reference");
            }

            return reference.Split('/').Last();
        }

        /// <summary>
        /// Загружает агрегированную схему.
        /// </summary>
        private static JsonSchema LoadAggregatedSchema()
        {
            JsonSchema aggregatedSchema;

            var schemaInfos = LoadSchemaInfos();

            using (var stream = new MemoryStream())
            {
                // Формирование файла агрегированной схемы
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    using (var jWriter = new JsonTextWriter(writer))
                    {
                        // {
                        //    "properties": {
                        //       "SchemaId": SchemaObject,
                        //       "SchemaId": SchemaObject,
                        //       ...
                        //    }
                        // }

                        jWriter.WriteStartObject();
                        jWriter.WritePropertyName("properties");
                        jWriter.WriteStartObject();

                        foreach (var item in schemaInfos)
                        {
                            jWriter.WritePropertyName(item.SchemaId);
                            item.SchemaObject.WriteTo(jWriter);
                        }

                        jWriter.WriteEndObject();
                        jWriter.WriteEndObject();
                        jWriter.Flush();
                    }
                }

                stream.Position = 0;

                // Загрузка агрегированной схемы из файла
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    using (var jReader = new JsonTextReader(reader))
                    {
                        aggregatedSchema = JsonSchema.Read(jReader, new InternalJsonSchemaResolver());
                    }
                }
            }

            // Разрешение зависимостей загруженных схем
            if (aggregatedSchema.Properties != null)
            {
                foreach (var schema in aggregatedSchema.Properties)
                {
                    ResolveExtend(schema.Value);
                }
            }

            return aggregatedSchema;
        }

        /// <summary>
        /// Загружает информацию о схемах.
        /// </summary>
        private static IEnumerable<SchemaInfo> LoadSchemaInfos()
        {
            var schemaInfos = new Dictionary<string, SchemaInfo>();

            // Имена файлов ресурсов со схемами
            var schemaResourceNames =
                SchemaAssembly.GetManifestResourceNames()
                              .Where(i => i.StartsWith(SchemaNamespace) && i.EndsWith(".resjson"));

            foreach (var schemaResource in schemaResourceNames)
            {
                // Загрузка схемы из файла ресурсов
                var schemaObject = LoadSchemaObject(schemaResource);
                var schemaId = LoadSchemaId(schemaObject);

                // Схема не имеет идентификатора
                if (string.IsNullOrWhiteSpace(schemaId))
                {
                    throw new ArgumentException(string.Format(Resources.SchemaDoesNotContainIdProperty, schemaResource));
                }

                // Схема уже объявлена ранее
                if (schemaInfos.ContainsKey(schemaId))
                {
                    throw new InvalidOperationException(string.Format(Resources.SchemaIsAlreadyDeclared, schemaId,
                        schemaResource));
                }

                schemaInfos.Add(schemaId, new SchemaInfo
                                          {
                                              SchemaObject = schemaObject,
                                              SchemaId = schemaId
                                          });
            }

            return schemaInfos.Values;
        }

        /// <summary>
        /// Загружает объект схемы.
        /// </summary>
        private static JObject LoadSchemaObject(string schemaResource)
        {
            JObject schemaObject = null;
            Exception schemaObjectError = null;

            try
            {
                var schemaStream = SchemaAssembly.GetManifestResourceStream(schemaResource);

                if (schemaStream != null)
                {
                    using (var reader = new StreamReader(schemaStream))
                    {
                        using (var jReader = new JsonTextReader(reader))
                        {
                            schemaObject = JObject.Load(jReader);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                schemaObjectError = error;
            }

            if (schemaObject == null)
            {
                throw new ArgumentException(string.Format(Resources.SchemaIncorrectOrNotDeclared, schemaResource),
                    schemaObjectError);
            }

            return schemaObject;
        }

        /// <summary>
        /// Загружает идентификатор схемы.
        /// </summary>
        private static string LoadSchemaId(JObject schemaObject)
        {
            var jsonSchemaId = schemaObject["id"] as JValue;

            return (jsonSchemaId != null) ? jsonSchemaId.Value as string : null;
        }

        /// <summary>
        /// Разрешает зависимости схемы.
        /// </summary>
        /// <remarks>
        /// Вызвано недоработкой текущей реализации <see cref="JsonSchema" />.
        /// </remarks>
        private static void ResolveExtend(JsonSchema schema)
        {
            ResolveExtend(schema, new List<JsonSchema>());
        }

        private static void ResolveExtend(JsonSchema schema, ICollection<JsonSchema> processed)
        {
            if (!processed.Contains(schema))
            {
                processed.Add(schema);

                var schemaExtends = schema.Extends;
                schema.Extends = null;

                if (schemaExtends != null)
                {
                    if (schema.Properties == null)
                    {
                        schema.Properties = new Dictionary<string, JsonSchema>();
                    }

                    foreach (var baseShema in schemaExtends)
                    {
                        ResolveExtend(baseShema, processed);

                        if (baseShema.Properties != null)
                        {
                            foreach (var baseProperty in baseShema.Properties)
                            {
                                if (!schema.Properties.ContainsKey(baseProperty.Key))
                                {
                                    schema.Properties.Add(baseProperty);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Информация о схеме.
        /// </summary>
        private class SchemaInfo
        {
            public string SchemaId;
            public JObject SchemaObject;
        }


        /// <summary>
        /// Находит схему по ссылке.
        /// </summary>
        private class InternalJsonSchemaResolver : JsonSchemaResolver
        {
            public override JsonSchema GetSchema(string reference)
            {
                var schemaId = GetSchemaIdByReference(reference);
                return base.GetSchema(schemaId);
            }
        }
    }
}