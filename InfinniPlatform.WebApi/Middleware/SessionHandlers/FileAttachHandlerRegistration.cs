using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.MultipartFormData;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class FileAttachHandlerRegistration : HandlerRegistration
    {
        public FileAttachHandlerRegistration() : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "POST")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetVersionPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
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
                    return new ValueRequestHandlerResult(new SessionApi().AttachFile(linkedData, fileStream));
                }
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }
        }
    }
}
