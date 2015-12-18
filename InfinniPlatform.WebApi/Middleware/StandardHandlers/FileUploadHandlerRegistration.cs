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

namespace InfinniPlatform.WebApi.Middleware.StandardHandlers
{
    public sealed class FileUploadHandlerRegistration : HandlerRegistration
    {
        public FileUploadHandlerRegistration(UploadApi uploadApi) : base(new RouteFormatterStandard(), new RequestPathConstructor(), Priority.Higher, "POST")
        {
            _uploadApi = uploadApi;
        }

        private readonly UploadApi _uploadApi;

        protected override PathStringProvider GetPath(IOwinContext context)
        {
            return RouteFormatter.FormatRoutePath(context, new PathString(PathConstructor.GetVersionPath() + "/files/upload")).Create(Priority.Concrete);
        }

        protected override IRequestHandlerResult ExecuteHandler(IOwinContext context)
        {
            var nameValueCollection = new NameValueCollection();
            if (context.Request.QueryString.HasValue)
            {
                nameValueCollection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(context.Request.QueryString.Value));
            }

            dynamic linkedData = JObject.Parse(nameValueCollection.Get("linkedData"));

            using (var fileStream = new MultipartFormDataParser(context.Request.Body, Encoding.UTF8).Files.Select(
                f => f.Data).First())
            {
                if (linkedData.ApplicationId != null &&
                    linkedData.DocumentType != null &&
                    linkedData.InstanceId != null &&
                    linkedData.FieldName != null &&
                    linkedData.FileName != null)
                {
                    return
                        new ValueRequestHandlerResult(_uploadApi.UploadBinaryContent(linkedData.ApplicationId.ToString(), linkedData.DocumentType.ToString(), linkedData.InstanceId.ToString(),
                            linkedData.FieldName.ToString(), linkedData.FileName.ToString(), fileStream));
                }
                return new ErrorRequestHandlerResult(Resources.NotAllRequestParamsAreSpecified);
            }
        }
    }
}