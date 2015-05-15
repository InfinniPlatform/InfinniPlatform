using System.Collections.Generic;
using InfinniPlatform.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation.ResultConverters.ConverterImplementations
{
    public class ResultConverterEmpty : IResultConverter
    {
        public void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<ParsedToken> jtokens)
        {
            projectionResult.Add(projectName,new JArray());
        }
    }
}
