using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Sdk.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class ParsedTokenContainer
    {
        private readonly IEnumerable<ParsedToken> _parsedTokens;

        public ParsedTokenContainer(ParsedToken parsedToken)
        {
            _parsedTokens = new[] {parsedToken};
        }

        public ParsedTokenContainer(IEnumerable<ParsedToken> parsedTokens)
        {
            _parsedTokens = parsedTokens;
        }

        public IEnumerable<ParsedToken> ParsedTokens
        {
            get { return _parsedTokens; }
        }

        /// <summary>
        ///     Получить список событий для генерации результата выборки
        /// </summary>
        /// <param name="isLastToken">Признак последнего токена в списке</param>
        /// <returns>Список событий для генерации результата</returns>
        public IEnumerable<EventDefinition> GetResultObjectDefinitions(bool isLastToken)
        {
            var result = new List<EventDefinition>();
            var collectionIndex = 0;
            //индекс элемента в коллекции выборки (в общем случае не совпадает с индексом элемента в выбираемой коллекции)
            var projectionItemIndex = 0;
            foreach (var parsedToken in ParsedTokens)
            {
                var tokenName = parsedToken.TokenName;
                int index;
                //список всех токенов по указанному пути
                var tokenList = tokenName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                //последний токен пути в списке, содержащий значение
                var lastToken = tokenList.LastOrDefault();
                //признак того, что последний токен является итемом коллекции
                var isCollectionItem = int.TryParse(lastToken, out index);
                //токен итема коллекции
                var collectionItemToken = tokenList.Any()
                    ? string.Join(".", tokenList.Take(tokenList.Count() - 1))
                    : "";

                var jToken = parsedToken.JToken;

                var jProperty = jToken as JProperty;
                if (jProperty != null)
                {
                    if (jProperty.Value.GetType() == typeof (JArray))
                    {
                        //если токен-наименование коллекции представляет собой последний элемент пути,
                        //то заполняем список событий на основе имеющегося значения коллекции
                        if (isLastToken)
                        {
                            result.AddRange(jProperty.Value.ToEventListAsObject(tokenName).GetEvents());
                        }
                        else
                        {
                            result.Add(new EventDefinition
                            {
                                Action = EventType.CreateCollection,
                                Property = tokenName
                            });
                        }
                    }
                    else if (jProperty.Value.GetType() == typeof (JObject))
                    {
                        result.Add(new EventDefinition
                        {
                            Action = EventType.CreateContainer,
                            Property = tokenName
                        });
                        if (isLastToken)
                        {
                            result.AddRange(jProperty.Value.ToEventListAsObject(tokenName).GetEvents());
                        }
                    }
                    else if (jProperty.Value.GetType() == typeof (JValue))
                    {
                        result.Add(new EventDefinition
                        {
                            Action = EventType.CreateProperty,
                            Property = tokenName,
                            Value = jProperty.Value
                        });
                    }
                }
                if (isCollectionItem)
                {
                    if (isLastToken)
                    {
                        //событие генерируем с индексом в коллекции выборки
                        //т.к. tokenName м.б равно, например, ObjectMetadata.5, а в коллекции выборки индекс будет другим
                        var tokens = tokenList.ToList().Take(tokenList.Count() - 1).ToList();
                        var tokenCollectionName = string.Join(".", tokens);

                        result.AddRange(parsedToken.JToken.ToEventListCollectionItem(
                            tokenCollectionName,
                            projectionItemIndex).GetEvents());

                        projectionItemIndex++;
                    }
                    else
                    {
                        result.Add(new EventDefinition
                        {
                            Action = EventType.AddItemToCollection,
                            Property = collectionItemToken,
                            Index = collectionIndex++
                        });
                    }
                }
            }
            return result;
        }
    }
}