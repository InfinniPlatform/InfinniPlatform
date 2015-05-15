using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation.ResultConverters.ConverterImplementations
{
    public class ResultConverterDefault : IResultConverter
    {
        public void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<JToken> jtokens)
        {
            jtokens = jtokens.ToArray();
            if (jtokens.FirstOrDefault() != null && jtokens.FirstOrDefault() is JProperty)
            {
                projectionResult.Add(projectName, ((JProperty) jtokens.FirstOrDefault()).Value);
            }
            else
            {
                if (jtokens.FirstOrDefault() != null && jtokens.FirstOrDefault() is JObject)
                {
                    projectionResult.Add(projectName, jtokens.FirstOrDefault());
                }
                else
                {
                    throw new ArgumentException("unknown json token type");
                }
            }
            
        }
    }
}