using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

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
                target.Context.GetComponent<ITransactionComponent>(target.Version)
                      .GetTransactionManager()
                      .GetTransaction(target.Item.SessionId);

            target.Result = new DynamicWrapper();
            target.Result.Items = transaction.GetTransactionItems().Where(g => !g.Detached).Select(tr => new
                {
                    Application = tr.ConfigId,
                    DocumentType = tr.DocumentId,
                    Document = tr.Documents.FirstOrDefault(),
                    tr.Version
                });
        }
    }
}