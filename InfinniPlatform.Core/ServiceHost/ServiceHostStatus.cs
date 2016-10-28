namespace InfinniPlatform.Core.ServiceHost
{
    public enum ServiceHostStatus
    {
        InitializePending,
        Initialized,

        StartPending,
        Started,

        StopPending,
        Stopped
    }
}