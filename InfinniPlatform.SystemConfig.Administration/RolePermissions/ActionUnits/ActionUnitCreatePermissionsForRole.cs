using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Administration.RolePermissions.ActionUnits
{
    public sealed class ActionUnitCreatePermissionsForRole
    {
        public void Action(IApplyContext target)
        {
            var parameters = target.Item.Document;

            var api = target.Context.GetComponent<DocumentApi>();

            var sectionFull = api.GetDocument("Administration", "Section",
                f => f.AddCriteria(cr => cr.Property("Id").IsEquals(parameters.Section.Id)), 0, 1).FirstOrDefault();

            var aclApi = target.Context.GetComponent<AuthApi>();

            var roleName = parameters.Role.DisplayName;
            //необходимо реализовать bulk insert для вставки документов в aclapi. Добавление прав на 1 документ влечет создание 11 записей в acl
            if (sectionFull != null)
            {
                foreach (var docType in sectionFull.DocumentTypeList)
                {
                    if (parameters.PermissionRead == true)
                    {
                        aclApi.GrantAccess(roleName, docType.ConfigId, docType.DocumentId, "getdocument");
                    }
                    else
                    {
                        aclApi.DenyAccess(roleName, docType.ConfigId, docType.DocumentId, "getdocument");
                    }
                    aclApi.DenyAccessAll(docType.ConfigId, docType.DocumentId, "getdocument");

                    aclApi.GrantAccess(AuthorizationStorageExtensions.AdminRole, docType.ConfigId, docType.DocumentId,
                        "getdocument");

                    if (parameters.PermissionUpdate == true)
                    {
                        aclApi.GrantAccess(roleName, docType.ConfigId, docType.DocumentId, "setdocument");
                    }
                    else
                    {
                        aclApi.DenyAccess(roleName, docType.ConfigId, docType.DocumentId, "setdocument");
                    }

                    aclApi.DenyAccessAll(docType.ConfigId, docType.DocumentId, "setdocument");

                    aclApi.GrantAccess(AuthorizationStorageExtensions.AdminRole, docType.ConfigId, docType.DocumentId,
                        "setdocument");

                    if (parameters.PermissionCreate == true)
                    {
                        aclApi.GrantAccess(roleName, docType.ConfigId, docType.DocumentId, "setdocument");
                    }
                    else
                    {
                        aclApi.DenyAccess(roleName, docType.ConfigId, docType.DocumentId, "setdocument");
                    }


                    if (parameters.PermissionDelete == true)
                    {
                        aclApi.GrantAccess(roleName, docType.ConfigId, docType.DocumentId, "deletedocument");
                    }
                    else
                    {
                        aclApi.DenyAccess(roleName, docType.ConfigId, docType.DocumentId, "deletedocument");
                    }

                    aclApi.DenyAccessAll(docType.ConfigId, docType.DocumentId, "deletedocument");

                    aclApi.GrantAccess(AuthorizationStorageExtensions.AdminRole, docType.ConfigId, docType.DocumentId,
                        "deletedocument");
                }
            }

            target.Item.Document.RoleFullName = roleName;
            api.SetDocument("Administration", "RolePermissions", target.Item.Document);
        }
    }
}