using System;
using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata.Validation
{
    /// <summary>
    ///     Исключение при наличии ошибок в схеме объекта метаданных.
    /// </summary>
    [Serializable]
    public sealed class MetadataSchemaException : Exception
    {
        public MetadataSchemaException(string message, IEnumerable<MetadataSchemaError> errors = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            Errors = errors ?? new List<MetadataSchemaError>();
        }

        /// <summary>
        ///     Список ошибок.
        /// </summary>
        public IEnumerable<MetadataSchemaError> Errors { get; private set; }
    }
}