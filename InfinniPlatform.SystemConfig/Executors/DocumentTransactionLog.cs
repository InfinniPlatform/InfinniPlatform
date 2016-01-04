using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.SystemConfig.Executors
{
    /// <summary>
    /// Журнал операций над документами при выполнении транзакции.
    /// </summary>
    internal sealed class DocumentTransactionLog
    {
        public DocumentTransactionLog()
        {
            _entriesSync = new object();
            _entries = new Dictionary<string, DocumentTransactionCommand>(StringComparer.OrdinalIgnoreCase);
        }


        private readonly object _entriesSync;
        private readonly Dictionary<string, DocumentTransactionCommand> _entries;


        /// <summary>
        /// Добавляет запись в журнал.
        /// </summary>
        public void EnqueueEntry(DocumentTransactionCommand entry)
        {
            var documentUid = $"{entry.Configuration}.{entry.DocumentType}.{entry.DocumentId}";

            lock (_entriesSync)
            {
                // Имеет смысл выполнять только последнюю операцию над экземпляром документа,
                // в противном случае при раздельном выполнении массового сохранения и удаления
                // может быть нарушен порядок выполнения операций.

                _entries[documentUid] = entry;
            }
        }

        /// <summary>
        /// Извлекает записи из журнала.
        /// </summary>
        public DocumentTransactionCommand[] DequeueEntries()
        {
            lock (_entriesSync)
            {
                var entries = _entries.Values.ToArray();

                _entries.Clear();

                return entries;
            }
        }
    }
}