using System;
using System.Collections;
using System.Linq;
using InfinniPlatform.Api.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Serialization
{
    /// <summary>
    ///     Осуществляет преобразование объекта в JSON-представление и обратно на основе списка известных типов.
    /// </summary>
    internal sealed class KnownTypesJsonConverter : JsonConverter
    {
        private readonly Type _enumerableType;
        private readonly KnownTypesContainer _knownTypes;

        public KnownTypesJsonConverter(KnownTypesContainer knownTypes)
        {
            _enumerableType = typeof (IEnumerable);
            _knownTypes = knownTypes ?? new KnownTypesContainer();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _knownTypes.HasType(objectType) ||
                   ((objectType.IsInterface || objectType.IsAbstract) && !_enumerableType.IsAssignableFrom(objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObj = JToken.FromObject(value);

            if (jObj.Type == JTokenType.Object)
            {
                var objType = value.GetType();
                var objTypeName = _knownTypes.GetName(objType);

                var jObjProperty = new JProperty(objTypeName, jObj);
                var jObjWrapper = new JObject {jObjProperty};

                WrapObjectProperties((JObject) jObj, value, serializer);

                jObj = jObjWrapper;
            }

            jObj.WriteTo(writer);
        }

        private static void WrapObjectProperties(JObject jObj, object value, JsonSerializer serializer)
        {
            var objType = value.GetType();

            foreach (var jProperty in jObj.Properties())
            {
                var objProperty = objType.GetProperty(jProperty.Name);

                if (objProperty != null)
                {
                    var objPropertyValue = objProperty.GetValue(value);

                    if (objPropertyValue != null)
                    {
                        jProperty.Value = JToken.FromObject(objPropertyValue, serializer);
                    }
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObjWrapper = JObject.Load(reader);
            var jObjProperty = jObjWrapper.Properties().FirstOrDefault();

            if (jObjProperty != null)
            {
                var objTypeName = jObjProperty.Name;
                var objType = _knownTypes.GetType(objTypeName);

                if (objType != null)
                {
                    var obj = Activator.CreateInstance(objType, true);
                    serializer.Populate(jObjProperty.Value.CreateReader(), obj);

                    return obj;
                }
            }

            throw new InvalidOperationException(string.Format(Resources.CannotDeserializeTypeError, objectType.FullName));
        }
    }
}