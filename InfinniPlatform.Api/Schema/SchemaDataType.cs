using System.Collections.Generic;

namespace InfinniPlatform.Api.Schema
{
    public static class SchemaDataTypeExtensions
    {
        private static readonly Dictionary<string, SchemaDataType> schemaTypes = new Dictionary<string, SchemaDataType>
        {
            {"string", SchemaDataType.String},
            {"float", SchemaDataType.Float},
            {"array", SchemaDataType.Array},
            {"boolean", SchemaDataType.Boolean},
            {"datetime", SchemaDataType.DateTime},
            {"integer", SchemaDataType.Integer},
            {"object", SchemaDataType.Object}
        };

        public static SchemaDataType ConvertFromString(string schemaDataType)
        {
            if (string.IsNullOrEmpty(schemaDataType))
            {
                return SchemaDataType.String;
            }

            schemaDataType = schemaDataType.ToLowerInvariant();
            return schemaTypes.ContainsKey(schemaDataType) ? schemaTypes[schemaDataType] : SchemaDataType.String;
        }
    }

    /// <summary>
    ///     Типы данных.
    /// </summary>
    /// <remarks>
    ///     За основу была взята спецификация "JSON Schema".
    /// </remarks>
    public enum SchemaDataType
    {
        /// <summary>
        ///     Не определен.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Строка.
        /// </summary>
        String = 1,

        /// <summary>
        ///     Дробное число.
        /// </summary>
        Float = 2,

        /// <summary>
        ///     Целое число.
        /// </summary>
        Integer = 4,

        /// <summary>
        ///     Логическое значение.
        /// </summary>
        Boolean = 8,

        /// <summary>
        ///     Дата/время.
        /// </summary>
        DateTime = 16,

        /// <summary>
        ///     Объект.
        /// </summary>
        Object = 32,

        /// <summary>
        ///     Массив.
        /// </summary>
        Array = 64
    }
}