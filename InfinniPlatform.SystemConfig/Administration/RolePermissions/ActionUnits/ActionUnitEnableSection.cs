using System.Linq;

using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Administration.RolePermissions.ActionUnits
{
    public sealed class ActionUnitEnableSection
    {
        public void Action(IApplyContext target)
        {
            var parameters = target.Item.Document;

            var api = target.Context.GetComponent<DocumentApi>();

            var sectionFull = api.GetDocument("Administration", "Section",
                f => f.AddCriteria(cr => cr.Property("Id").IsEquals(parameters.Id)), 0, 1).FirstOrDefault();

            var aclApi = target.Context.GetComponent<AuthApi>();

            //для роли Default устанавливаем запрет для всех пользователей
            if (sectionFull != null)
            {
                foreach (var docType in sectionFull.DocumentTypeList)
                {
                    aclApi.GrantAccess("Default", docType.ConfigId, docType.DocumentId, "getdocument");
                    aclApi.GrantAccess("Default", docType.ConfigId, docType.DocumentId, "setdocument");
                    aclApi.GrantAccess("Default", docType.ConfigId, docType.DocumentId, "setdocument");
                    aclApi.GrantAccess("Default", docType.ConfigId, docType.DocumentId, "deletedocument");
                }
            }
        }
    }
}