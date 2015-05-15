using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
    public sealed class HttpResultHandlerHtml : IHttpResultHandler
    {
        public HttpResponseMessage WrapResult(object result)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(result.ToString(),Encoding.UTF8);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            response.Content.Headers.ContentEncoding.Add("UTF-8");
            return response;

        }
    }
}
