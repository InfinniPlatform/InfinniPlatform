using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage.ResultConverters.ConverterImplementations
{
    public class ResultConverterEmpty : IResultConverter
    {
        public void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<JToken> jtokens)
        {
            projectionResult.Add(projectName,new JObject());
        }
    }
}
