using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Api.Profiling.Implementation
{
    internal sealed class ActionUnitFormatterLog : ISnapshotFormatter
    {
        private readonly string _arguments;
        private readonly ILog _log;
        private readonly string _methodName;

        public ActionUnitFormatterLog(ILog log, string methodName, string arguments)
        {
            _log = log;
            _methodName = methodName;
            _arguments = arguments;
        }

        public void FormatSnapshot(Snapshot snapshot)
        {
            _log.Info("Run method {0} with arguments {1}. ELAPSED {2} ms", _methodName, _arguments,
                snapshot.ElapsedMilliseconds);
        }
    }
}