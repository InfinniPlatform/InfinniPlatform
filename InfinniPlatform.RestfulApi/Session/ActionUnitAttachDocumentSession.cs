using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Присоединить документ к клиентской сессии
    /// </summary>
    public sealed class ActionUnitAttachDocumentSession
    {
        public void Action(IApplyContext target)
        {
            var manager = target.Context.GetComponent<ITransactionComponent>().GetTransactionManager();

            if (!string.IsNullOrEmpty(target.Item.SessionId) &&
                target.Item.AttachedInfo.Document != null &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.Application) &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.DocumentType) &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.Document.Id))
            {
                ITransaction transaction = manager.GetTransaction(target.Item.SessionId);

				transaction.Attach(
                    target.Item.AttachedInfo.Application,
                    target.Item.AttachedInfo.DocumentType,
                    null,
                    new[] {target.Item.AttachedInfo.Document});

                target.Result = new DynamicWrapper();
                target.Result.Id = target.Item.AttachedInfo.Document.Id;
                target.Result.IsValid = true;
                target.Result.ValidationMessage = Resources.DocumentAttachedSuccessfully;
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.Id = target.Item.AttachedInfo.Document.Id;
                target.Result.IsValid = false;
                target.Result.ValidationMessage = Resources.SessionIdAndDocumentShouldntBeEmpty;
            }
        }
    }
}