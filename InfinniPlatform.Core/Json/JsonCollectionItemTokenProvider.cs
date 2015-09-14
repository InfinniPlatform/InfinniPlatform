using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
    public class JsonCollectionItemTokenProvider : IJsonTokenProvider
    {
        private readonly string _token;

        public JsonCollectionItemTokenProvider(string token)
        {
            _token = token;
        }

        public string Token
        {
            get { return _token; }
        }

        public IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return null;
            }

            var parseToken = -1;
            int.TryParse(Token, out parseToken);
            if (parseToken == -1)
            {
                throw new ArgumentException("fail to parse collection item");
            }

            if (parseToken > 0)
            {
                return new List<ParsedToken>
                {
                    new ParsedToken(CreatePath(parsedToken.TokenName),
                        parsedToken.JToken.Values().Skip(parseToken).FirstOrDefault())
                };
            }
            return new List<ParsedToken>
            {
                new ParsedToken(CreatePath(parsedToken.TokenName), parsedToken.JToken.Values().FirstOrDefault())
            };
        }

        private string CreatePath(string jsonPath)
        {
            return jsonPath + "." + Token;
        }
    }
}