using System;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// ��������� � �������.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// ���������� ���� ���������.
        /// </summary>
        object GetBody();

        /// <summary>
        /// ���������� ��� ���� ���������.
        /// </summary>
        Type GetBodyType();
    }
}