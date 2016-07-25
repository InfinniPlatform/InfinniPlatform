using System;

namespace InfinniPlatform.Caching.RabbitMQ
{
    public interface IRabbitBus
    {
        /// <summary>
        /// ���������� ������� ��������� ���������.
        /// </summary>
        Action<SharedCacheMessage> OnMessageRecieve { get; set; }

        /// <summary>
        /// ���������� �������.
        /// </summary>
        /// <param name="key">����</param>
        /// <param name="value">��������</param>
        void Publish(string key, string value);
    }
}