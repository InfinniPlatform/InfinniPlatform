using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Environment.Transactions
{
    /// <summary>
    /// Объект транзакции
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// Главная транзакция
        /// </summary>
        ITransaction MasterTransaction { get; }

        /// <summary>
        /// Зафиксировать транзакцию
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Отсоединить документ от транзакции
        /// </summary>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        void Detach(string instanceId);

        /// <summary>
        /// Получить идентификатор транзакции
        /// </summary>
        /// <returns></returns>
        string GetTransactionMarker();

        /// <summary>
        /// Получить список документов транзакции
        /// </summary>
        /// <returns>Список документов транзакции</returns>
        List<AttachedInstance> GetTransactionItems();

        /// <summary>
        /// Присоединить документ к транзакции
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="documents">Присоединяемые документы</param>
        void Attach(string configId, string documentId, IEnumerable<dynamic> documents);

        /// <summary>
        /// Присоединить файл к участнику транзакции, ссылающемуся на документ
        /// с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля бинарных данных в схеме документа</param>
        /// <param name="stream">Файловый поток</param>
        void AttachFile(string instanceId, string fieldName, Stream stream);

        /// <summary>
        /// Отсоединить файл от участника транзакции, ссылающегося на документ
        /// с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля бинарных данных в схеме документа</param>
        void DetachFile(string instanceId, string fieldName);
    }
}