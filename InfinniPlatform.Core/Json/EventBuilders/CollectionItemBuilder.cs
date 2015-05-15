using System;
using System.Linq;
using InfinniPlatform.Api.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.EventBuilders
{
    public class CollectionItemBuilder : IJsonObjectBuilder
    {
        public void BuildJObject(JToken backboneObject, EventDefinition eventDefinition)
        {
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

	        var tokenToAdd = eventDefinition.Value != null ? JToken.FromObject(eventDefinition.Value) : null;

			UpdateProperty(jArray,tokenToAdd  ?? new JObject(), eventDefinition.Index);
        }

        private void UpdateProperty(JArray parentArray, JToken objectToAdd, int index )
        {
			//Если указанный индекс больше количества элементов в массиве, просто добавляем элемент
            if (index > parentArray.Count - 1)
            {
				parentArray.Add(objectToAdd);
				return;
            }

			//если объект в указанном индексе уже сушествует, то ничего не делаем
			if (index != -1 && parentArray[index] != null)
			{
				return;
			}
			
            parentArray.Add(objectToAdd);
            
        }


    }
}