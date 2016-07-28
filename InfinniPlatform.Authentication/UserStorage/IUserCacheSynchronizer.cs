using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// ��������� ������������� ���� ������������� ����� ������� ���������.
    /// </summary>
    public interface IUserCacheSynchronizer
    {
        /// <summary>
        /// ������������ ��������� �� �������.
        /// </summary>
        /// <param name="message">��������� �� �������.</param>
        Task ProcessMessage(Message<string> message);

        /// <summary>
        /// ���������� ����������� �� ��������� ������������.
        /// </summary>
        /// <param name="userId">������������� ������������.</param>
        void NotifyUserChanged(string userId);
    }
}