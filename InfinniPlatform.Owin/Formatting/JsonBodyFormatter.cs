using System.Threading.Tasks;

using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Formatting
{
    /// <summary>
    /// Предоставляет методы чтения и записи тела запроса и ответа, представленных в формате "application/json".
    /// </summary>
    public sealed class JsonBodyFormatter : IBodyFormatter
    {
        public static readonly JsonBodyFormatter Instance = new JsonBodyFormatter();

        public string ContentType
        {
            get { return "application/json"; }
        }

        public object ReadBody(IOwinRequest request)
        {
            return JsonObjectSerializer.Default.Deserialize(request.Body, typeof(DynamicWrapper));
        }

        public async Task WriteBody(IOwinResponse response, object value)
        {
            if (value != null)
            {
                var data = new JsonObjectSerializer(true, null).Serialize(value);
                response.ContentLength = data.LongLength;
                response.ContentType = ContentType;
                await response.WriteAsync(data);
            }
        }
    }
}