using System.Diagnostics;
using System.Reflection;

using InfinniPlatform.Core.SystemInfo;
using InfinniPlatform.ElasticSearch.ElasticProviders;

namespace InfinniPlatform.SystemConfig.SystemInfo
{
    /// <summary>
    /// Провайдер информации о системе.
    /// </summary>
    /// <remarks>
    /// Предоставляет базовую информацию о системе, например, версия, номер сборки, состояние окружения и самой системы и т.д.
    /// </remarks>
    internal sealed class SystemInfoProvider : ISystemInfoProvider
    {
        public SystemInfoProvider(ElasticConnection elasticConnection)
        {
            _elasticConnection = elasticConnection;
        }

        private readonly ElasticConnection _elasticConnection;

        public object GetSystemInfo()
        {
            // TODO: Подумать над составом информации

            return new
                   {
                       SystemVersion = GetSystemVersion(),
                       ElasticSearch = GetElasticSearchStatus()
                   };
        }

        private static string GetSystemVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return versionInfo.FileVersion;
        }

        private string GetElasticSearchStatus()
        {
            return _elasticConnection.GetStatus();
        }


    }
}