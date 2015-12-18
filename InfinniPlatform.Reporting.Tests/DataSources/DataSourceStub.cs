using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.Tests.DataSources
{
    internal sealed class DataSourceStub : IDataSource
    {
        public DataSourceStub(JArray data)
        {
            _data = data;
        }

        private readonly JArray _data;

        public Type ProviderType => typeof(DataProviderInfoStub);

        public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
        {
            return _data;
        }
    }
}