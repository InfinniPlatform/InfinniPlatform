using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json
{
    public class JsonCollectionLastItemTokenProvider : IJsonTokenProvider
    {
        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            var result = new List<ParsedToken>();
            var lastElement = parsedToken.JToken.Values().LastOrDefault();
            if (lastElement != null)
            {
                var indexOfLastElement = parsedToken.JToken.Values().ToList().IndexOf(lastElement).ToString();
                result.Add(new ParsedToken(CreatePath(parsedToken.TokenName, indexOfLastElement), lastElement));
                return result;
            }
            throw new ArgumentException(string.Format("collection contains no element: {0}", parsedToken.TokenName));
        }

        private string CreatePath(string jsonPath, string index)
        {
            return jsonPath + "." + index;
        }
    }
}