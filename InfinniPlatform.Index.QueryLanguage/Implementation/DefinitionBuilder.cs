using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public sealed class DefinitionBuilder
    {
        private readonly ParsedToken _parsedToken;
        private readonly string[] _paths;

        public DefinitionBuilder(ParsedToken parsedToken)
        {
            _parsedToken = parsedToken;

            _paths = parsedToken.TokenName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<EventDefinition> BuildEvents()
        {
            var result = new List<EventDefinition>();
            for (int i = 0; i < _paths.Count(); i++)
            {
                int parseIndex;
                if (int.TryParse(_paths[i], out parseIndex))
                {
                    var eventDef = new EventDefinition()
                        {
                            Action = EventType.AddItemToCollection,
                            Index = i,
                        };
                    result.Add(eventDef);
                    //если добавляем объект, а не простое значение, создаем контейнер
                    if (i < _paths.Count() - 1)
                    {
                        eventDef = new EventDefinition();
                        eventDef.Action = EventType.CreateContainer;
                        result.Add(eventDef);
                    }
                    else
                    {
                        eventDef.Value = ((JValue) _parsedToken.JToken).Value;
                    }
                }
                //если текущий объект - коллекция 
                else if (i < _paths.Count() - 1 && int.TryParse(_paths[i], out parseIndex))
                {
                    
                }

            }
            return null;
        }
    }
}
