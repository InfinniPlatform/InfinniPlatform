using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Index;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionManager : ITransactionManager
    {
        private readonly IIndexFactory _indexFactory;
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly ConcurrentDictionary<string, ITransaction> _transactions = new ConcurrentDictionary<string, ITransaction>();


        public TransactionManager(IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory)
        {
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
        }

        /// <summary>
        ///   Зафиксировать транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        public void CommitTransaction(string transactionMarker)
        {
            _transactions[transactionMarker].MasterTransaction.CommitTransaction();
        }

        /// <summary>
        ///  Удалить транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        public void RemoveTransaction(string transactionMarker)
        {
            if (_transactions.ContainsKey(transactionMarker))
            {
                RemoveTransaction(_transactions[transactionMarker]);
            }
        }

        /// <summary>
        ///   Получить транзакцию с указанным идентификатором
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        /// <returns>Транзакция</returns>
        public ITransaction GetTransaction(string transactionMarker)
        {
            ITransaction result = null;

            if (_transactions.ContainsKey(transactionMarker))
            {
                var containedInstances = _transactions[transactionMarker].GetTransactionItems();
                var transactionSlave = new TransactionSlave(transactionMarker,_transactions[transactionMarker].MasterTransaction, containedInstances);
                result = transactionSlave;
            }

            else
            {
                var transactionMaster = new TransactionMaster(_indexFactory, _blobStorageFactory, transactionMarker, new List<AttachedInstance>())
                {
                    OnCommit = RemoveTransaction
                };

                result = transactionMaster;
	            lock (_transactions)
	            {
		            _transactions.AddOrUpdate(transactionMarker, result, (key,oldValue) => result);
	            }
            }

            return result;
        }


        private void RemoveTransaction(ITransaction removedTransaction)
        {
            var key = removedTransaction.GetTransactionMarker();
            if (_transactions.ContainsKey(key))
            {
	            ITransaction transaction;
                _transactions.TryRemove(key,out transaction);
            }
        }
    }
}
