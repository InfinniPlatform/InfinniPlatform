using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.Sdk.Application.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class BackboneBuilder
    {
        private readonly IDictionary<EventType, IJsonObjectBuilder> _builders =
            new Dictionary<EventType, IJsonObjectBuilder>();

        public void RegisterBuilder(EventType eventDefinition, IJsonObjectBuilder jObjectBuilder)
        {
            _builders.Add(eventDefinition, jObjectBuilder);
        }

        public object ConstructJsonObject(object rootObject, IEnumerable<EventDefinition> events)
        {
            var root = JObject.FromObject(rootObject);

            foreach (var @event in events)
            {
                var eventDefinition = @event.Action;

                if (_builders.ContainsKey(eventDefinition))
                {
                    var builder = _builders[eventDefinition];
                    builder.BuildJObject(root, @event);
                }
            }

            //RemoveEmptyEntries(root);
            return root.ToDynamic();
        }

        private static void RemoveEmptyEntries(JObject jObject)
        {
            foreach (var property in jObject.Properties().ToList())
            {
                if (NeedRemoveEntry(property))
                {
                    property.Remove();
                    continue;
                }
                var array = property.Value as JArray;
                if (array != null)
                {
                    var jarray = array;

                    //не удаляем пустые массивы при создании объекта из событий
                    //if (!jarray.Any() && property.Parent != null)
                    //{
                    //	property.Remove();
                    //}

                    foreach (var jtoken in jarray.ToList())
                    {
                        var o = jtoken as JObject;
                        if (o != null)
                        {
                            RemoveEmptyEntries(o);
                        }

                        else if (NeedRemoveEntry(jtoken))
                        {
                            jtoken.Remove();
                        }
                    }
                }
                var jobject = property.Value as JObject;
                if (jobject != null)
                {
                    RemoveEmptyEntries(property.Value as JObject);
                }
            }
            if (!jObject.Properties().Any() && (jObject.Parent != null && jObject.Parent.GetType() == typeof (JArray)))
            {
                jObject.Remove();
            }
        }

        private static bool NeedRemoveEntry(JToken entry)
        {
            if (entry.GetType() == typeof (JProperty))
            {
                return NeedRemoveEntry(((JProperty) entry).Value);
            }
            if (entry.GetType() == typeof (JObject))
            {
                var jobject = ((JObject) entry);
                if (!jobject.Properties().Any() &&
                    (jobject.Parent != null && jobject.Parent.GetType() == typeof (JArray)))
                {
                    return true;
                }

                return false;
            }
            if (entry.GetType() == typeof (JValue))
            {
                return false;
            }
            return false;
        }
    }
}