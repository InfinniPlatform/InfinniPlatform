using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel.Configuration;
using System.Text;
using System.Web;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
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
            RegisterPostRequestHandler(GetRestTemplateDocument, InvokePostDocumentService);
            RegisterPutRequestHandler(GetRestTemplateDocument, InvokePutDocumentService);
            RegisterDeleteRequestHandler(GetRestTemplateDocumentDelete, InvokeDeleteDocumentService);

            RegisterPutRequestHandler(GetRestTemplateDocuments, InvokePutDocumentsService);

            RegisterGetRequestHandler(GetRestTemplateDocumentById, InvokeGetByIdDocumentService);

            RegisterPostRequestHandler(GetRestTemplate, InvokeCustomService);
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
        }

        private PathString GetBaseApplicationPath()
        {
            return new PathString("/_version_/_application_");
        }

        private PathStringProvider GetSessionTemplate(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString("/_version_")).Create(Priority.Standard);
        }


        private PathStringProvider GetSessionTemplateById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString("/_version_/_sessionId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetSessionTemplateAttachmentById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString("/_version_/_sessionId_/_attachmentId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplate(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_service_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocumentList(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocument(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_")).Create(Priority.Standard);
        }

        private PathStringProvider GetRestTemplateDocuments(IOwinContext context)
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
                throw new ArgumentException(Resources.NotAllRequestParamsAreSpecified);
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

            throw new ArgumentException(Resources.IncorrectDownloadRequest);
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
                throw new ArgumentException(Resources.NotAllRequestParamsAreSpecified);
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
            throw new ArgumentException(Resources.NotAllRequestParamsAreSpecified);

        }
        private static IRequestHandlerResult InvokeCustomService(IOwinContext context)
        {
            throw new NotImplementedException();
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


        private static IRequestHandlerResult InvokeGetByIdDocumentService(IOwinContext context)
        {
            var routeDictionary = context.GetRouteDictionary();

            var result = new DocumentApi().GetDocument(routeDictionary["application"], routeDictionary["documentType"],
                cr => cr.AddCriteria(f => f.IsEquals(routeDictionary["instanceId"]).Property("Id")), 0, 1).FirstOrDefault();

            return new ValueRequestHandlerResult(result);
        }

    }
}
