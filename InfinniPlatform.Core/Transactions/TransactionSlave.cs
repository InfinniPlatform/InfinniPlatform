using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionSlave : ITransaction
    {
        private readonly string _transactionMarker;
		private readonly List<AttachedInstance> _itemsList;

        public TransactionSlave(string transactionMarker, List<AttachedInstance> itemsList)
        {
            _transactionMarker = transactionMarker;
            _itemsList = itemsList;
        }

        public void CommitTransaction()
        {
        }

        public void Attach(AttachedInstance item)
        {
            _itemsList.Add(item);
        }

        public string GetTransactionMarker()
        {
            return _transactionMarker;
        }

        public List<AttachedInstance> GetTransactionItems()
        {
            return _itemsList;
        }
    }
}
