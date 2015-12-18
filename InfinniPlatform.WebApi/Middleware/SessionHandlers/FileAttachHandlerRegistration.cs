using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.WebApi.Middleware.MultipartFormData;
using InfinniPlatform.WebApi.Middleware.RouteFormatters;

using Microsoft.Owin;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.WebApi.Middleware.SessionHandlers
{
    public sealed class FileAttachHandlerRegistration : HandlerRegistration
    {
        public FileAttachHandlerRegistration(SessionApi sessionApi) : base(new RouteFormatterSession(), new RequestPathConstructor(), Priority.Standard, "POST")
        {
            _sessionApi = sessionApi;
        }

        private readonly SessionApi _sessionApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, PathConstructor.GetVersionPath()).Create(Priority);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            dynamic linkedData = JObject.Parse(nameValueCollection.Get("linkedData"));

            using (var fileStream = new MultipartFormDataParser(context.Request.Body, Encoding.UTF8).Files.Select(f => f.Data).First())
            {
                if (linkedData.InstanceId != null && linkedData.FieldName != null && linkedData.SessionId != null)
                {
                    return new ValueRequestHandlerResult(_sessionApi.AttachFile(linkedData, fileStream));
                }
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreFiled);
            }
        }
    }
}