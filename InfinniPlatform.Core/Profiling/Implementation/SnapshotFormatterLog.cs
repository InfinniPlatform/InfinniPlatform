using System.Collections.Generic;

using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Core.Profiling.Implementation
{
    public sealed class SnapshotFormatterLog : ISnapshotFormatter
    {
        private readonly string _action;
        private readonly dynamic _body;
        private readonly string _configId;
        private readonly ILog _log;
        private readonly string _metadata;

        public SnapshotFormatterLog(ILog log, string configId, string metadata, string action, dynamic body)
        {
            _log = log;
            _configId = configId;
            _metadata = metadata;
            _action = action;
            _body = body;
        }

        public void FormatSnapshot(Snapshot snapshot)
        {
            _log.Info("Method execution captured (ms).", new Dictionary<string, object>
                                                         {
                                                             { "configurationId", _configId },
                                                             { "metadata", _metadata },
                                                             { "action", _action },
                                                             { "arguments", _body != null ? _body.ToString() : "<noargs>" },
                                                             { "elapsed", snapshot.ElapsedMilliseconds },
                                                         });
        }
    }
}