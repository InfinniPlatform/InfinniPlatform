using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Utils
{
	public static class TransactionUtils
	{
		public static void ApplyTransactionMarker(IApplyContext target)
		{
            if (!string.IsNullOrEmpty(target.TransactionMarker))
            {
                dynamic docs = (target.Item.Document != null ? new[] { target.Item.Document } : null) ??
                               target.Item.Documents;

                if (docs != null)
                {
					target.Context.GetComponent<ITransactionComponent>().GetTransactionManager()
                          .GetTransaction(target.TransactionMarker)
                          .Attach(target.Item.Configuration,
                                  target.Item.Metadata,
                                  docs);
                }
            }
		}


	}
}
