using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Security;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitAddClaim
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            var user = storage.FindUserByName(target.Item.UserName);
            if (user == null)
            {
                throw new ArgumentException(string.Format("User \"{0}\" not found.",target.Item.UserName));
            }

            dynamic claim = storage.FindClaimType(target.Item.ClaimType);

            if (claim == null)
            {
                storage.AddClaimType(target.Item.ClaimType);
            }

            storage.AddUserClaim(user, target.Item.ClaimType,target.Item.ClaimValue);

            //обновляем утверждение для указанного пользователя системы

            target.Context.GetComponent<ISecurityComponent>(target.Version).UpdateClaim(target.Item.UserName, target.Item.ClaimType, target.Item.ClaimValue);
            target.Result = new DynamicWrapper();
            target.Result.ValidationMessage = "Claim added successfully";
            target.Result.IsValid = true;

            
        }
    }
}
