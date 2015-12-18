using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.Properties;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
    /// <summary>
    /// Настраиваемый источник данных.
    /// </summary>
    internal sealed class GenericDataSource : IDataSource
    {
        private readonly Dictionary<Type, GetDataDelegate> _dataSources = new Dictionary<Type, GetDataDelegate>();

        public Type ProviderType => null;

        public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
        {
            if (dataSourceInfo == null)
            {
                throw new ArgumentNullException(nameof(dataSourceInfo));
            }

            if (dataSourceInfo.Provider == null)
            {
                throw new ArgumentException(Resources.DataProviderInfoCannotBeNull, nameof(dataSourceInfo));
            }

            if (dataSourceInfo.Schema == null)
            {
                throw new ArgumentException(Resources.DataSchemaCannotBeNull, nameof(dataSourceInfo));
            }

            var dataProviderType = dataSourceInfo.Provider.GetType();

            GetDataDelegate getData;

            if (_dataSources.TryGetValue(dataProviderType, out getData) == false)
            {
                throw new NotSupportedException(string.Format(Resources.DataProviderIsNotSupported, dataProviderType.FullName));
            }

            return getData(dataSourceInfo, parameterInfos, parameterValues);
        }

        public void RegisterDataSource(IDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException(nameof(dataSource));
            }

            var dataProviderType = dataSource.ProviderType ?? GetType();

            _dataSources[dataProviderType] = dataSource.GetData;
        }


        private delegate JArray GetDataDelegate(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues);
    }
}