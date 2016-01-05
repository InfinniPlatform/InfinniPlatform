using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json
{
    public class JsonCollectionItemsTokenProvider : IJsonTokenProvider
    {
        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            var index = 0;
            var result = new List<ParsedToken>();
            foreach (var value in parsedToken.JToken.Values())
            {
                result.Add(new ParsedToken(CreatePath(parsedToken.TokenName, index.ToString()), value));
                index++;
            }
            return result;
        }

        private string CreatePath(string jsonPath, string index)
        {
            return jsonPath + "." + index;
        }
    }
}