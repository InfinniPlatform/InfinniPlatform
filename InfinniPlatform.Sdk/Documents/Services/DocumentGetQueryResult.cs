﻿using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на получение документов.
    /// </summary>
    public class DocumentGetQueryResult : DocumentQueryResult
    {
        /// <summary>
        /// Список документов.
        /// </summary>
        public IEnumerable<object> Items { get; set; }

        /// <summary>
        /// Количество документов.
        /// </summary>
        public long? Count { get; set; }
    }
}