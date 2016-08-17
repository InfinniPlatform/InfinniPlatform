using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии системы в целом.
    /// </summary>
    internal class SystemStatusProvider : ISystemStatusProvider
    {
        public SystemStatusProvider(IEnumerable<ISubsystemStatusProvider> subsystems, ILog log)
        {
            _subsystems = subsystems;
            _log = log;
        }


        private readonly IEnumerable<ISubsystemStatusProvider> _subsystems;
        private readonly ILog _log;


        public async Task<object> GetStatus()
        {
            var status = new DynamicWrapper { { "version", GetSystemVersion() } };

            var allOk = true;

            if (_subsystems != null)
            {
                foreach (var subsystem in _subsystems)
                {
                    string subsystemName = null;

                    try
                    {
                        subsystemName = subsystem.Name;

                        try
                        {
                            var subsystemStatus = await subsystem.GetStatus();
                            status[subsystemName] = subsystemStatus ?? new DynamicWrapper();
                        }
                        catch (Exception)
                        {
                            var subsystemStatus = new DynamicWrapper { { "error", Resources.CouldNotGetStatusForTheSubsystem } };
                            status[subsystemName] = subsystemStatus;
                            throw;
                        }
                    }
                    catch (Exception exception)
                    {
                        allOk = false;

                        _log.Error(Resources.CouldNotGetStatusForTheSubsystem, exception, () => new Dictionary<string, object> { { "subsystemName", subsystemName } });
                    }
                }
            }

            status["ok"] = allOk;

            return status;
        }


        private static string GetSystemVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return versionInfo.FileVersion;
        }
    }
}