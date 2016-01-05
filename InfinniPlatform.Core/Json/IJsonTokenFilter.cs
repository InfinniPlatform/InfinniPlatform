using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json
{
    public interface IJsonTokenFilter
    {
        void FilterJsonToken(JToken jtoken);
    }
}