using System.Diagnostics;

namespace InfinniPlatform.Api.Profiling.Implementation
{
	public sealed class WatchProfiler : IOperationProfiler
	{
		private readonly ISnapshotFormatter _snapshotFormatter;
		private Stopwatch _currentWatch;

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
