using System.Collections.Generic;
using InfinniPlatform.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation.ResultConverters
{
    public interface IResultConverter
    {
        void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<ParsedToken> jtokens);
    }
}
