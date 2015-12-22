using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Environment.Transactions
{
    /// <summary>
    /// Присоединенный к транзакции экземпляр документа
    /// </summary>
    public sealed class AttachedInstance
    {
        private readonly object _syncObject = new object();

        /// <summary>
        /// Идентификатор конфигурации
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// Идентификатор типа документа
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Список сохраняемых документов
        /// </summary>
        public IEnumerable<dynamic> Documents { get; set; }

        /// <summary>
        /// Связанные с документами файлы
        /// </summary>
        public List<FileDescription> Files { get; } = new List<FileDescription>();

        /// <summary>
        /// Признак отсоединения от транзакции
        /// </summary>
        public bool Detached { get; set; }

        /// <summary>
        /// Добавить связанный с документом файл
        /// </summary>
        /// <param name="fieldName">Наименование поля</param>
        /// <param name="stream">Файловый поток, связанный с полем документа</param>
        public void AddFile(string fieldName, Stream stream)
        {
            Files.Add(new FileDescription
            {
                FieldName = fieldName,
                Bytes = ReadAllBytes(stream)
            });
        }

        private static byte[] ReadAllBytes(Stream stream)
        {
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                memory.Flush();

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Содержит ли присоединенный экземпляр документ с указанным идентификатором
        /// </summary>
        /// <param name="id">Идентификатор документа</param>
        /// <returns>Признак наличия документа с указанным идентификатором</returns>
        public bool ContainsInstance(object id)
        {
            return Documents.Any(i => CompareDocumentIds(i.Id, id));
        }

        /// <summary>
        /// Удалить файл из списка присоединенных
        /// </summary>
        /// <param name="fieldName">Наименование поля ссылки в документе</param>
        public void RemoveFile(string fieldName)
        {
            var fileDescription = Files.FirstOrDefault(f => f.FieldName == fieldName);

            if (fileDescription != null)
            {
                Files.RemoveItem(fileDescription);
            }
        }

        public void UpdateDocument(object id, object item)
        {
            lock (_syncObject)
            {
                var doc = Documents.FirstOrDefault(i => CompareDocumentIds(i.Id, id));

                if (doc != null)
                {
                    Documents = Documents.Where(i => !CompareDocumentIds(i.Id, id)).ToArray();
                    Documents = Documents.Concat(new[] { item });
                }
            }
        }

        private static bool CompareDocumentIds(object id1, object id2)
        {
            return Equals(id1, id2) || (id1 != null && id2 != null && string.Equals(id1.ToString(), id2.ToString(), StringComparison.OrdinalIgnoreCase));
        }
    }
}