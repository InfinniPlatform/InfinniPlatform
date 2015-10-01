using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionManager : ITransactionManager
    {
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly ISharedCacheComponent _sharedCache;
        private readonly IIndexFactory _indexFactory;

        private const string CacheTransactionKey = "___transactions";

        public TransactionManager(ISharedCacheComponent sharedCache, IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory)
        {
            var transactions = sharedCache.Get(CacheTransactionKey);
            if (transactions == null)
            {
                sharedCache.Lock();
                try
                {
                    if (sharedCache.Get(CacheTransactionKey) == null)
                    {
                        sharedCache.Set(CacheTransactionKey, new ConcurrentDictionary<string, ITransaction>());
                    }
                }
                finally
                {
                    sharedCache.Unlock();
                }
            }

            _sharedCache = sharedCache;
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
        }

        private ConcurrentDictionary<string, ITransaction> Transactions
        {
            get { return (ConcurrentDictionary<string, ITransaction>) _sharedCache.Get(CacheTransactionKey); }
        }

        /// <summary>
        ///     Зафиксировать транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        public void CommitTransaction(string transactionMarker)
        {
            Transactions[transactionMarker].MasterTransaction.CommitTransaction();
        }

        /// <summary>
        ///     Удалить транзакцию
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        public void RemoveTransaction(string transactionMarker)
        {
            if (Transactions.ContainsKey(transactionMarker))
            {
                RemoveTransaction(Transactions[transactionMarker]);
            }
        }

        private readonly object _syncTransactions = new object();

        /// <summary>
        ///     Получить транзакцию с указанным идентификатором
        /// </summary>
        /// <param name="transactionMarker">Идентификатор транзакции</param>
        /// <returns>Транзакция</returns>
        public ITransaction GetTransaction(string transactionMarker)
        {
            ITransaction result = null;

            if (Transactions.ContainsKey(transactionMarker))
            {
                var containedInstances = Transactions[transactionMarker].GetTransactionItems();
                var transactionSlave = new TransactionSlave(transactionMarker,
                    Transactions[transactionMarker].MasterTransaction, containedInstances);
                result = transactionSlave;
            }

            else
            {
                var transactionMaster = new TransactionMaster(_indexFactory, _blobStorageFactory, transactionMarker,
                    new List<AttachedInstance>())
                {
                    OnCommit = RemoveTransaction
                };

                result = transactionMaster;
                lock (_syncTransactions)
                {
                    Transactions.AddOrUpdate(transactionMarker, result, (key, oldValue) => result);
                }
            }

            return result;
        }

        private void RemoveTransaction(ITransaction removedTransaction)
        {
            var key = removedTransaction.GetTransactionMarker();
            if (Transactions.ContainsKey(key))
            {
                ITransaction transaction;
                Transactions.TryRemove(key, out transaction);
            }
        }
    }
}