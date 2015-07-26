using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.Metadata;
using InfinniPlatform.WebApi.Middleware.Metadata.Configuration;
using InfinniPlatform.WebApi.Middleware.Metadata.Solution;
using InfinniPlatform.WebApi.Middleware.RoleAuthHandlers;
using InfinniPlatform.WebApi.Middleware.SessionHandlers;
using InfinniPlatform.WebApi.Middleware.StandardHandlers;
using InfinniPlatform.WebApi.Middleware.UserAuthHandlers;
using InfinniPlatform.WebApi.Middleware.VersionHandlers;
using Microsoft.Owin;

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
            RegisterHandler(new GetIrrelevantVersionHandlerRegistration());
            RegisterHandler(new SetRelevantVersionHandlerRegistration());
            
            RegisterHandler(new InsertSolutionHandlerRegistration());
            RegisterHandler(new UpdateSolutionHandlerRegistration());
            RegisterHandler(new DeleteSolutionHandlerRegistration());
            RegisterHandler(new GetSolutionHandlerRegistration());

            RegisterHandler(new InsertConfigHandlerRegistration());
            RegisterHandler(new UpdateConfigHandlerRegistration());
            RegisterHandler(new DeleteConfigHandlerRegistration());
            RegisterHandler(new GetConfigHandlerRegistration());
        }
    }
}
