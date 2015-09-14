namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Сервис для управления рабочими потоками очередей сообщений.
    /// </summary>
    public interface IMessageQueueListener
    {
        /// <summary>
        ///     Запустить прослушивание очереди.
        /// </summary>
        /// <param name="queueName">Наименование очереди сообщений.</param>
        void StartListen(string queueName);

        /// <summary>
        ///     Остановить прослушивание очереди.
        /// </summary>
        /// <param name="queueName">Наименование очереди сообщений.</param>
        void StopListen(string queueName);

        /// <summary>
        ///     Запустить прослушивание всех очередей.
        /// </summary>
        void StartListenAll();

        /// <summary>
        ///     Остановить прослушивание всех очередей.
        /// </summary>
        void StopListenAll();
    }
}