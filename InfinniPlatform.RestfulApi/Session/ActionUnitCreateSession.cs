using System;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Модуль создания новой сессии
    /// </summary>
    public sealed class ActionUnitCreateSession
    {
        public void Action(IApplyContext target)
        {
            string transactionMarker = Guid.NewGuid().ToString();

            target.Context.GetComponent<ITransactionComponent>()
                  .GetTransactionManager()
                  .GetTransaction(transactionMarker);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = Resources.SessionCreatedSuccessfully;
            target.Result.SessionId = transactionMarker;
        }
    }
}