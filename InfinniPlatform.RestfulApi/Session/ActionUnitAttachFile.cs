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
    ///   Присоединить файл к сессии
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
