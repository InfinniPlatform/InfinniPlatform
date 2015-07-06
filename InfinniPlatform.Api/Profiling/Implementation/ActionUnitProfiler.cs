using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Api.Profiling.Implementation
{
    public sealed class ActionUnitProfiler : IOperationProfiler
    {
        private readonly WatchProfiler _profiler;

        public ActionUnitProfiler(ILog log, string methodName, string arguments)
        {
            _profiler = new WatchProfiler(new ActionUnitFormatterLog(log, methodName, arguments));
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