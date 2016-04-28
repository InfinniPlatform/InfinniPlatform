﻿namespace InfinniPlatform.Sdk.Queues
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