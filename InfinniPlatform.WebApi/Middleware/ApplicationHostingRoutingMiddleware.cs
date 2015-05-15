using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Owin.Middleware;
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
		public ApplicationHostingRoutingMiddleware(OwinMiddleware next) : base(next)
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

            RegisterPostRequestHandler(GetSessionTemplate, InvokeSessionService);
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

        private PathString GetSessionTemplate(IOwinContext context)
	    {
	        return context.FormatSessionRoutePath(new PathString("/_version_"));
	    }


        private PathString GetSessionTemplateById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString("/_version_/_sessionId_"));
        }

        private PathString GetSessionTemplateAttachmentById(IOwinContext context)
        {
            return context.FormatSessionRoutePath(new PathString("/_version_/_sessionId_/_attachmentId_"));
        }

		private PathString GetRestTemplate(IOwinContext context)
		{
			return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_service_"));
		}

        private PathString GetRestTemplateDocumentList(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_"));
        }

		private PathString GetRestTemplateDocument(IOwinContext context)
		{
			return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_"));
		}

        private PathString GetRestTemplateDocuments(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_"));
        }


        private PathString GetRestTemplateDocumentDelete(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_"));
        }

        private PathString GetRestTemplateDocumentById(IOwinContext context)
        {
            return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_instanceId_"));
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
		        Convert.ToInt32(nameValueCollection["pagenumber"]), Convert.ToInt32(nameValueCollection["pageSize"]),null, sortingList);

            return new ValueRequestHandlerResult(result);
		}

		private static IRequestHandlerResult InvokePostDocumentService(IOwinContext context)
		{
		    var body = JObject.Parse(ReadRequestBody(context).ToString()).ToDynamic();

		    var routeDictionary = context.GetRouteDictionary();

            body.Id = routeDictionary["instanceId"];

            return new ValueRequestHandlerResult(new DocumentApi().UpdateDocument(routeDictionary["application"],routeDictionary["documentType"],body));
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
                cr => cr.AddCriteria(f => f.IsEquals(routeDictionary["instanceId"]).Property("Id")),0, 1).FirstOrDefault();

            return new ValueRequestHandlerResult(result);
        }

	}
}
