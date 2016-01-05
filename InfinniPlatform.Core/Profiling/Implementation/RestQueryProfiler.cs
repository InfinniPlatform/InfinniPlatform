using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Core.Profiling.Implementation
{
    public sealed class RestQueryProfiler : IOperationProfiler
    {
        private readonly WatchProfiler _profiler;

        public RestQueryProfiler(ILog log, string configId, string metadata, string action, dynamic body)
        {
            _profiler = new WatchProfiler(new SnapshotFormatterLog(log, configId, metadata, action, body));
        }

        public void Reset()
        {
            _profiler.Reset();
        }

        public void TakeSnapshot()
        {
            _profiler.TakeSnapshot();
        }
    }
}