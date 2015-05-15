using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage.ResultConverters
{
    public interface IResultConverter
    {
        void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<JToken> jtokens);
    }
}
