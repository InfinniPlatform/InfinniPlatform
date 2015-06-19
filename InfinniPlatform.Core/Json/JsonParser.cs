using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class JsonParser
    {
        private readonly JsonTokenConfiguration _jsonTokenConfiguration = new JsonTokenConfiguration();

        public JsonParser()
        {
            int collectionItemToken;
            _jsonTokenConfiguration.BuilderFor(token => token == "$", token => new JsonCollectionItemsTokenProvider());
            _jsonTokenConfiguration.BuilderFor(token => token == "@", token => new JsonCollectionLastItemTokenProvider());
            _jsonTokenConfiguration.BuilderFor(token => token.Contains(":"),
                token => new JsonCollectionSearchByPropertyProvider(token));
            _jsonTokenConfiguration.BuilderFor(token => int.TryParse(token, out collectionItemToken),
                token => new JsonCollectionItemTokenProvider(token));
            _jsonTokenConfiguration.DefaultBuilder(token => new JsonPropertyTokenProvider(token));
        }

        private IEnumerable<IJsonTokenProvider> GetJsonTokenProviderSequence(string path)
        {
            var result = new List<IJsonTokenProvider>();
            //в качестве наименования свойства анализируем только то, что встречается до двоеточия, остальное анализируем как значение
            //Example: Assemblies.$.Name:Infinni.Integration
            var pathToSplit = path.Split(':');
            var tokenList = new List<string>();
            if (pathToSplit.Any())
            {
                //если путь содержит уточняющее inline значение 
                if (pathToSplit.Count() > 1) //path[0] : "Assemblies.$.Name", path[1] : "Infinni.Integration"
                {
                    //path[0] :  [ {Assemblies},{$}, {Name}], path[1]
                    var basePath = pathToSplit.First().Split('.');

                    //добавляем [ {Assemblies},{$}]
                    tokenList.AddRange(basePath.Take(basePath.Count() - 1));
                    //получаем последнее значение токена пути: {Name} и соединяем с Infinni.Integration
                    tokenList.Add(basePath.Last() + ":" + pathToSplit.Skip(1).First());
                }
                else
                {
                    tokenList.AddRange(pathToSplit.First().Split('.'));
                }
            }

            foreach (var token in tokenList)
            {
                var tokenProvider = _jsonTokenConfiguration.Build(token);
                if (tokenProvider == null)
                {
                    throw new ArgumentException("no registered providers for json token");
                }
                result.Add(tokenProvider);
            }
            return result;
        }

        public IEnumerable<JToken> FindJsonToken(JToken startToken, string path)
        {
            var containers = FindJsonPath(startToken, path).ToList();
            var result = new List<JToken>();
            if (containers.Any())
            {
                result.AddRange(containers.Last().ParsedTokens.Select(p => p.JToken).ToList());
            }
            return result;
        }

        /// <summary>
        ///     Выполняем парсинг строки пути, преобразуя ее в соответствующую JSON-структуру
        ///     Двигаемся по цепочке, проходя по каждому токену, список которых предоставляется в результате
        ///     метода GetJsonTokenSequence. Каждый токен представляет собой либо свойство объекта, либо
        ///     индекс элемента коллекции
        /// </summary>
        public IEnumerable<ParsedTokenContainer> FindJsonPath(JToken startToken, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new List<ParsedTokenContainer>
                {
                    new ParsedTokenContainer(new ParsedToken(path, startToken))
                };
            }

            var tokens = GetJsonTokenProviderSequence(path);


            var tokenList = new List<ParsedToken>();

            if (startToken != null)
            {
                tokenList.Add(new ParsedToken("", startToken));
            }

            var containers = new List<ParsedTokenContainer>();
            foreach (var jsonTokenProvider in tokens)
            {
                GetResultJsonTokenList(tokenList.Select(tl => new ParsedToken(tl.TokenName, tl.JToken)).ToList(),
                    tokenList, jsonTokenProvider);

                if (!tokenList.Any())
                {
                    return new List<ParsedTokenContainer>();
                }
                containers.Add(new ParsedTokenContainer(tokenList.ToList()));
            }


            return containers;
        }

        private static void GetResultJsonTokenList(IEnumerable<ParsedToken> startToken, List<ParsedToken> jTokenResult,
            IJsonTokenProvider jsonTokenProvider)
        {
            jTokenResult.Clear();
            foreach (var jToken in startToken)
            {
                var jsonFound = jsonTokenProvider.GetJsonToken(jToken);
                if (jsonFound != null)
                {
                    jTokenResult.AddRange(jsonFound.Where(j => j.JToken != null));
                }
            }
        }
    }

    public static class JsonParserExtensions
    {
        private static object GetPropertyValue(this IEnumerable<JToken> tokens)
        {
            if (tokens != null && tokens.Any() && tokens.First() is JProperty)
            {
                var result = ((JProperty) tokens.First()).Value;
                if (result != null)
                {
                    return result;
                }
                return null;
            }
            return null;
        }

        public static string GetPropertyValueToString(this IEnumerable<JToken> tokens)
        {
            var objectValue = GetPropertyValue(tokens);
            if (objectValue != null)
            {
                return objectValue.ToString();
            }
            return string.Empty;
        }

        public static object GetPropertyValueObject(this IEnumerable<JToken> tokens)
        {
            var value = GetPropertyValue(tokens);
            return value != null ? ((JValue) value).Value : null;
        }

        public static object GetPropertyValueObject(this JToken token)
        {
            return GetPropertyValueObject(new[] {token});
        }
    }
}