using System.Collections.Generic;

namespace InfinniPlatform.Core.Schema
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
}