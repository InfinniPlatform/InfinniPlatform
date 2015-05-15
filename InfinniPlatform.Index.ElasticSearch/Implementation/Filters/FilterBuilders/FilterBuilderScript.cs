using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
    /// <summary>
    ///   Builder for "StartsWith" filter applicable for strings
    /// </summary>
    public class FilterBuilderScript : IFilterBuilder
    {
        public BaseFilter Construct<T>(string script, string value) where T : class
        {
            return Filter<T>.Script(scriptFilterDescriptor => scriptFilterDescriptor.Script(string.Format("{0} == \"{1}\"", script, value)));
        }
    }
}
