using System;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Удаление утверждения оносительно пользователя (Claim)
    /// </summary>
    public sealed class ActionUnitRemoveClaim
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            ApplicationUser user = storage.FindUserByName(target.Item.UserName);
            if (user == null)
            {
                throw new ArgumentException(string.Format(Resources.UserToRemoveClaimNotFound, target.Item.UserName));
            }

            storage.RemoveUserClaim(user, target.Item.ClaimType);

            //обновляем пользователей системы
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateUsers();
            target.Result = new DynamicWrapper();
            target.Result.ValidationMessage = Resources.ClaimRemovedSuccessfully;
            target.Result.IsValid = true;
        }
    }
}