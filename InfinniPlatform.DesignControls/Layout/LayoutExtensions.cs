using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.DesignControls.Layout
{
    public static class LayoutExtensions
    {
        public static void RemoveEmptyEntries(dynamic layout)
        {
            JObject instance = JObject.FromObject(layout);
            foreach (var property in instance.Properties().ToList())
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

                    //не удаляем пустые массивы
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
            if (!instance.Properties().Any() && instance.Parent != null)
            {
                instance.Remove();
            }

        }

        private static bool NeedRemoveEntry(JToken entry)
        {
            if (entry.GetType() == typeof(JProperty))
            {
                return NeedRemoveEntry(((JProperty)entry).Value);
            }
            if (entry.GetType() == typeof(JObject))
            {
                var jobject = ((JObject)entry);
                if (!jobject.Properties().Any())
                {
                    return true;
                }

                var result = true;

                foreach (var property in jobject.Properties().ToList())
                {
                    result = result && NeedRemoveEntry(property);
                }
                return result;

            }
            if (entry.GetType() == typeof(JValue))
            {
                return ((JValue)entry).Value == null;
            }
            return false;
        }

    }
}
