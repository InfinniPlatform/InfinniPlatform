using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.Transactions
{
    /// <summary>
    ///   Мастер-транзакция, отвечающая за сохранение данных по завершении
    /// </summary>
    public sealed class TransactionMaster : ITransaction
    {
        private readonly string _transactionMarker;
	    private readonly List<AttachedInstance> _itemsList;
        private readonly IIndexFactory _indexFactory;

        /// <summary>
        ///   Конструктор мастер-транзакции
        /// </summary>
        /// <param name="indexFactory">Фабрика для работы с индексами</param>
        /// <param name="transactionMarker">Идентификатор создаваемой транзакции</param>
        /// <param name="itemsList">Разделяемый между различными экземплярами ITransaction список присоединенных элементов</param>
	    public TransactionMaster(IIndexFactory indexFactory, string transactionMarker, List<AttachedInstance> itemsList)
        {
            _indexFactory = indexFactory;
	        _transactionMarker = transactionMarker;
	        _itemsList = itemsList;
        }

        /// <summary>
        ///   Зафиксировать транзакцию
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                foreach (var item in _itemsList.Where(i => !i.Detached).ToList())
                {
	                
                    IVersionProvider versionProvider = _indexFactory.BuildVersionProvider(item.ConfigId, item.DocumentId, item.Routing);

					versionProvider.SetDocuments(item.Documents);
                }


                if (OnCommit != null)
                {
                    OnCommit(this);
                }

            }
            catch (Exception e)
            {
                
                throw new ArgumentException("Fail to commit transaction: " + e.Message);
            }
        }

        /// <summary>
        ///   Отсоединить документ от транзакции
        /// </summary>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public void Detach(string instanceId)
        {
            var itemDetached = _itemsList.FirstOrDefault(i => i.Documents.Any(a => a.IdId.Equals(instanceId)));
            if (itemDetached != null)
            {
                itemDetached.Detached = true;
            }

        }

        /// <summary>
        ///   Присоединить документ к транзакции
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="document">Присоединяемые документы</param>
        /// <param name="routing">Роутинг сохранения</param>
        public void Attach(string configId, string documentId, string version, IEnumerable<dynamic> document, string routing)
        {
            _itemsList.Add(new AttachedInstance()
            {
                Documents = document,
                ConfigId = configId,
                DocumentId = documentId,
                Version = version,
                Routing = routing
            });
        }

        /// <summary>
        ///   Главная транзакция
        /// </summary>
        public ITransaction MasterTransaction
        {
            get { return this; }
        }

        /// <summary>
        ///   Получить идентификатор транзакции
        /// </summary>
        /// <returns></returns>
        public string GetTransactionMarker()
        {
            return _transactionMarker;
        }

        /// <summary>
        ///   Получить список документов транзакции
        /// </summary>
        /// <returns>Список документов транзакции</returns>
        public List<AttachedInstance> GetTransactionItems()
        {
            return _itemsList;
        }

        public Action<ITransaction> OnCommit { get; set; }
    }
}
