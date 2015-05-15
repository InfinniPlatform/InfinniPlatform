﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///   Удалить клиентскую сессию 
    /// </summary>
    public sealed class ActionUnitRemoveSession
    {
        public void Action(IApplyContext target)
        {
            if (!string.IsNullOrEmpty(target.Item.SessionId))
            {
                target.Context.GetComponent<ITransactionComponent>()
                    .GetTransactionManager()
                    .RemoveTransaction(target.Item.SessionId);
            }
        }
    }
}
