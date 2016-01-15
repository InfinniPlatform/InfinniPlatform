using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    public sealed class StreamHttpResponse : HttpResponse
    {
        public StreamHttpResponse(byte[] content, string contentType = HttpConstants.JsonContentType) : this(new MemoryStream(content), contentType)
        {
        }

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