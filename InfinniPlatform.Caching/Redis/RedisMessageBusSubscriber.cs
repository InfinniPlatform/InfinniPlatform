using System;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// ��������� ���� ��������� Redis.
    /// </summary>
    internal sealed class RedisMessageBusSubscriber : IDisposable
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="handleAction">���������� ���������.</param>
        /// <param name="unsubscribeAction">������� ������� �� ���������.</param>
        public RedisMessageBusSubscriber(Action<string, string> handleAction, Action unsubscribeAction)
        {
            _handleAction = handleAction;
            _unsubscribeAction = unsubscribeAction;
        }


        private readonly Action<string, string> _handleAction;
        private readonly Action _unsubscribeAction;


        /// <summary>
        /// ������������ ���������.
        /// </summary>
        public void Handle(string key, string value)
        {
            _handleAction(key, value);
        }


        public void Dispose()
        {
            _unsubscribeAction();
        }
    }
}