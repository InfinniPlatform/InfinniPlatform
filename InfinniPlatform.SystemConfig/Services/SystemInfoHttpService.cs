using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для получения информации о системе.
    /// </summary>
    internal sealed class SystemInfoHttpService : IHttpService
    {
        public SystemInfoHttpService(ISystemStatusProvider systemStatusProvider)
        {
            _systemStatusProvider = systemStatusProvider;
        }

        private readonly ISystemStatusProvider _systemStatusProvider;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/"] = GetSystemInfo;
            builder.Get["/Info"] = GetSystemInfo;
            builder.Post["/"] = GetSystemInfo;
            builder.Post["/Info"] = GetSystemInfo;
        }

        private Task<object> GetSystemInfo(IHttpRequest request)
        {
            return _systemStatusProvider.GetStatus();
        }
    }
}