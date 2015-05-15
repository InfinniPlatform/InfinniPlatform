using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.Tests.DataSources
{
	sealed class DataSourceStub : IDataSource
	{
		private readonly JArray _data;

		public DataSourceStub(JArray data)
		{
			_data = data;
		}

		public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
		{
			return _data;
		}
	}
}