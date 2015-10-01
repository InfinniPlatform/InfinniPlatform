using System.Diagnostics;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Api.Profiling.Implementation
{
    public sealed class WatchProfiler : IOperationProfiler
    {
        private Stopwatch _currentWatch;
        private readonly ISnapshotFormatter _snapshotFormatter;

        public WatchProfiler(ISnapshotFormatter snapshotFormatter)
        {
            _snapshotFormatter = snapshotFormatter;
        }

        public void Reset()
        {
            _currentWatch = Stopwatch.StartNew();
        }

        public void TakeSnapshot()
        {
            _currentWatch.Stop();

            if (_snapshotFormatter != null)
            {
                _snapshotFormatter.FormatSnapshot(new Snapshot {ElapsedMilliseconds = _currentWatch.ElapsedMilliseconds});
            }
        }
    }
}