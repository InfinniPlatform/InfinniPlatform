using System;
using System.IO;
using System.Text;

namespace InfinniPlatform.Core.Serialization
{
    /// <summary>
    /// JSON-сериализатор объектов.
    /// </summary>
    public interface IJsonObjectSerializer : IObjectSerializer
    {
        /// <summary>
        /// Кодировка символов.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Сериализованное представление объекта.</param>
        /// <returns>Объект.</returns>
        object Deserialize(byte[] data);

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Поток для чтения сериализованного представление объекта.</param>
        /// <returns>Объект.</returns>
        object Deserialize(Stream data);

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Сериализованное представление объекта.</param>
        /// <returns>Объект.</returns>
        object Deserialize(string data);

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="data">Поток для чтения сериализованного представление объекта.</param>
        /// <param name="type">Тип объекта.</param>
        /// <returns>Объект.</returns>
        object Deserialize(string data, Type type);

        /// <summary>
        /// Преобразовать объект в строку.
        /// </summary>
        string ConvertToString(object value);

        /// <summary>
        /// Преобразовать строготипизированный объект в динамический.
        /// </summary>
        object ConvertToDynamic(object value);

        /// <summary>
        /// Преобразовать динамический объект в строготипизированный.
        /// </summary>
        object ConvertFromDynamic(object value, Type type);
    }
}