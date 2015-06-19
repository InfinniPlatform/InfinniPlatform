using System.Diagnostics;
using System.Reflection;

namespace InfinniPlatform.SystemInfo
{
    /// <summary>
    ///     Провайдер информации о системе.
    /// </summary>
    /// <remarks>
    ///     Предоставляет базовую информацию о системе, например, версия, номер сборки, состояние окружения и самой системы и
    ///     т.д.
    /// </remarks>
    public sealed class SystemInfoProvider : ISystemInfoProvider
    {
        public object GetSystemInfo()
        {
            // Todo: Подумать над составом информации

            return new
            {
                SystemVersion = GetSystemVersion()
            };
        }

        private static string GetSystemVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return versionInfo.FileVersion;
        }
    }
}