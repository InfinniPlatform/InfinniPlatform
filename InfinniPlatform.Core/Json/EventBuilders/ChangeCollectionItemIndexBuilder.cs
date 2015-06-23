using System;
using System.Linq;
using InfinniPlatform.Sdk.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.EventBuilders
{
    public sealed class ChangeCollectionItemIndexBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
            var indexes =
                ((string) eventDefinition.Value).Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => Convert.ToInt32(r))
                    .ToArray();

            var tokenList = new JsonParser().FindJsonToken(backboneObject, eventDefinition.Property);

            if (tokenList == null)
            {
                throw new ArgumentException("object to apply event is not defined");
            }

            var jArray = tokenList.First().First as JArray;
            if (jArray == null)
            {
                throw new ArgumentException("object to add item is not a collection");
            }

            var tokenToMove = jArray[indexes[0]];
            jArray.RemoveAt(indexes[0]);
            if (indexes[1] > jArray.Count - 1)
            {
                jArray.Add(tokenToMove);
            }
            else
            {
                jArray.Insert(indexes[1], tokenToMove);
            }
        }
    }
}