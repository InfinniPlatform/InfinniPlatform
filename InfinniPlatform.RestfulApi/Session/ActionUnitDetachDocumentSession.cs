using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Отсоединить документ от сессии
    /// </summary>
    public sealed class ActionUnitDetachDocumentSession
    {
        public void Action(IApplyContext target)
        {
            var manager = target.Context.GetComponent<ITransactionComponent>().GetTransactionManager();

            if (!string.IsNullOrEmpty(target.Item.SessionId) && target.Item.AttachmentId != null)
            {
                ITransaction transaction = manager.GetTransaction(target.Item.SessionId);

                transaction.Detach(target.Item.AttachmentId);
            }
        }
    }
}