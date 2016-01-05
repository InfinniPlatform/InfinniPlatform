using System.Collections.Generic;

namespace InfinniPlatform.Core.Json
{
    public interface IJsonTokenProvider
    {
        IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken);
    }
}