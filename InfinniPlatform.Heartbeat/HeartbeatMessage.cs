namespace InfinniPlatform.Heartbeat
{
    /// <summary>
    /// ��������� ��� Infinni.Server.
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
        /// ��� ���������� ����������.
        /// </summary>
        public string InstanceId;

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        public string Message;

        /// <summary>
        /// ��� ������.
        /// </summary>
        public string Name;
    }
}