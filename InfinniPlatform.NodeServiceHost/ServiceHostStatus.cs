namespace InfinniPlatform.NodeServiceHost
{
    public enum ServiceHostStatus
    {
        Stopped,
        InitializationPending,
        Initializing,
        StartPending,
        Running,
        StopPending
    }
}