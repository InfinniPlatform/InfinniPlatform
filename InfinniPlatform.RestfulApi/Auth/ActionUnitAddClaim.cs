using System;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitAddClaim
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            ApplicationUser user = storage.FindUserByName(target.Item.UserName);
            if (user == null)
            {
                throw new ArgumentException(string.Format("User \"{0}\" not found.", target.Item.UserName));
            }

            dynamic claim = storage.FindClaimType(target.Item.ClaimType);

            if (claim == null)
            {
                storage.AddClaimType(target.Item.ClaimType);
            }

            var overwrite = true;

            if (target.Item.Overwrite != null &&
                target.Item.Overwrite == false)
            {
                overwrite = false;
            }

            storage.AddUserClaim(user, target.Item.ClaimType, target.Item.ClaimValue, overwrite);

            //обновляем утверждение для указанного пользователя системы

            target.Context.GetComponent<ISecurityComponent>()
                  .UpdateClaim(target.Item.UserName, target.Item.ClaimType, target.Item.ClaimValue);
            target.Result = new DynamicWrapper();
            target.Result.ValidationMessage = "Claim added successfully";
            target.Result.IsValid = true;
        }
    }
}