using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Index;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionManager : ITransactionManager
    {
        private readonly IIndexFactory _indexFactory;
        private readonly ConcurrentDictionary<string, ITransaction> _transactions = new ConcurrentDictionary<string, ITransaction>();


        public TransactionManager(IIndexFactory indexFactory)
        {
            _indexFactory = indexFactory;
        }

        public ITransaction GetTransaction(string transactionMarker)
        {
            ITransaction result = null;

            if (_transactions.ContainsKey(transactionMarker))
            {
                var containedIdList = _transactions[transactionMarker].GetTransactionItems();
                var transactionSlave = new TransactionSlave(transactionMarker,containedIdList);
                result = transactionSlave;
            }

            else
            {
                var transactionMaster = new TransactionMaster(_indexFactory, transactionMarker, new List<AttachedInstance>())
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

        public void Attach(string transactionMarker, AttachedInstance document)
        {
            if (_transactions.ContainsKey(transactionMarker))
            {
                _transactions[transactionMarker].Attach(document);
            }
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
