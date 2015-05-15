using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation.ResultConverters.ConverterImplementations
{
    public class ResultConverterArray : IResultConverter
    {
        public void BuildResultJToken(string projectName, JObject projectionResult, IEnumerable<ParsedToken> jtokens)
        {           
            //выбираем все существующие пути
            var existingPaths = jtokens.Select(j => j.TokenName).ToList();


            var result = new JArray();
            foreach (var jtoken in jtokens)
            {
                //Если контейнер - добавляем в проекцию as is
                if (jtoken is JObject)
                {
                    result.Add(jtoken);    
                }
                //если простой тип - берем только значение свойства
                else
                {
                    var property = jtoken as JProperty;
                    if (property != null)
                    {
                        result.Add(property.Value);
                    }
                    else
                    {
                        throw new ArgumentException("unknown json type");
                    }
                }
            }
            projectionResult.Add(projectName, result);
        }
    }
}