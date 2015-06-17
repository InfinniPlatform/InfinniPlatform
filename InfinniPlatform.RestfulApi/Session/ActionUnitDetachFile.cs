using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///   Отсоединить файл от сессии
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
