using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.Transactions
{
    /// <summary>
    ///     Подчиненная транзакция.
    ///     Сохранение данных выполняется только в рамках мастер-транзакции.
    ///     Подчиненная транзакция только обновляет разделяемый список сохраняемых элементов данных
    /// </summary>
    public sealed class TransactionSlave : ITransaction
    {
        private readonly List<AttachedInstance> _itemsList;
        private readonly ITransaction _masterTransaction;
        private readonly string _transactionMarker;

        /// <summary>
        ///     Конструктор подчиненной транзакции
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        /// <param name="masterTransaction">Базовая транзакция</param>
        /// <param name="itemsList">Список элементов транзакции</param>
        public TransactionSlave(string transactionMarker, ITransaction masterTransaction,
            List<AttachedInstance> itemsList)
        {
            _transactionMarker = transactionMarker;
            _masterTransaction = masterTransaction;
            _itemsList = itemsList;
        }

        /// <summary>
        ///     Главная транзакция
        /// </summary>
        public ITransaction MasterTransaction
        {
            get { return _masterTransaction; }
        }

        /// <summary>
        ///     Присоединить файл к участнику транзакции, ссылающемуся на документ
        ///     с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля бинарных данных в схеме документа</param>
        /// <param name="stream">Файловый поток</param>
        public void AttachFile(string instanceId, string fieldName, Stream stream)
        {
            var attachedInstance = _itemsList.FirstOrDefault(i => i.ContainsInstance(instanceId));

            if (attachedInstance != null)
            {
                attachedInstance.AddFile(fieldName, stream);
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.InstanceNotFoundToAttachFile, instanceId));
            }
        }

        /// <summary>
        ///     Отсоединить файл от участника транзакции, ссылающегося на документ
        ///     с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля бинарных данных в схеме документа</param>
        public void DetachFile(string instanceId, string fieldName)
        {
            var attachedInstance = _itemsList.FirstOrDefault(i => i.ContainsInstance(instanceId));
            if (attachedInstance != null)
            {
                attachedInstance.RemoveFile(fieldName);
            }
        }

        /// <summary>
        ///     Зафиксировать транзакцию
        /// </summary>
        public void CommitTransaction()
        {
            //подчиненная транзакция не выполняет сохранение данных. За сохранение данных отвечает мастер-транзакция
        }

        /// <summary>
        ///     Отсоединить документ от транзакции
        /// </summary>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public void Detach(string instanceId)
        {
            var itemDetached = _itemsList.FirstOrDefault(i => i.Documents.Any(a => a.Id.Equals(instanceId)));
            if (itemDetached != null)
            {
                itemDetached.Detached = true;
            }
        }

        /// <summary>
        ///     Присоединить документ к транзакции
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="documents">Присоединяемые документы</param>
        /// <param name="routing">Роутинг сохранения</param>
        public void Attach(string configId, string documentId, string version, IEnumerable<dynamic> documents,
            string routing)
        {
            _itemsList.Add(new AttachedInstance
            {
                Documents = documents,
                ConfigId = configId,
                DocumentId = documentId,
                Version = version,
                Routing = routing
            });
        }

        /// <summary>
        ///     Получить идентификатор транзакции
        /// </summary>
        /// <returns></returns>
        public string GetTransactionMarker()
        {
            return _transactionMarker;
        }

        /// <summary>
        ///     Получить список документов транзакции
        /// </summary>
        /// <returns>Список документов транзакции</returns>
        public List<AttachedInstance> GetTransactionItems()
        {
            return _itemsList;
        }
    }
}