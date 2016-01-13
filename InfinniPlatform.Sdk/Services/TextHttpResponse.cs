using System.Text;

namespace InfinniPlatform.Sdk.Services
{
    public sealed class TextHttpResponse : HttpResponse
    {
        public TextHttpResponse(string content, string contentType = HttpConstants.TextContentType, Encoding encoding = null)
        {
            ContentType = contentType;

            if (!string.IsNullOrEmpty(content))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                var bytes = encoding.GetBytes(content);

                Content = stream => stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}