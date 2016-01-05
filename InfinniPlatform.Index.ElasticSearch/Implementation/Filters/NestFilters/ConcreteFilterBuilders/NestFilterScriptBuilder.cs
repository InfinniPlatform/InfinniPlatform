using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed  class NestFilterScriptBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string script, object value)
        {
            var scriptText = value != null
                                 ? string.Format("{0} == \"{1}\"", script, value)
                                 : script;
            return new NestFilter(Nest.Filter<dynamic>.Script(scriptDescriptor => scriptDescriptor.Script(scriptText)));
        }
    }
}
