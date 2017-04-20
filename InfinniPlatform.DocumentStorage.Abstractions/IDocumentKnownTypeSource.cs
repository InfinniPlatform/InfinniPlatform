using System;
using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Регистрация типов для создания маппингов классов в MongoDB для сериализации/десериализации.
    /// </summary>
    public interface IDocumentKnownTypeSource
    {
        /// <summary>
        /// Перечисление регистрируемых типов.
        /// </summary>
        IEnumerable<Type> KnownTypes { get; }
    }
}