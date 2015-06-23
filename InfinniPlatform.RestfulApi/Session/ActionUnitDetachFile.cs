using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Отсоединить файл от сессии
    /// </summary>
    public sealed class ActionUnitDetachFile
    {
        public void Action(IApplyContext target)
        {
            ITransaction transaction =
                target.Context.GetComponent<ITransactionComponent>(target.Version)
                      .GetTransactionManager()
                      .GetTransaction(target.Item.SessionId);

            transaction.DetachFile(target.Item.InstanceId, target.Item.FieldName);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "File detached successfully.";
        }
    }
}