using System.Collections.Generic;

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
            _log.Info("Method execution captured (ms).", new Dictionary<string, object>
                                                         {
                                                             { "method", _methodName },
                                                             { "arguments", _arguments },
                                                             { "elapsed", snapshot.ElapsedMilliseconds },
                                                         });
        }
    }
}