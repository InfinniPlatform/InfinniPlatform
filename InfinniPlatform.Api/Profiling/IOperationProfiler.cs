namespace InfinniPlatform.Api.Profiling
{
    public interface IOperationProfiler
    {
        void Reset();
        void TakeSnapshot();
    }
}