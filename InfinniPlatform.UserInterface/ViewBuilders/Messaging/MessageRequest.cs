using System;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Запрос на обработку сообщения.
    /// </summary>
    public sealed class MessageRequest
    {
        private readonly TaskCompletionSource<bool> _requestCompletion;

        /// <summary>
        ///     Точка обмена.
        /// </summary>
        public readonly string ExchangeName;

        /// <summary>
        ///     Тело сообщения.
        /// </summary>
        public readonly object MessageBody;

        /// <summary>
        ///     Тип сообщения.
        /// </summary>
        public readonly string MessageType;

        public MessageRequest(string exchangeName, string messageType, dynamic messageBody)
        {
            ExchangeName = exchangeName;
            MessageType = messageType;
            MessageBody = messageBody;

            _requestCompletion = new TaskCompletionSource<bool>();
        }

        /// <summary>
        ///     Возвращает задачу обработки запроса.
        /// </summary>
        public Task RequestTask
        {
            get { return _requestCompletion.Task; }
        }

        /// <summary>
        ///     Устанавливает признак успешного завершения обработки запроса.
        /// </summary>
        public void OnSuccessComplete()
        {
            _requestCompletion.SetResult(true);
        }

        /// <summary>
        ///     Устанавливает признак неуспешного завершения обработки запроса.
        /// </summary>
        public void OnErrorComplete(Exception error)
        {
            _requestCompletion.SetException(error);
        }
    }
}