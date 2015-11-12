using System.Linq;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Получить клиентскую сессию с указанным идентификатором
    /// </summary>
    public sealed class ActionUnitGetSession
    {
        public void Action(IApplyContext target)
        {
            ITransaction transaction =
                target.Context.GetComponent<ITransactionComponent>()
                      .GetTransactionManager()
                      .GetTransaction(target.Item.SessionId);

            target.Result = new DynamicWrapper();
            target.Result.Items = transaction.GetTransactionItems().Where(g => !g.Detached).Select(tr => new
                {
                    Application = tr.ConfigId,
                    DocumentType = tr.DocumentId,
                    Document = tr.Documents.FirstOrDefault()
                });
        }
    }
}