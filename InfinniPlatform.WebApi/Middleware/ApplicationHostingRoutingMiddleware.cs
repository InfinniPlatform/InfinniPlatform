using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
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
            //порядок регистрации обработчиков на текущий момент определяет приоритет применения роутинга в случае совпадения пути, сформированного
            //в результате обработки шаблона

            RegisterGetRequestHandler(GetRestTemplateDocumentList, InvokeGetDocumentService);
            RegisterPostRequestHandler(GetRestTemplateSpecifiedDocument, InvokePostDocumentService);
            RegisterPutRequestHandler(GetRestTemplateSpecifiedDocument, InvokePutDocumentService);
            RegisterDeleteRequestHandler(GetRestTemplateDocumentDelete, InvokeDeleteDocumentService);

            RegisterPutRequestHandler(GetRestTemplateDocument, InvokePutDocumentsService);

            RegisterGetRequestHandler(GetRestTemplateDocumentById, InvokeGetByIdDocumentService);

            RegisterPostRequestHandler(GetRestTemplate, InvokeCustomServicePost);

            RegisterPostRequestHandler(GetRestTemplateFileUpload, InvokeFileUpload);
            RegisterGetRequestHandler(GetRestTemplateFileDownload, InvokeFileDownload);

            RegisterPostRequestHandler(GetSessionTemplate, InvokeFileAttach);
            RegisterDeleteRequestHandler(GetSessionTemplate, InvokeFileDetach);
            RegisterPutRequestHandler(GetSessionTemplate, InvokeSessionService);
            RegisterPostRequestHandler(GetSessionTemplateById, InvokeSessionServiceCommit);
            RegisterDeleteRequestHandler(GetSessionTemplateById, InvokeSessionServiceRemove);
            RegisterDeleteRequestHandler(GetSessionTemplateAttachmentById, InvokeDetachSessionDocumentService);
            RegisterPutRequestHandler(GetSessionTemplateById, InvokeAttachSessionDocumentService);
            RegisterGetRequestHandler(GetSessionTemplateById, InvokeGetSessionService);

            RegisterPostRequestHandler(GetRestTemplateSignIn,InvokeSignInService);
            RegisterPostRequestHandler(GetRestTemplateSignOut,InvokeSignOutService);
            RegisterPostRequestHandler(GetRestTemplateChangePassword, InvokeChangePassword);

            RegisterPostRequestHandler(GetRestTemplateGrantAccess, InvokeGrantAccessUser);
            RegisterPostRequestHandler(GetRestTemplateDenyAccess, InvokeDenyAccessUser);

            RegisterPutRequestHandler(GetRestTemplateAddUserClaim, InvokeAddUserClaim);
            RegisterDeleteRequestHandler(GetRestTemplateRemoveUserClaim, InvokeRemoveUserClaim);
            RegisterGetRequestHandler(GetRestTemplateGetUserClaim, InvokeGetUserClaim);
            
            RegisterPutRequestHandler(GetRestTemplateAddRole, InvokeAddRole);
            RegisterDeleteRequestHandler(GetRestTemplateDeleteRole, InvokeDeleteRole);

            RegisterPutRequestHandler(GetRestTemplateAddUser, InvokeAddUser);
            RegisterDeleteRequestHandler(GetRestTemplateDeleteUser, InvokeDeleteUser);
            RegisterGetRequestHandler(GetRestTemplateGetUser, InvokeGetUser);

            RegisterPutRequestHandler(GetRestTemplateAddUserRole, InvokeAddUserRole);
            RegisterDeleteRequestHandler(GetRestTemplateDeleteUserRole, InvokeDeleteUserRole);
        }

        private PathString GetVersionPath()
        {
            return new PathString("/_version_");
        }

        private PathString GetBaseApplicationPath()
        {
            return new PathString("/_version_/_application_");
        }

        private PathString GetUserPath()
        {
            return new PathString(GetBaseApplicationPath() + "/User");
        }

        private PathString GetRolePath()
        {
            return new PathString(GetBaseApplicationPath() + "/Role");
        }

        private PathString GetSpecifiedUserPath()
        {
            return new PathString(GetUserPath() + "/_userName_");
        }


        private PathString GetSpecifiedRolePath()
        {
            return new PathString(GetRolePath() + "/_roleName_");
        }

        private PathString GetSpecifiedUserRolePath()
        {
            return new PathString(GetSpecifiedUserPath() + "/Roles/_roleName_");
        }

        private PathString GetSpecifiedUserClaimPath()
        {
            return new PathString(GetSpecifiedUserPath() + "/Claims/_claimType_");
        }


        private PathStringProvider GetSessionTemplate(IOwinContext context)
        {
            return context.FormatSessionRoutePath(GetVersionPath()).Create(Priority.Standard);
        }


        private PathStringProvider GetSessionTemplateById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString(GetVersionPath() + "/_sessionId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetSessionTemplateAttachmentById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString(GetVersionPath() + "/_sessionId_/_attachmentId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplate(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_service_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocumentList(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateSpecifiedDocument(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocument(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_")).Create(Priority.Standard);
        }


        private PathStringProvider GetRestTemplateDocumentDelete(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocumentById(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateFileUpload(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/files/upload")).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateFileDownload(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/files/download")).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateSignIn(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetVersionPath() + "/signin")).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateSignOut(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetVersionPath() + "/signout")).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateChangePassword(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetVersionPath() + "/changepassword")).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateGrantAccess(IOwinContext context)
        {
            return
                context.FormatRoutePath(new PathString(GetUserPath() + "/GrantAccess"))
                    .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateDenyAccess(IOwinContext context)
        {
            return
                context.FormatRoutePath(new PathString(GetUserPath() + "/DenyAccess"))
                    .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateAddUserClaim(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserClaimPath())
                .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateRemoveUserClaim(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserClaimPath())
                .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateGetUserClaim(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserClaimPath())
                    .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateDeleteRole(IOwinContext context)
        {
            return context.FormatRoleAuthRoutePath(GetSpecifiedRolePath())
                .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateAddRole(IOwinContext context)
        {
            return context.FormatRoleAuthRoutePath(GetSpecifiedRolePath())
                    .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateAddUserRole(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserRolePath()).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateDeleteUserRole(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserRolePath()).Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateDeleteUser(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserPath())
                .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateAddUser(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetUserPath())
                    .Create(Priority.Higher);
        }

        private PathStringProvider GetRestTemplateGetUser(IOwinContext context)
        {
            return context.FormatUserAuthRoutePath(GetSpecifiedUserPath())
                    .Create(Priority.Higher);
        }

        private static IRequestHandlerResult InvokeFileUpload(IOwinContext context)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            dynamic linkedData = JObject.Parse(nameValueCollection.Get("linkedData"));


            using (var fileStream = new MultipartFormDataParser(context.Request.Body, Encoding.UTF8).Files.Select(
                        f => f.Data).First())
            {
                if (linkedData.InstanceId != null &&
                    linkedData.FieldName != null && 
                    linkedData.FileName != null)
                {

                    return
                        new ValueRequestHandlerResult(new UploadApi().UploadBinaryContent(linkedData.InstanceId.ToString(),
                            linkedData.FieldName.ToString(), linkedData.FileName.ToString(), fileStream));
                }
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreSpecified);
            }
        }

        private static IRequestHandlerResult InvokeFileDownload(IOwinContext context)
        {

            NameValueCollection nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            if (nameValueCollection.Get("Form") != null)
            {
                dynamic formData = JObject.Parse(HttpUtility.UrlDecode(nameValueCollection.Get("Form")));

                if (formData.InstanceId == null ||
                    formData.FieldName == null)
                {
                    throw new ArgumentException(Resources.NotAllRequestParamsAreSpecified);
                }

                return new ValueRequestHandlerResult(new UploadApi().DownloadBinaryContent(
                    formData.InstanceId.ToString(), formData.FieldName.ToString()));    
            }

            return new ErrorRequestHandlerResult(Resources.IncorrectDownloadRequest);
        }

        public static IRequestHandlerResult InvokeFileAttach(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            NameValueCollection nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            dynamic linkedData = JObject.Parse(nameValueCollection.Get("linkedData"));

            using (var fileStream = new MultipartFormDataParser(context.Request.Body, Encoding.UTF8).Files.Select(f => f.Data).First())
            {
                if (linkedData.InstanceId != null && linkedData.FieldName != null && linkedData.SessionId != null)
                {
                    return new ValueRequestHandlerResult(new SessionApi().AttachFile(routeDictionary["version"], linkedData, fileStream));
                }
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }
        }


        public static IRequestHandlerResult InvokeFileDetach(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.InstanceId != null && body.FieldName != null && body.SessionId != null)
            {
                return new ValueRequestHandlerResult(new SessionApi().DetachFile(routeDictionary["version"], body));
            }
            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);

        }

        private static IRequestHandlerResult InvokeCustomServicePost(IOwinContext context)
        {
            var routeDictionary = context.GetRouteDictionary();

            var body = JObject.Parse(ReadRequestBody(context).ToString());

            return new ValueRequestHandlerResult(RestQueryApi.QueryPostJsonRaw(
                routeDictionary["application"],routeDictionary["documentType"],routeDictionary["service"],null,body));
        }

        private static IRequestHandlerResult InvokeGrantAccessUser(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.Application != null && body.UserName != null && body.Application.ToString() != string.Empty && body.UserName.ToString() != string.Empty)
            {
                return new ValueRequestHandlerResult(new AuthApi().GrantAccess(
                    body.UserName.ToString(),
                    body.Application.ToString(),
                    body.DocumentType != null ? body.DocumentType.ToString() : null,
                    body.Service != null ? body.Service.ToString() : null,
                    body.InstanceId != null ? body.InstanceId.ToString() : null));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }

        private static IRequestHandlerResult InvokeDenyAccessUser(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.Application != null && body.UserName != null && body.Application.ToString() != string.Empty && body.UserName.ToString() != string.Empty)
            {
                return new ValueRequestHandlerResult(new AuthApi().GrantAccess(
                    body.UserName.ToString(),
                    body.Application.ToString(),
                    body.DocumentType != null ? body.DocumentType.ToString() : null,
                    body.Service != null ? body.Service.ToString() : null,
                    body.InstanceId != null ? body.InstanceId.ToString() : null));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }

        private static IRequestHandlerResult InvokeAddUserClaim(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.ClaimValue != null)
            {
                return new ValueRequestHandlerResult(new AuthApi().AddClaim(routeDictionary["userName"], routeDictionary["claimType"], body.ClaimValue.ToString()));
            }
            
            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }

        private static IRequestHandlerResult InvokeGetUserClaim(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new AuthApi().GetClaim(routeDictionary["userName"],routeDictionary["claimType"]));
        }

        private static IRequestHandlerResult InvokeRemoveUserClaim(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new AuthApi().RemoveClaim(routeDictionary["userName"], routeDictionary["claimType"]));
        }

        private static IRequestHandlerResult InvokeAddRole(IOwinContext context)
        {
            var routeDictionary = context.GetRoleAuthRouteDictionary();

            string roleName = routeDictionary["roleName"];

            return new ValueRequestHandlerResult(new UsersApi().AddRole(roleName, roleName, roleName));
        }

        private static IRequestHandlerResult InvokeAddUserRole(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new UsersApi().AddUserRole(routeDictionary["userName"], routeDictionary["roleName"]));
        }

        private static IRequestHandlerResult InvokeDeleteUserRole(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new UsersApi().AddUserRole(routeDictionary["userName"], routeDictionary["roleName"]));
        }
             
        private static IRequestHandlerResult InvokeDeleteRole(IOwinContext context)
        {
            var routeDictionary = context.GetRoleAuthRouteDictionary();

            return new ValueRequestHandlerResult(new UsersApi().DeleteRole(routeDictionary["roleName"]));
        }

        private static IRequestHandlerResult InvokeAddUser(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.UserName != null && body.Password != null)
            {
                return new ValueRequestHandlerResult(new UsersApi().AddUser(body.UserName.ToString(), body.Password.ToString()));
            }

            return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
        }

        private static IRequestHandlerResult InvokeDeleteUser(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new UsersApi().RemoveUser(routeDictionary["userName"]));
        }

        private static IRequestHandlerResult InvokeGetUser(IOwinContext context)
        {
            var routeDictionary = context.GetUserAuthRouteDictionary();

            return new ValueRequestHandlerResult(new UsersApi().GetUser(routeDictionary["userName"]));
        }


        private static IRequestHandlerResult InvokeSessionService(IOwinContext context)
        {
            return new ValueRequestHandlerResult(new SessionApi().CreateSession());
        }

        private static IRequestHandlerResult InvokeSessionServiceCommit(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            return new ValueRequestHandlerResult(new SessionApi().SaveSession(routeDictionary["sessionId"]));
        }

        private static IRequestHandlerResult InvokeSessionServiceRemove(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            return new ValueRequestHandlerResult(new SessionApi().RemoveSession(routeDictionary["sessionId"]));
        }

        private static IRequestHandlerResult InvokeAttachSessionDocumentService(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            var body = JObject.Parse(ReadRequestBody(context).ToString());

            return new ValueRequestHandlerResult(new SessionApi().Attach(routeDictionary["version"], routeDictionary["sessionId"], body));
        }

        private static IRequestHandlerResult InvokeDetachSessionDocumentService(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            return new ValueRequestHandlerResult(new SessionApi().Detach(routeDictionary["version"], routeDictionary["sessionId"], routeDictionary["attachmentId"]));
        }

        private static IRequestHandlerResult InvokeGetSessionService(IOwinContext context)
        {
            var routeDictionary = context.GetSessionRouteDictionary();

            return new ValueRequestHandlerResult(new SessionApi().GetSession(routeDictionary["version"], routeDictionary["sessionId"]));
        }

        private static IRequestHandlerResult InvokeGetDocumentService(IOwinContext context)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            var filter = nameValueCollection.Get("filter");
            IEnumerable<dynamic> criteriaList = new List<dynamic>();
            if (filter != null)
            {
                criteriaList = new FilterConverter().Convert(filter);
            }

            var sorting = nameValueCollection.Get("sorting");
            IEnumerable<dynamic> sortingList = new List<dynamic>();
            if (sorting != null)
            {
                sortingList = new SortingConverter().Convert(sorting);
            }


            var routeDictionary = context.GetRouteDictionary();

            IEnumerable<dynamic> result = new DocumentApi().GetDocument(routeDictionary["application"], routeDictionary["documentType"], criteriaList,
                Convert.ToInt32(nameValueCollection["pagenumber"]), Convert.ToInt32(nameValueCollection["pageSize"]), null, sortingList);

            return new ValueRequestHandlerResult(result);
        }

        private static IRequestHandlerResult InvokePostDocumentService(IOwinContext context)
        {
            var body = JObject.Parse(ReadRequestBody(context).ToString()).ToDynamic();

            var routeDictionary = context.GetRouteDictionary();

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(new DocumentApi().UpdateDocument(routeDictionary["application"], routeDictionary["documentType"], body));
        }

        private static IRequestHandlerResult InvokePutDocumentService(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            var routeDictionary = context.GetRouteDictionary();

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(new DocumentApi().SetDocument(routeDictionary["application"], routeDictionary["documentType"], body));
        }


        private static IRequestHandlerResult InvokePutDocumentsService(IOwinContext context)
        {
            dynamic body = JArray.Parse(ReadRequestBody(context).ToString());

            var routeDictionary = context.GetRouteDictionary();

            return new ValueRequestHandlerResult(new DocumentApi().SetDocuments(routeDictionary["application"], routeDictionary["documentType"], body));
        }

        private static IRequestHandlerResult InvokeDeleteDocumentService(IOwinContext context)
        {
            var routeDictionary = context.GetRouteDictionary();

            return new ValueRequestHandlerResult(new DocumentApi().DeleteDocument(routeDictionary["application"], routeDictionary["documentType"], routeDictionary["instanceId"]));
        }

        private static IRequestHandlerResult InvokeSignInService(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.UserName == null || body.Password == null || body.Remember == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            return new ValueRequestHandlerResult(new SignInApi().SignInInternal(body.UserName.ToString(), body.Password.ToString(), Convert.ToBoolean(body.Remember)));
        }

        private static IRequestHandlerResult InvokeSignOutService(IOwinContext context)
        {
            dynamic result = null;
            try
            {
                result = new SignInApi().SignOutInternal();
            }
            catch(Exception e)
            {
                return new ErrorRequestHandlerResult(e.Message);
            }

            return new ValueRequestHandlerResult(result);
        }

        private static IRequestHandlerResult InvokeChangePassword(IOwinContext context)
        {
            dynamic body = JObject.Parse(ReadRequestBody(context).ToString());

            if (body.UserName == null || body.OldPassword == null || body.NewPassword == null)
            {
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }

            try
            {
               return new ValueRequestHandlerResult(new SignInApi().ChangePassword(body.UserName.ToString(), body.OldPassword.ToString(),
                    body.NewPassword.ToString()));
            }
            catch (Exception e)
            {
                return new ErrorRequestHandlerResult(e.Message);
            }            
        }


        private static IRequestHandlerResult InvokeGetByIdDocumentService(IOwinContext context)
        {
            var routeDictionary = context.GetRouteDictionary();

            var result = new DocumentApi().GetDocument(routeDictionary["application"], routeDictionary["documentType"],
                cr => cr.AddCriteria(f => f.IsEquals(routeDictionary["instanceId"]).Property("Id")), 0, 1).FirstOrDefault();

            return new ValueRequestHandlerResult(result);
        }

    }
}
