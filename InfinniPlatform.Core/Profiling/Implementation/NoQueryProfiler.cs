using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Core.Profiling.Implementation
{
    public sealed class NoQueryProfiler : IOperationProfiler
    {
        public void Reset()
        {
        }

        public void TakeSnapshot()
        {
        }
    }
}