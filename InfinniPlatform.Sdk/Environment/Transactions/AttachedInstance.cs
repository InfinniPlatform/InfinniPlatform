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

        private byte[] ReadAllBytes(Stream stream)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Содержит ли присоединенный экземпляр документ с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <returns>Признак наличия документа с указанным идентификатором</returns>
        public bool ContainsInstance(string instanceId)
        {
            return Documents.Any(d => d.Id != null && d.Id.Equals(instanceId));
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
                var doc = Documents.FirstOrDefault(f => f.Id == id);
                if (doc != null)
                {
                    Documents = Documents.Where(d => d.Id != id).ToArray();
                    Documents = Documents.Concat(new[] { item });
                }
            }
        }
    }
}