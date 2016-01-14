using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Services
{
    public sealed class JsonHttpResponse : HttpResponse
    {
        public JsonHttpResponse(object content)
        {
            ContentType = HttpConstants.JsonContentType;

            if (content != null)
            {
                Content = stream => JsonObjectSerializer.Default.Serialize(stream, content);
            }
        }
    }
}