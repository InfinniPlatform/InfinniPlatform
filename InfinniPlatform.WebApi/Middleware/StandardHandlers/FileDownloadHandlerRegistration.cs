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

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class FileDownloadHandlerRegistration : HandlerRegistration
    {
        public FileDownloadHandlerRegistration() : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher,"GET")
        {
        }

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetBaseApplicationPath() + "/files/download")).Create(Priority.Higher);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

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

                return new ValueRequestHandlerResult(new UploadApi(routeDictionary["version"]).DownloadBinaryContent(
                    formData.InstanceId.ToString(), formData.FieldName.ToString()));
            }

            return new ErrorRequestHandlerResult(Resources.IncorrectDownloadRequest);
        }
    }
}
