using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class JsonPropertyTokenProvider : IJsonTokenProvider
    {
        private readonly string _token;

        public JsonPropertyTokenProvider(string token)
        {
            _token = token;
        }

        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            var jToken = parsedToken.JToken;
            //если текущий элемент является свойством объекта и наименование свойства совпадает с токеном для поиска,
            //полученным в конструкторе и у свойства нет дочерних объектов, тогда возвращаем контейнер, указывающий на данную JProperty
            if (ContainsPropertyName(jToken, _token) && !jToken.Children().Any())
            {
                return new[] {new ParsedToken(CreatePath(parsedToken.TokenName), jToken)};
            }

            //иначе ищем среди дочерних узлов
            var childProperty = jToken.Children().FirstOrDefault(j => ContainsPropertyName(j, _token));
            if (childProperty != null)
            {
                return new List<ParsedToken>
                {
                    new ParsedToken(CreatePath(parsedToken.TokenName), childProperty)
                };
            }

            var foundChild = jToken.Children().OfType<JObject>()
                .Select(j => j.Children().FirstOrDefault(ch => ContainsPropertyName(ch, _token)))
                .FirstOrDefault();
            return foundChild != null
                ? new List<ParsedToken>
                {
                    new ParsedToken(CreatePath(parsedToken.TokenName), foundChild)
                }
                : new List<ParsedToken>();
        }

        private bool ContainsPropertyName(JToken jtoken, string propertyName)
        {
            if (jtoken is JProperty)
            {
                var jproperty = (JProperty) jtoken;
                return jproperty.Name.ToLowerInvariant() == propertyName.ToLowerInvariant();
            }
            return false;
        }

        private string CreatePath(string jsonPath)
        {
            return string.IsNullOrEmpty(jsonPath) ? _token : jsonPath + "." + _token;
        }
    }
}