using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Удалить клиентскую сессию
    /// </summary>
    public sealed class ActionUnitRemoveSession
    {
        public void Action(IApplyContext target)
        {
            if (!string.IsNullOrEmpty(target.Item.SessionId))
            {
                target.Context.GetComponent<ITransactionComponent>(target.Version)
                      .GetTransactionManager()
                      .RemoveTransaction(target.Item.SessionId);
            }
        }
    }
}