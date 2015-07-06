using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Api.Profiling.Implementation
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
            _log.Info("Config: {0}, Metadata: {1}, Action {2}, with arguments: {3}. ELAPSED {4} ms",
                _configId, _metadata, _action, _body != null ? _body.ToString() : "<no arguments>",
                snapshot.ElapsedMilliseconds);
        }
    }
}