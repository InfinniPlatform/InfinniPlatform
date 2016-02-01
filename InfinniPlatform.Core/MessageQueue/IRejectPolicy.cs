namespace InfinniPlatform.Core.MessageQueue
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