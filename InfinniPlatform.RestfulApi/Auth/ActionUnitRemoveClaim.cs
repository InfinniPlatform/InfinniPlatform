using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///Удаление утверждения оносительно пользователя (Claim)
    /// </summary>
    public sealed class ActionUnitRemoveClaim
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            var user = storage.FindUserByName(target.Item.UserName);
            if (user == null)
            {
                throw new ArgumentException(string.Format(Resources.UserToRemoveClaimNotFound, target.Item.UserName));
            }

            storage.RemoveUserClaim(user, target.Item.ClaimType);

            //обновляем пользователей системы
            target.Context.GetComponent<CachedSecurityComponent>().UpdateUsers();
            target.Result = new DynamicWrapper();
            target.Result.ValidationMessage = Resources.ClaimRemovedSuccessfully;
            target.Result.IsValid = true;

        }
    }
}
