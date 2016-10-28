namespace InfinniPlatform.NodeServiceHost
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