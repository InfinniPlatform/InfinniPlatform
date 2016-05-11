namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    /// <summary>
    /// ���������� ��������� �� ������� �� �������.
    /// </summary>
    public interface IBasicConsumer
    {
        string QueueName { get; }

        /// <summary>
        /// �������� ��������� �� �������.
        /// </summary>
        /// <returns>������ ��������� � ������� ��� null, ���� ��������� ���.</returns>
        string Consume();
    }
}