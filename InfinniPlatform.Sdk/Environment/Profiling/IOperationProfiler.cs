namespace InfinniPlatform.Sdk.Environment.Profiling
{
    public interface IOperationProfiler
    {
        void Reset();
        void TakeSnapshot();
    }
}