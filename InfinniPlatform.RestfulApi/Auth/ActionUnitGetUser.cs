using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Получить пользователя по логину
    /// </summary>
    public sealed class ActionUnitGetUser
    {
        public void Action(IApplyContext target)
        {
            dynamic user = null;
            if (target.Item.FromCache)
            {
                user = target.Context
                             .GetComponent<ISecurityComponent>(target.Version)
                             .Users.Cast<dynamic>().FirstOrDefault(u => u.UserName == target.Item.UserName);
            }
            else
            {
                user = target.Context.GetComponent<DocumentApi>(target.Version)
                             .GetDocument(AuthorizationStorageExtensions.AuthorizationConfigId,
                                          AuthorizationStorageExtensions.UserStore,
                                          f =>
                                          f.AddCriteria(cr => cr.Property("UserName").IsEquals(target.Item.UserName)), 0,
                                          1)
                             .FirstOrDefault();
            }

            if (user != null)
            {
                target.Result = user;
            }
            else
            {
                target.ValidationMessage = string.Format(Resources.UnableToFindUser, target.Item.UserName);
                target.IsValid = false;
            }
        }
    }
}