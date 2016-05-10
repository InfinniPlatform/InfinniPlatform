namespace InfinniPlatform.Sdk.Queues
{
    public interface IEventingConsumer
    {
        /// <summary>
        /// ���������� ���������.
        /// </summary>
        /// <param name="messageBytes">��������� � ���� ������� ����.</param>
        void Consume(byte[] messageBytes);
    }
}