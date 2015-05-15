using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
			RegisterPostRequestHandler(GetRestTemplatePath, InvokeCustomService);
			RegisterGetRequestHandler(GetRestTemplateDocumentPath, InvokeGetDocumentService);
			RegisterPostRequestHandler(GetRestTemplateDocumentPath, InvokePostDocumentService);
			RegisterPutRequestHandler(GetRestTemplateDocumentPath, InvokePutDocumentService);
			RegisterDeleteRequestHandler(GetRestTemplateDocumentPath, InvokeDeleteDocumentService);
		}

		private PathString GetBaseApplicationPath()
		{
			return new PathString("/_version_/_application_");
		}

		private PathString GetRestTemplatePath(IOwinContext context)
		{
			return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_/_service_"));
		}

		private PathString GetRestTemplateDocumentPath(IOwinContext context)
		{
			return context.FormatRoutePath(new PathString(GetBaseApplicationPath() + "/_documentType_"));
		}


		private static IRequestHandlerResult InvokeCustomService(IOwinContext context)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		private static IRequestHandlerResult InvokePutDocumentService(IOwinContext context)
		{		   
            var body = JObject.Parse(ReadRequestBody(context).ToString());

            var routeDictionary = context.GetRouteDictionary();

		    return new ValueRequestHandlerResult(new DocumentApi().SetDocument(routeDictionary["application"], routeDictionary["documentType"], body));
		}

		private static IRequestHandlerResult InvokeDeleteDocumentService(IOwinContext context)
		{
			throw new NotImplementedException();
		}

	}
}
