﻿using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль для получения утверждений относительно пользователя
    /// </summary>
    public sealed class ActionUnitGetClaim
    {
        public void Action(IApplyContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.ClaimType = target.Item.ClaimType;
            target.Result.UserName = target.Item.UserName;
            target.Result.ClaimValue = target.Context.GetComponent<ISecurityComponent>(target.Version)
                                             .GetClaim(target.Item.ClaimType, target.Item.UserName);
        }
    }
}