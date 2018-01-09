using System;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Security;
using InfinniPlatform.Session;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.DocumentStorage
{
    internal class DocumentStorageHeaderProvider : IDocumentStorageHeaderProvider
    {
        public DocumentStorageHeaderProvider(ITenantProvider tenantProvider, 
                                             IHttpContextAccessor httpContextAccessor)
        {
            _tenantProvider = tenantProvider;
            _httpContextAccessor = httpContextAccessor;
        }


        private readonly ITenantProvider _tenantProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public void SetInsertHeader(DynamicDocument document)
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            var header = new DynamicDocument
            {
                ["_tenant"] = _tenantProvider.GetTenantId(),
                ["_created"] = currentDate,
                ["_createUser"] = userInfo.Item1,
                ["_createUserId"] = userInfo.Item2
            };

            document["_header"] = header;
        }

        public void SetInsertHeader<TDocument>(TDocument document) where TDocument : Document
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            var header = new DocumentHeader
            {
                _tenant = _tenantProvider.GetTenantId(),
                _created = currentDate,
                _createUser = userInfo.Item1,
                _createUserId = userInfo.Item2
            };

            document._header = header;
        }


        public void SetReplaceHeader(DynamicDocument document)
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            var header = (document["_header"] as DynamicDocument) ?? new DynamicDocument();

            header["_tenant"] = header["_tenant"] ?? _tenantProvider.GetTenantId();
            header["_created"] = header["_created"] ?? currentDate;
            header["_createUser"] = header["_createUser"] ?? userInfo.Item1;
            header["_createUserId"] = header["_createUserId"] ?? userInfo.Item2;

            header["_updated"] = currentDate;
            header["_updateUser"] = userInfo.Item1;
            header["_updateUserId"] = userInfo.Item2;

            header["_deleted"] = null;
            header["_deleteUser"] = null;
            header["_deleteUserId"] = null;

            document["_header"] = header;
        }

        public void SetReplaceHeader<TDocument>(TDocument document) where TDocument : Document
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            var header = document._header ?? new DocumentHeader();

            header._tenant = header._tenant ?? _tenantProvider.GetTenantId();
            header._created = header._created ?? currentDate;
            header._createUser = header._createUser ?? userInfo.Item1;
            header._createUserId = header._createUserId ?? userInfo.Item2;

            header._updated = currentDate;
            header._updateUser = userInfo.Item1;
            header._updateUserId = userInfo.Item2;

            header._deleted = null;
            header._deleteUser = null;
            header._deleteUserId = null;

            document._header = header;
        }


        public Action<IDocumentUpdateBuilder> SetUpdateHeader(Action<IDocumentUpdateBuilder> update)
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            return u =>
                   {
                       update?.Invoke(u);

                       u.Set("_header._updated", currentDate);
                       u.Set("_header._updateUser", userInfo.Item1);
                       u.Set("_header._updateUserId", userInfo.Item2);

                       u.Remove("_header._deleted");
                       u.Remove("_header._deleteUser");
                       u.Remove("_header._deleteUserId");
                   };
        }

        public Action<IDocumentUpdateBuilder<TDocument>> SetUpdateHeader<TDocument>(Action<IDocumentUpdateBuilder<TDocument>> update) where TDocument : Document
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            return u =>
                   {
                       update?.Invoke(u);

                       u.Set(d => d._header._updated, currentDate);
                       u.Set(d => d._header._updateUser, userInfo.Item1);
                       u.Set(d => d._header._updateUserId, userInfo.Item2);

                       u.Remove(d => d._header._deleted);
                       u.Remove(d => d._header._deleteUser);
                       u.Remove(d => d._header._deleteUserId);
                   };
        }


        public Action<IDocumentUpdateBuilder> SetDeleteHeader()
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            return u =>
                   {
                       u.Set("_header._deleted", currentDate);
                       u.Set("_header._deleteUser", userInfo.Item1);
                       u.Set("_header._deleteUserId", userInfo.Item2);
                   };
        }

        public Action<IDocumentUpdateBuilder<TDocument>> SetDeleteHeader<TDocument>() where TDocument : Document
        {
            var userInfo = GetCurrentUserInfo();
            var currentDate = DateTime.UtcNow;

            return u =>
                   {
                       u.Set(d => d._header._deleted, currentDate);
                       u.Set(d => d._header._deleteUser, userInfo.Item1);
                       u.Set(d => d._header._deleteUserId, userInfo.Item2);
                   };
        }


        private Tuple<string, string> GetCurrentUserInfo()
        {
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
            var userName = DocumentStorageHelpers.AnonymousUser;
            var userId = DocumentStorageHelpers.AnonymousUser;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                userName = userIdentity.Name;
                userId = userIdentity.GetUserId();
            }

            return new Tuple<string, string>(userName, userId);
        }
    }
}