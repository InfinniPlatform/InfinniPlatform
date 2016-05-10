using System;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    /// <summary>
    /// ����������� ��������� �� ������� �� �������.
    /// </summary>
    public interface IBasicConsumer : IDisposable
    {
        /// <summary>
        /// �������� ��������� �� �������.
        /// </summary>
        /// <returns>������ ��������� � ������� ��� null, ���� ��������� ���.</returns>
        string Get();
    }
}