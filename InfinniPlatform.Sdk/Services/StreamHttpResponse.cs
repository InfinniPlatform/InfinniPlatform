using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    public sealed class StreamHttpResponse : HttpResponse
    {
        public StreamHttpResponse(Stream content, string contentType = HttpConstants.JsonContentType)
        {
            ContentType = contentType;

            if (content != null)
            {
                Content = content.CopyTo;
            }
        }
    }
}