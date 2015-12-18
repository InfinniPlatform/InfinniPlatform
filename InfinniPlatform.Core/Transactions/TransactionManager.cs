using System.Collections.Concurrent;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionManager : ITransactionManager
    {
        public TransactionManager(IIndexFactory indexFactory, BinaryManager binaryManager, IPerformanceLog performanceLog)
        {
            _indexFactory = indexFactory;
            _binaryManager = binaryManager;
            _performanceLog = performanceLog;
            _transactions = new ConcurrentDictionary<string, ITransaction>();
        }

        private readonly IPerformanceLog _performanceLog;
        private readonly IIndexFactory _indexFactory;
        private readonly BinaryManager _binaryManager;
        private readonly object _syncTransactions = new object();
        private readonly ConcurrentDictionary<string, ITransaction> _transactions;

        /// <summary>
        /// Зафиксировать транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        public void CommitTransaction(string transactionMarker)
        {
            _transactions[transactionMarker].MasterTransaction.CommitTransaction();
        }

        /// <summary>
        /// Удалить транзакцию
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
        /// Получить транзакцию с указанным идентификатором
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        /// <returns>Транзакция</returns>
        public ITransaction GetTransaction(string transactionMarker)
        {
            ITransaction result;

            if (_transactions.ContainsKey(transactionMarker))
            {
                var containedInstances = _transactions[transactionMarker].GetTransactionItems();
                var transactionSlave = new TransactionSlave(transactionMarker, _transactions[transactionMarker].MasterTransaction, containedInstances);
                result = transactionSlave;
            }
            else
            {
                var transactionMaster = new TransactionMaster(_indexFactory, _binaryManager, _performanceLog, transactionMarker, new List<AttachedInstance>())
                {
                    OnCommit = RemoveTransaction
                };

                result = transactionMaster;

                lock (_syncTransactions)
                {
                    _transactions.AddOrUpdate(transactionMarker, result, (key, oldValue) => result);
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
                _transactions.TryRemove(key, out transaction);
            }
        }
    }
}