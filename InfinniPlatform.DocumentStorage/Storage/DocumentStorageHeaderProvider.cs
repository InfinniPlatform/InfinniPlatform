using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageHeaderProvider : IDocumentStorageHeaderProvider
    {
        public DocumentStorageHeaderProvider(ITenantProvider tenantProvider, IUserIdentityProvider userIdentityProvider)
        {
            _tenantProvider = tenantProvider;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly ITenantProvider _tenantProvider;
        private readonly IUserIdentityProvider _userIdentityProvider;


        public void SetInsertHeader(DynamicWrapper document)
        {
            var userIdentity = _userIdentityProvider.GetUserIdentity();
            var userName = DocumentStorageHelpers.AnonymousUser;
            var userId = DocumentStorageHelpers.AnonymousUser;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                userName = userIdentity.Name;
                userId = userIdentity.GetUserId();
            }

            var header = new DynamicWrapper
            {
                ["_tenant"] = _tenantProvider.GetTenantId(),
                ["_created"] = DateTime.UtcNow,
                ["_createUser"] = userName,
                ["_createUserId"] = userId
            };

            document["_header"] = header;
        }

        public void SetReplaceHeader(DynamicWrapper document)
        {
            var userIdentity = _userIdentityProvider.GetUserIdentity();
            var userName = DocumentStorageHelpers.AnonymousUser;
            var userId = DocumentStorageHelpers.AnonymousUser;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                userName = userIdentity.Name;
                userId = userIdentity.GetUserId();
            }

            var header = (document["_header"] as DynamicWrapper) ?? new DynamicWrapper();

            header["_tenant"] = header["_tenant"] ?? _tenantProvider.GetTenantId();
            header["_created"] = header["_created"] ?? DateTime.UtcNow;
            header["_createUser"] = header["_createUser"] ?? userName;
            header["_createUserId"] = header["_createUserId"] ?? userId;

            header["_updated"] = DateTime.UtcNow;
            header["_updateUser"] = userName;
            header["_updateUserId"] = userId;

            header["_deleted"] = null;
            header["_deleteUser"] = null;
            header["_deleteUserId"] = null;

            document["_header"] = header;
        }

        public Action<IDocumentUpdateBuilder> SetUpdateHeader(Action<IDocumentUpdateBuilder> update)
        {
            return u =>
                   {
                       update?.Invoke(u);

                       var userIdentity = _userIdentityProvider.GetUserIdentity();
                       var userName = DocumentStorageHelpers.AnonymousUser;
                       var userId = DocumentStorageHelpers.AnonymousUser;

                       if (userIdentity != null && userIdentity.IsAuthenticated)
                       {
                           userName = userIdentity.Name;
                           userId = userIdentity.GetUserId();
                       }

                       u.Set("_header._updated", DateTime.UtcNow);
                       u.Set("_header._updateUser", userName);
                       u.Set("_header._updateUserId", userId);

                       u.Remove("_header._deleted");
                       u.Remove("_header._deleteUser");
                       u.Remove("_header._deleteUserId");
                   };
        }

        public Action<IDocumentUpdateBuilder> SetDeleteHeader()
        {
            return u =>
                   {
                       var userIdentity = _userIdentityProvider.GetUserIdentity();
                       var userName = DocumentStorageHelpers.AnonymousUser;
                       var userId = DocumentStorageHelpers.AnonymousUser;

                       if (userIdentity != null && userIdentity.IsAuthenticated)
                       {
                           userName = userIdentity.Name;
                           userId = userIdentity.GetUserId();
                       }

                       u.Set("_header._deleted", DateTime.UtcNow);
                       u.Set("_header._deleteUser", userName);
                       u.Set("_header._deleteUserId", userId);
                   };
        }
    }
}