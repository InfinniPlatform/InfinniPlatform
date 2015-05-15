using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Transactions
{
    /// <summary>
    ///   Объект транзакции
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        ///   Зафиксировать транзакцию
        /// </summary>
        void CommitTransaction();


        /// <summary>
        ///   Отсоединить документ от транзакции
        /// </summary>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        void Detach(string instanceId);

        /// <summary>
        ///   Получить идентификатор транзакции
        /// </summary>
        /// <returns></returns>
        string GetTransactionMarker();

        /// <summary>
        ///   Получить список документов транзакции
        /// </summary>
        /// <returns>Список документов транзакции</returns>
        List<AttachedInstance> GetTransactionItems();

        /// <summary>
        ///   Присоединить документ к транзакции
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="documents">Присоединяемые документы</param>
        /// <param name="routing">Роутинг сохранения</param>
        void Attach(string configId, string documentId, string version, IEnumerable<dynamic> documents, string routing);

        /// <summary>
        ///   Главная транзакция
        /// </summary>
        ITransaction MasterTransaction { get; }
    }
}
