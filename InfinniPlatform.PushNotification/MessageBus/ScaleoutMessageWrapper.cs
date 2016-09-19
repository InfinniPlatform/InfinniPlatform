namespace InfinniPlatform.PushNotification.MessageBus
{
    /// <summary>
    /// Обертка для сообщений масштабирующей шины SignalR.
    /// </summary>
    internal class ScaleoutMessageWrapper
    {
        public int StreamIndex { get; set; }

        public ulong Id { get; set; }

        public byte[] Body { get; set; }
    }
}