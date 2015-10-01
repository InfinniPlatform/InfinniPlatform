using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.UserInterface.Dynamic
{
    internal static class DynamicExtensions
    {
        /// <summary>
        ///     Преобразует объект в JSON.
        /// </summary>
        public static object ObjectToJson(this object target)
        {
            if (target != null)
            {
                return JObject.FromObject(target);
            }

            return null;
        }

        /// <summary>
        ///     Преобразует JSON в объект.
        /// </summary>
        public static object JsonToObject(this object target)
        {
            if (target != null)
            {
                return (target is DynamicWrapper) ? target : JObject.FromObject(target).ToObject<DynamicWrapper>();
            }

            return null;
        }
    }
}