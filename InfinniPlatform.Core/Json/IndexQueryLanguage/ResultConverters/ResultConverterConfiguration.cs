using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage.ResultConverters
{
    public sealed class ResultConverterConfiguration
    {
        private readonly Dictionary<Func<IEnumerable<JToken>, bool>, Func<IResultConverter>> _builders = new Dictionary<Func<IEnumerable<JToken>, bool>, Func<IResultConverter>>();
        private Func<IResultConverter> _defaultProvider;

        public ResultConverterConfiguration BuilderFor(Func<IEnumerable<JToken>, bool> conditionFunc, Func<IResultConverter> resultConverterFunc)
        {
            _builders.Add(conditionFunc, resultConverterFunc);
            return this;
        }

        public ResultConverterConfiguration DefaultBuilder(Func<IResultConverter> resultConverterProvider)
        {
            _defaultProvider = resultConverterProvider;
            return this;
        }


        public IResultConverter Build(IEnumerable<JToken> tokens)
        {
            return _builders.Where(b => b.Key(tokens)).Select(b => b.Value()).FirstOrDefault() ?? _defaultProvider();
        }
    }
}
