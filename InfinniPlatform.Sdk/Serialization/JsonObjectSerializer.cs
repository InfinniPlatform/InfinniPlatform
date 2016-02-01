using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// JSON-сериализатор объектов.
    /// </summary>
    public sealed class JsonObjectSerializer : IObjectSerializer
    {
        private const int BufferSize = 1024;

        /// <summary>
        /// Экземпляр с настройками по умолчанию.
        /// </summary>
        public static readonly JsonObjectSerializer Default = new JsonObjectSerializer();

        /// <summary>
        /// Экземпляр с настройками форматирования.
        /// </summary>
        public static readonly JsonObjectSerializer Formated = new JsonObjectSerializer(true);

        public JsonObjectSerializer(bool withFormatting = false, KnownTypesContainer knownTypes = null)
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Formatting = withFormatting ? Formatting.Indented : Formatting.None
            };

            // При сериализации будут учитываться приватные поля
            var contractResolver = new DefaultContractResolver();
            serializer.ContractResolver = contractResolver;

            if (knownTypes != null)
            {
                serializer.Converters.Add(new KnownTypesJsonConverter(knownTypes));
            }

            _serializer = serializer;
        }


        private readonly JsonSerializer _serializer;


        // IObjectSerializer


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

                        writer.Flush();
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

                    writer.Flush();
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
                            return _serializer.Deserialize(jReader);
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
                        return _serializer.Deserialize(jReader);
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
                        return _serializer.Deserialize(jReader);
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
                using (var writer = new StringWriter())
                {
                    _serializer.Serialize(writer, value);

                    writer.Flush();

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
                using (var stream = new MemoryStream())
                {
                    using (var writer = CreateWriter(stream, true))
                    {
                        _serializer.Serialize(writer, value);

                        writer.Flush();
                    }

                    stream.Position = 0;

                    using (var reader = CreateReader(stream, true))
                    {
                        using (var jReader = new JsonTextReader(reader))
                        {
                            return _serializer.Deserialize(jReader);
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
                var jValue = JToken.FromObject(value);

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


        private static StreamReader CreateReader(Stream stream, bool leaveOpen)
        {
            return new StreamReader(stream, Encoding.UTF8, true, BufferSize, leaveOpen);
        }

        private static StreamWriter CreateWriter(Stream stream, bool leaveOpen)
        {
            return new StreamWriter(stream, Encoding.UTF8, BufferSize, leaveOpen);
        }
    }
}