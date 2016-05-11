using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
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
        IMessage Consume<T>() where T : class;
    }
}