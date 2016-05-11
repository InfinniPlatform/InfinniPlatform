using System;

namespace InfinniPlatform.Sdk.Queues.Outdated
{
    /// <summary>
    ///     Политика подтверждения окончания выполнения действия.
    /// </summary>
    public interface IAcknowledgePolicy
    {
        /// <summary>
        ///     Перед выполнением действия.
        /// </summary>
        bool OnBefore();

        /// <summary>
        ///     После успешного выполнения действия.
        /// </summary>
        bool OnSuccess();

        /// <summary>
        ///     После неуспешного выполнения действия.
        /// </summary>
        bool OnFailure(Exception error);
    }
}