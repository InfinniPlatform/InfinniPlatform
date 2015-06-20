using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Модуль сохранения документов сессии
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