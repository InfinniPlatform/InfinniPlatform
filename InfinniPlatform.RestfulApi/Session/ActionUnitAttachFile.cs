using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///     Присоединить файл к сессии
    /// </summary>
    public sealed class ActionUnitAttachFile
    {
        public void Action(IUploadContext target)
        {
            ITransaction transaction =
                target.Context.GetComponent<ITransactionComponent>(target.Version)
                      .GetTransactionManager()
                      .GetTransaction(target.LinkedData.SessionId);

            transaction.AttachFile(target.LinkedData.InstanceId, target.LinkedData.FieldName, target.FileContent);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = Resources.FileAttachedSuccessfully;
        }
    }
}