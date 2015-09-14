using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Profiling;

namespace InfinniPlatform.Api.Profiling.Implementation
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