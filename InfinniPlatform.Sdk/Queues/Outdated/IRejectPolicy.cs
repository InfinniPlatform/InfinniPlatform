namespace InfinniPlatform.Sdk.Queues.Outdated
{
    /// <summary>
    ///     Политика подтверждения отказа от выполнения действия.
    /// </summary>
    public interface IRejectPolicy
    {
        /// <summary>
        ///     Отказаться от выполнения действия.
        /// </summary>
        bool MustReject();
    }
}