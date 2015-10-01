using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Transactions;

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
                target.Context.GetComponent<ITransactionComponent>()
                      .GetTransactionManager()
                      .GetTransaction(target.Item.SessionId);

            transaction.DetachFile(target.Item.InstanceId, target.Item.FieldName);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "File detached successfully.";
        }
    }
}