using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    /// <summary>
    ///     Провайдер, осуществляющий поиск объекта по значению его свойства
    /// </summary>
    public sealed class JsonCollectionSearchByPropertyProvider : IJsonTokenProvider
    {
        private readonly string _propertyName;
        private readonly string _propertyValue;

        public JsonCollectionSearchByPropertyProvider(string token)
        {
            var searchPropertyValue = token.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            if (searchPropertyValue.Count() < 2)
            {
                throw new ArgumentException(string.Format("fail to get property search value. No value specified: {0}",
                    token));
            }
            _propertyName = searchPropertyValue[0];
            _propertyValue = searchPropertyValue[1];
        }

        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            var result = new List<ParsedToken>();
            //ищем токен, значение свойства которого соответствует указанному значению выборки 
            JToken itemByPropertyValue = null;
            var jObject = parsedToken.JToken as JObject;
            if (jObject != null)
            {
                itemByPropertyValue =
                    jObject.Properties().FirstOrDefault(p => p.Name == _propertyName && GetValue(p) == _propertyValue);
            }
            else
            {
                //иначе ищем среди дочерних узлов
                itemByPropertyValue =
                    parsedToken.JToken.Children()
                        .FirstOrDefault(j => ContainsPropertyName(j, _propertyName) && GetValue(j) == _propertyValue);
                if (itemByPropertyValue == null)
                {
                    itemByPropertyValue = parsedToken.JToken.Children().OfType<JObject>()
                        .Select(
                            j =>
                                j.Children()
                                    .FirstOrDefault(
                                        ch => ContainsPropertyName(ch, _propertyName) && GetValue(ch) == _propertyValue))
                        .FirstOrDefault();
                }
            }


            if (itemByPropertyValue != null)
            {
                result.Add(new ParsedToken(parsedToken.TokenName, parsedToken.JToken));
            }
            return result;
        }

        private string GetValue(JToken token)
        {
            var property = token as JProperty;
            if (property == null)
            {
                return string.Empty;
            }
            var value = property.Value as JValue;
            return value != null && value.Value != null ? value.Value.ToString() : string.Empty;
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
    }
}