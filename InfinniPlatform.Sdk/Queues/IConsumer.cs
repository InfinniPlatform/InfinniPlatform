namespace InfinniPlatform.Sdk.Queues
{
    public interface IConsumer
    {
        string QueueName { get; }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        /// <param name="messageBytes">��������� � ���� ������� ����.</param>
        void Consume(byte[] messageBytes);
    }
}