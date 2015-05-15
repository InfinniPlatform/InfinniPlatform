using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class JsonCollectionItemsTokenProvider : IJsonTokenProvider
    {
        private string CreatePath(string jsonPath, string index)
        {
            return jsonPath + "." + index;
        }

        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            int index = 0;
            var result = new List<ParsedToken>();
            foreach (var value in parsedToken.JToken.Values())
            {
                result.Add(new ParsedToken(CreatePath(parsedToken.TokenName, index.ToString()), value));
                index++;
            }
            return result;
        }
    }
}