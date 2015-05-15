using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.RestfulApi.Session
{
    /// <summary>
    ///   Присоединить документ к клиентской сессии
    /// </summary>
    public sealed class ActionUnitAttachDocumentSession
    {
        public void Action(IApplyContext target)
        {
            var manager = target.Context.GetComponent<ITransactionComponent>().GetTransactionManager();

            if (!string.IsNullOrEmpty(target.Item.SessionId) && 
                target.Item.AttachedInfo.Document != null &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.ConfigId) &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.DocumentId))
            {
               ITransaction transaction = manager.GetTransaction(target.Item.SessionId);

               transaction.Attach(
                   target.Item.AttachedInfo.ConfigId,
                   target.Item.AttachedInfo.DocumentId, 
                   target.Version,
                   new [] {target.Item.AttachedInfo.Document}, 
                   target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);

                target.Result = new DynamicWrapper();
                target.Result.IsValid = true;
                target.Result.ValidationMessage = Resources.DocumentAttachedSuccessfully;

            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = Resources.SessionIdAndDocumentShouldntBeEmpty;
            }
        }
    }
}
