using System.Collections.Generic;

namespace InfinniPlatform.Json
{
    public interface IJsonTokenProvider
    {
        IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken);
    }
}