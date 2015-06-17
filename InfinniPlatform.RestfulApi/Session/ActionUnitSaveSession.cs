using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    /// Модуль сохранения документов сессии
    /// </summary>
    public sealed class ActionUnitSaveSession
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<ITransactionComponent>(target.Version)
                .GetTransactionManager()
                .CommitTransaction(target.Item.SessionId);

        }
    }
}
