using System;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitSetDocument
    {
        public ActionUnitSetDocument(ITransactionComponent transactionComponent)
        {
            _transactionManager = transactionComponent.GetTransactionManager();
        }


        private readonly ITransactionManager _transactionManager;


        public void Action(IApplyContext target)
        {
            var documents = target.Item.Documents;

            target.Result = new DynamicWrapper();

            if (documents != null)
            {
                foreach (var document in documents)
                {
                    if (document.Id == null)
                    {
                        document.Id = Guid.NewGuid();
                    }

                    target.Result.Id = document.Id;
                }

                if (!string.IsNullOrEmpty(target.TransactionMarker))
                {
                    var transaction = _transactionManager.GetTransaction(target.TransactionMarker);

                    transaction.Attach(target.Item.Configuration, target.Item.Metadata, documents);
                }
            }

            target.Result.IsValid = true;
            target.Result.ValidationMessage = string.Empty;
        }
    }
}