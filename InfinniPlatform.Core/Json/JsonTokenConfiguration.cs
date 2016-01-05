using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Core.Json
{
    public sealed class JsonTokenConfiguration
    {
        private Func<string, IJsonTokenProvider> _defaultProvider;

        private readonly Dictionary<Func<string, bool>, Func<string, IJsonTokenProvider>> _builders =
            new Dictionary<Func<string, bool>, Func<string, IJsonTokenProvider>>();

        public JsonTokenConfiguration BuilderFor(Func<string, bool> conditionFunc,
            Func<string, IJsonTokenProvider> jsonTokenProvider)
        {
            _builders.Add(conditionFunc, jsonTokenProvider);
            return this;
        }

        public JsonTokenConfiguration DefaultBuilder(Func<string, IJsonTokenProvider> jsonTokenProvider)
        {
            _defaultProvider = jsonTokenProvider;
            return this;
        }

        public IJsonTokenProvider Build(string code)
        {
            IJsonTokenProvider provider = null;
            if (!string.IsNullOrEmpty(code))
            {
                provider = _builders.Where(b => b.Key(code)).Select(b => b.Value(code)).FirstOrDefault();
            }
            return provider ?? _defaultProvider(code);
        }
    }
}