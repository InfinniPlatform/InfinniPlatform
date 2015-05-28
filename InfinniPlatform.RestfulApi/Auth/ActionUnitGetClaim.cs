using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль для получения утверждений относительно пользователя
    /// </summary>
    public sealed class ActionUnitGetClaim
    {
        public void Action(IApplyContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.ClaimType = target.Item.ClaimType;
            target.Result.UserName = target.Item.UserName;
            target.Result.ClaimValue = target.Context.GetComponent<ISecurityComponent>()
                .GetClaim(target.Item.ClaimType, target.Item.UserName);
        }
    }
}
