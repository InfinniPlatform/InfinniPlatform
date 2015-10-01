using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Модуль сохранения документов сессии
    /// </summary>
    public sealed class ActionUnitSaveSession
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<ITransactionComponent>()
                  .GetTransactionManager()
                  .CommitTransaction(target.Item.SessionId);
        }
    }
}