using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///   Модуль создания новой сессии
    /// </summary>
    public sealed class ActionUnitCreateSession
    {
        public void Action(IApplyContext target)
        {
            string transactionMarker = Guid.NewGuid().ToString();

            target.Context.GetComponent<ITransactionComponent>(target.Version).GetTransactionManager().GetTransaction(transactionMarker);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = Resources.SessionCreatedSuccessfully;
            target.Result.SessionId = transactionMarker;
        }
    }
}
