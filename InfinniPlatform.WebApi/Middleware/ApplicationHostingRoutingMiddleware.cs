using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.MultipartFormData;
using InfinniPlatform.WebApi.Middleware.RoleAuthHandlers;
using InfinniPlatform.WebApi.Middleware.SessionHandlers;
using InfinniPlatform.WebApi.Middleware.StandardHandlers;
using InfinniPlatform.WebApi.Middleware.UserAuthHandlers;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    ///   Модуль хостинга приложений на платформе
    /// </summary>
    public sealed class ApplicationHostingRoutingMiddleware : RoutingOwinMiddleware
    {
        public ApplicationHostingRoutingMiddleware(OwinMiddleware next)
            : base(next)
        {
            RegisterHandler(new GetDocumentByIdHandlerRegistration());
            RegisterHandler(new GetDocumentHandlerRegistration());
            RegisterHandler(new DeleteDocumentHandlerRegistration());
            RegisterHandler(new ChangePasswordHandlerRegistration());
            RegisterHandler(new CustomServiceRegistrationHandler());
            RegisterHandler(new DenyAccessHandlerRegistration());
            RegisterHandler(new FileDownloadHandlerRegistration());
            RegisterHandler(new FileUploadHandlerRegistration());
            RegisterHandler(new GrantAccessHandlerRegistration());
            RegisterHandler(new SetDocumentHandlerRegistration());
            RegisterHandler(new SetDocumentsHandlerRegistration());
            RegisterHandler(new SignInRegistrationHandler());
            RegisterHandler(new SignOutRegistrationHandler());
            RegisterHandler(new UpdateDocumentHandlerRegistration());
            RegisterHandler(new AttachDocumentHandlerRegistration());
            RegisterHandler(new CreateSessionHandlerRegistration());
            RegisterHandler(new DetachDocumentHandlerRegistration());
            RegisterHandler(new FileAttachHandlerRegistration());
            RegisterHandler(new FileDetachHandlerRegistration());
            RegisterHandler(new GetSessionByIdHandlerRegistration());
            RegisterHandler(new SessionCommitHandlerRegistration());
            RegisterHandler(new SessionRemoveHandlerRegistration());
            RegisterHandler(new AddUserClaimHandlerRegistration());
            RegisterHandler(new AddUserHandlerRegistration());
            RegisterHandler(new AddUserRoleHandlerRegistration());
            RegisterHandler(new DeleteUserHandlerRegistration());
            RegisterHandler(new DeleteUserRoleHandlerRegistration());
            RegisterHandler(new AddRoleHandlerRegistration());
            RegisterHandler(new DeleteRoleHandlerRegistration());
            RegisterHandler(new GetUserClaimHandlerRegistration());
            RegisterHandler(new GetUserHandlerRegistration());
            RegisterHandler(new RemoveUserClaimHandlerRegistration());
        }
    }
}
