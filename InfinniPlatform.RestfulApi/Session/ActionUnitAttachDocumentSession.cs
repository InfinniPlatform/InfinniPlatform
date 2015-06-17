using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
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
            var manager = target.Context.GetComponent<ITransactionComponent>(target.Version).GetTransactionManager();

            if (!string.IsNullOrEmpty(target.Item.SessionId) && 
                target.Item.AttachedInfo.Document != null &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.Application) &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.DocumentType) &&
                !string.IsNullOrEmpty(target.Item.AttachedInfo.Document.Id))
            {
               ITransaction transaction = manager.GetTransaction(target.Item.SessionId);

               transaction.Attach(
                   target.Item.AttachedInfo.Application,
                   target.Item.AttachedInfo.DocumentType, 
                   target.Version,
                   new [] {target.Item.AttachedInfo.Document},
                   target.Context.GetComponent<ISecurityComponent>(target.Version).GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);

                target.Result = new DynamicWrapper();
                target.Result.Id = target.Item.AttachedInfo.Document.Id;
                target.Result.IsValid = true;
                target.Result.ValidationMessage = Resources.DocumentAttachedSuccessfully;

            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.Id = target.Item.AttachedInfo.Document.Id;
                target.Result.IsValid = false;
                target.Result.ValidationMessage = Resources.SessionIdAndDocumentShouldntBeEmpty;
            }
        }
    }
}
