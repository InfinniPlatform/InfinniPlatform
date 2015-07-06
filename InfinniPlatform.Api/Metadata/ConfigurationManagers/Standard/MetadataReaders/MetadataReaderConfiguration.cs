using System;
using System.Collections.Generic;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public sealed class MetadataReaderConfiguration : IDataReader
    {
        private readonly string _version;
        private readonly bool _doNotCheckVersion;

        /// <summary>
        ///  Конструктор
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="doNotCheckVersion">Флаг учета версии конфигурации при выборке. 
        ///     Учитывается в данный момент только вследствие дизайнера, в котором пока не реализован показ Solution
        ///     и нужно показывать скопом все версии всех конфигураций.
        ///     После отображения Solution в дизайнере, данный флаг и его дальнейшее использование нужно удалить
        /// </param>
        public MetadataReaderConfiguration(string version, bool doNotCheckVersion = false)
        {
            _version = version;
            _doNotCheckVersion = doNotCheckVersion;
        }

        public IEnumerable<dynamic> GetItems()
        {
            dynamic body = new DynamicWrapper();
            body.DoNotCheckVersion = _doNotCheckVersion;
            body.Version = _version;
            return
                DynamicWrapperExtensions.ToEnumerable(
                    RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getregisteredconfiglist", null, body).ToDynamic().ConfigList);
        }

        public dynamic GetItem(string metadataName)
        {
            dynamic bodyQuery = new DynamicWrapper();
            bodyQuery.ConfigId = metadataName;
            bodyQuery.Version = _version;

            dynamic itemResult =
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getmetadata", null, bodyQuery)
                    .ToDynamic();

            if (itemResult.QueryResult == null)
            {
                throw new ArgumentException("Fail to make metadata request. Error: {0}", itemResult);
            }

            if (itemResult.QueryResult.Count > 0)
            {
                return itemResult.QueryResult[0].Result;
            }
            return null;
        }
    }
}