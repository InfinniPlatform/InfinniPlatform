﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// JSON-сериализатор объектов.
    /// </summary>
    public class JsonObjectSerializer : IJsonObjectSerializer
    {
        private const int BufferSize = 1024;


        /// <summary>
        /// Кодировка по умолчанию.
        /// </summary>
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false);

        /// <summary>
        /// Экземпляр с настройками по умолчанию.
        /// </summary>
        public static readonly JsonObjectSerializer Default = new JsonObjectSerializer();

        /// <summary>
        /// Экземпляр с настройками форматирования.
        /// </summary>
        public static readonly JsonObjectSerializer Formatted = new JsonObjectSerializer(true);


        /// <summary>
        /// Initializes a new instance of <see cref="JsonObjectSerializer" />.
        /// </summary>
        /// <param name="withFormatting">Flag indicating if JSON-output will be formatted.</param>
        /// <param name="encoding">Text encoding.</param>
        /// <param name="knownTypes">Source of known types.</param>
        /// <param name="valueConverters">Custom converters for JSON properties.</param>
        /// <param name="errorHandlers">Serialization errors handlers.</param>
        public JsonObjectSerializer(bool withFormatting = false,
                                    Encoding encoding = null,
                                    IEnumerable<IKnownTypesSource> knownTypes = null,
                                    IEnumerable<IMemberValueConverter> valueConverters = null,
                                    IEnumerable<ISerializerErrorHandler> errorHandlers = null)
        {
            Encoding = encoding ?? DefaultEncoding;

            var serializer = new JsonSerializer
                             {
                                 NullValueHandling = NullValueHandling.Ignore,
                                 ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                                 Formatting = withFormatting ? Formatting.Indented : Formatting.None
                             };

            var propertyInitializers = new List<IJsonPropertyInitializer>();

            var knownTypesList = knownTypes?.ToList();

            if (knownTypesList != null && knownTypesList.Count > 0)
            {
                var knownTypesContainer = new KnownTypesContainer();

                foreach (var knownTypesSource in knownTypesList)
                {
                    knownTypesSource.AddKnownTypes(knownTypesContainer);
                }

                if (knownTypesContainer.Any())
                {
                    propertyInitializers.Add(new KnownTypesJsonConverterInitializer(knownTypesContainer));
                }
            }

            var valueConverterList = valueConverters?.ToList();

            if (valueConverterList != null && valueConverterList.Count > 0)
            {
                propertyInitializers.Add(new MemberValueJsonConverterInitializer(valueConverterList));
            }

            serializer.ContractResolver = new JsonDefaultContractResolver(propertyInitializers);

            var errorHandlerList = errorHandlers?.ToList();

            if (errorHandlerList != null && errorHandlerList.Count > 0)
            {
                serializer.Error += (s, e) =>
                                    {
                                        var context = e.ErrorContext;

                                        var target = context.OriginalObject;
                                        var member = context.Member;
                                        var error = context.Error;

                                        context.Handled = errorHandlerList.Any(h => h.Handle(target, member, error));
                                    };
            }

            serializer.Converters.Add(new DateJsonConverter());
            serializer.Converters.Add(new TimeJsonConverter());
            serializer.Converters.Add(new DynamicDocumentJsonConverter());

            _serializer = serializer;
        }


        private readonly JsonSerializer _serializer;


        /// <summary>
        /// Кодировка символов.
        /// </summary>
        public Encoding Encoding { get; }


        /// <summary>
        /// Сериализовать объект.
        /// </summary>
        /// <param name="value">Объект.</param>
        /// <returns>Сериализованное представление объекта.</returns>
        public byte[] Serialize(object value)
        {
            if (value != null)
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = CreateWriter(stream, false))
                    {
                        _serializer.Serialize(writer, value);
                    }

                    return stream.ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// Сериализовать объект.
        /// </summary>
        /// <param name="data">Поток для записи сериализованного представление объекта.</param>
        /// <param name="value">Объект.</param>
        public void Serialize(Stream data, object value)
        {
            if (value != null)
            {
                using (var writer = CreateWriter(data, true))
                {
                    _serializer.Serialize(writer, value);
                }
            }
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Сериализованное представление объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(byte[] data, Type type)
        {
            if (data != null)
            {
                using (var stream = new MemoryStream(data))
                {
                    using (var reader = CreateReader(stream, false))
                    {
                        return _serializer.Deserialize(reader, type);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Поток для чтения сериализованного представление объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(Stream data, Type type)
        {
            if (data != null)
            {
                using (var reader = CreateReader(data, true))
                {
                    return _serializer.Deserialize(reader, type);
                }
            }

            return null;
        }


        // Dynamic

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Сериализованное представление объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(byte[] data)
        {
            if (data != null)
            {
                using (var stream = new MemoryStream(data))
                {
                    using (var reader = CreateReader(stream, false))
                    {
                        using (var jReader = new JsonTextReader(reader))
                        {
                            return _serializer.Deserialize(jReader, typeof(DynamicDocument));
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Поток для чтения сериализованного представление объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(Stream data)
        {
            if (data != null)
            {
                using (var reader = CreateReader(data, true))
                {
                    using (var jReader = new JsonTextReader(reader))
                    {
                        return _serializer.Deserialize(jReader, typeof(DynamicDocument));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Сериализованное представление объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(string data)
        {
            if (data != null)
            {
                using (var reader = new StringReader(data))
                {
                    using (var jReader = new JsonTextReader(reader))
                    {
                        return _serializer.Deserialize(jReader, typeof(DynamicDocument));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Поток для чтения сериализованного представление объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Объект.</returns>
        public object Deserialize(string data, Type type)
        {
            if (data != null)
            {
                using (var reader = new StringReader(data))
                {
                    return _serializer.Deserialize(reader, type);
                }
            }

            return null;
        }


        /// <summary>
        /// Преобразовать объект в строку.
        /// </summary>
        public string ConvertToString(object value)
        {
            if (value != null)
            {
                if (value is string)
                {
                    return (string)value;
                }

                using (var writer = new StringWriter())
                {
                    _serializer.Serialize(writer, value);

                    return writer.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Преобразовать строготипизированный объект в динамический.
        /// </summary>
        public object ConvertToDynamic(object value)
        {
            if (value != null)
            {
                if (value is IDynamicMetaObjectProvider)
                {
                    return value;
                }

                using (var stream = new MemoryStream())
                {
                    using (var writer = CreateWriter(stream, true))
                    {
                        _serializer.Serialize(writer, value);
                    }

                    stream.Position = 0;

                    using (var reader = CreateReader(stream, true))
                    {
                        using (var jReader = new JsonTextReader(reader))
                        {
                            return _serializer.Deserialize(jReader, typeof(DynamicDocument));
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Преобразовать динамический объект в строготипизированный.
        /// </summary>
        public object ConvertFromDynamic(object value, Type type)
        {
            if (value != null)
            {
                if (value.IsInstanceOfType(type))
                {
                    return value;
                }

                var jValue = JToken.FromObject(value, _serializer);

                if (jValue != null)
                {
                    using (var jReader = new JTokenReader(jValue))
                    {
                        return _serializer.Deserialize(jReader, type);
                    }
                }
            }

            return null;
        }


        // Helpers


        private StreamReader CreateReader(Stream stream, bool leaveOpen)
        {
            return new StreamReader(stream, Encoding, true, BufferSize, leaveOpen);
        }

        private StreamWriter CreateWriter(Stream stream, bool leaveOpen)
        {
            return new StreamWriter(stream, Encoding, BufferSize, leaveOpen);
        }
    }
}