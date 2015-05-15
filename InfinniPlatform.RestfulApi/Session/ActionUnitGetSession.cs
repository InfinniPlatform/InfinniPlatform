using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///   Получить клиентскую сессию с указанным идентификатором
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
                ConfigId = tr.ConfigId,
                DocumentId = tr.DocumentId,
                Document = tr.Documents.FirstOrDefault(),
                Version = tr.Version
            });
        }
    }
}
