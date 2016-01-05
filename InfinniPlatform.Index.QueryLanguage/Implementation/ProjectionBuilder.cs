using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index.SearchOptions;
using InfinniPlatform.Core.Json;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public sealed class ProjectionBuilder
    {
        private readonly JsonParser _jsonEventParser = new JsonParser();

        public JObject GetProjection(JObject jsonObject, IEnumerable<ProjectionObject> projectionItems,
            IEnumerable<WhereObject> where, IEnumerable<ReferencedObject> joins)
        {
            var resultProjection = new JObject();


            var jResult = new JObject();


            jsonObject.FilterItems(GetNotReferenceFields(joins.Select(j => j.Alias), where));


            foreach (var projectionItem in projectionItems)
            {
                var result = _jsonEventParser.FindJsonPath(jsonObject, projectionItem.ProjectionPath).ToList();


                var eventDefinitions =
                    result.SelectMany(c => c.GetResultObjectDefinitions(c.Equals(result.Last()))).ToList();


                jResult = JObject.FromObject(new ObjectRenderingHandler().RenderEvents(jResult, eventDefinitions));


                jResult.RemoveEmptyEntries();
            }


            //фильтруем итоговый полученный агрегат, так как на этом шаге заполнены все проекции
            jResult.FilterItems(where);

            //удаляем пустые коллекции и элементы
            jResult.RemoveEmptyEntries();

            resultProjection.Add("Result", projectionItems.Any() ? jResult : jsonObject);


            return resultProjection;
        }

        private IEnumerable<WhereObject> GetNotReferenceFields(IEnumerable<string> aliases,
            IEnumerable<WhereObject> @where)
        {
            var notReferencedFields =
                QuerySyntaxTree.GetNotAliasedFields(aliases, where.Select(w => w.RawProperty)).ToList();
            return @where.Where(wh => notReferencedFields.Contains(wh.RawProperty));
        }
    }

    public static class ProjectionBuilderExtensions
    {
        public static void FilterItems(this IEnumerable<JToken> jtokens, IEnumerable<WhereObject> criteria)
        {
            var queryInterpreter = new QueryCriteriaInterpreter();
            foreach (var filteredToken in jtokens)
            {
                var applicableToken = filteredToken;

                foreach (var criterion in criteria)
                {
                    var filterCriteria = new JsonFilterCriteria(criterion, applicableToken);

                    var clientFilterOperator = queryInterpreter.BuildOperator(criterion.ToCriteria());

                    filterCriteria.RemoveUnsatisfiedTokens(clientFilterOperator);

                    applicableToken = filterCriteria.JsonToken;
                }
            }
        }

        public static void RemoveEmptyEntries(this JObject jObject)
        {
            foreach (var property in jObject.Properties().ToList())
            {
                if (NeedRemoveEntry(property))
                {
                    property.Remove();
                }

                var array = property.Value as JArray;
                if (array != null)
                {
                    var jarray = array;
                    if (!jarray.Any() && property.Parent != null)
                    {
                        property.Remove();
                    }

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
            if (entry.GetType() == typeof (JArray))
            {
                return false;
                //var jarray = (JArray)entry;
                //var result = true;
                //if (!jarray.Any())
                //{
                //	return true;
                //}

                //foreach (var jtoken in jarray)
                //{
                //	result = result && NeedRemoveEntry(jtoken);
                //}
                //return result;
            }
            if (entry.GetType() == typeof (JObject))
            {
                var jobject = ((JObject) entry);
                if (!jobject.Properties().Any() &&
                    (jobject.Parent == null || jobject.Parent.GetType() == typeof (JArray)))
                {
                    return true;
                }

                return false;
                //var result = true;

                //foreach (var property in jobject.Properties().ToList())
                //{
                //	result = result && NeedRemoveEntry(property);	
                //}
                //return result;
            }
            if (entry.GetType() == typeof (JValue))
            {
                return false; //((JValue) entry).Value == null;
            }
            return false;
        }

        public static void FilterItems(this JToken item, IEnumerable<WhereObject> criteria)
        {
            new[] {item}.FilterItems(criteria);
        }
    }
}