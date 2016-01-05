using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Json
{
    public sealed class ParsedToken
    {
        private readonly JToken _jToken;
        private readonly string _tokenName;

        public ParsedToken(string tokenName, JToken jToken)
        {
            _tokenName = tokenName;
            _jToken = jToken;
        }

        public string TokenName
        {
            get { return _tokenName; }
        }

        public JToken JToken
        {
            get { return _jToken; }
        }
    }
}