using System.Threading.Tasks;

using InfinniPlatform.Core.SystemInfo;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для получения информации о системе.
    /// </summary>
    internal sealed class SystemInfoHttpService : IHttpService
    {
        public SystemInfoHttpService(ISystemInfoProvider systemInfoProvider)
        {
            _systemInfoProvider = systemInfoProvider;
        }

        private readonly ISystemInfoProvider _systemInfoProvider;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/"] = GetSystemInfo;
            builder.Get["/Info"] = GetSystemInfo;
            builder.Post["/"] = GetSystemInfo;
            builder.Post["/Info"] = GetSystemInfo;
        }

        private Task<object> GetSystemInfo(IHttpRequest request)
        {
            var systemInfo = _systemInfoProvider.GetSystemInfo();

            return Task.FromResult(systemInfo);
        }
    }
}