namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Обработчик очереди сообщений.
    /// </summary>
    /// <remarks>
    ///     Очередь может обрабатываться несколькими рабочими потоками с использованием одного и того же обработчика.
    ///     По этой причине нужно стараться делать код обработчика потокобезопасным и не зависящем от состояния.
    /// </remarks>
    public interface IQueueConsumer
    {
        /// <summary>
        ///     Обработать сообщение.
        /// </summary>
        /// <param name="message">Сообщение очереди.</param>
        void Handle(Message message);
    }
}