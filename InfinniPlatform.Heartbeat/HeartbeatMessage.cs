namespace InfinniPlatform.Heartbeat
{
    /// <summary>
    /// Сообщение для Infinni.Server.
    /// </summary>
    public class HeartbeatMessage
    {
        public HeartbeatMessage(string message, string name, string instanceId)
        {
            Message = message;
            Name = name;
            InstanceId = instanceId;
        }

        /// <summary>
        /// Имя экземпляра приложения.
        /// </summary>
        public string InstanceId;

        /// <summary>
        /// Содержимое сообщения.
        /// </summary>
        public string Message;

        /// <summary>
        /// Имя машины.
        /// </summary>
        public string Name;
    }
}