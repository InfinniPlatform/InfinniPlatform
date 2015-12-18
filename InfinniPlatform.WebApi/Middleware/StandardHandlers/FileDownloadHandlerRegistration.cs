using System;
using System.Collections.Specialized;
using System.Web;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class FileDownloadHandlerRegistration : HandlerRegistration
    {
        public FileDownloadHandlerRegistration(UploadApi uploadApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "GET")
        {
            _uploadApi = uploadApi;
        }

        private readonly UploadApi _uploadApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/files/download")).Create(Priority.Concrete);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var routeDictionary = RouteFormatter.GetRouteDictionary(context);

            var nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            if (nameValueCollection.Get("Form") != null)
            {
                dynamic formData = JObject.Parse(HttpUtility.UrlDecode(nameValueCollection.Get("Form")));

                if (formData.ContentId == null)
                {
                    throw new ArgumentException(Resources.NotAllRequestParamsAreSpecified);
                }

                return new ValueRequestHandlerResult(_uploadApi.DownloadBinaryContent(formData.ContentId.ToString()));
            }

            return new ErrorRequestHandlerResult(Resources.IncorrectDownloadRequest);
        }
    }
}