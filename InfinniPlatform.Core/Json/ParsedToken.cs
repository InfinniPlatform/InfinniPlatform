using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{

    public sealed class ParsedToken
    {
        private readonly string _tokenName;
        private readonly JToken _jToken;

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
