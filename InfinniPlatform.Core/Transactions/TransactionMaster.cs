using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Index;
using System;
using System.Collections.Generic;

namespace InfinniPlatform.Transactions
{
    public sealed class TransactionMaster : ITransaction
    {
        private readonly string _transactionMarker;
	    private readonly List<AttachedInstance> _itemsList;
        private readonly IIndexFactory _indexFactory;

	    public TransactionMaster(IIndexFactory indexFactory, string transactionMarker, List<AttachedInstance> itemsList)
        {
            _indexFactory = indexFactory;
	        _transactionMarker = transactionMarker;
	        _itemsList = itemsList;
        }

        public void CommitTransaction()
        {
            try
            {
                foreach (var item in _itemsList)
                {
	                
                    IVersionProvider versionProvider = _indexFactory.BuildVersionProvider(item.ConfigId, item.DocumentId, item.Routing);

	                if (item.Instance.Document != null)
	                {
		                versionProvider.SetDocument(item.Instance.Document);
	                }
					else if (item.Instance.Documents != null)
					{
						versionProvider.SetDocuments(item.Instance.Documents);
					}
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

        public Action<ITransaction> OnCommit { get; set; }
    }
}
