using System;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
	/// <summary>
	/// Подписка на обработку сообщения.
	/// </summary>
	sealed class Subscription : IDisposable
	{
		public Subscription(Action unsubscribeAction, Action<dynamic> messageHandler)
		{
			_messageHandler = messageHandler;
			_unsubscribeAction = unsubscribeAction;
		}


		private readonly Action _unsubscribeAction;
		private readonly Action<dynamic> _messageHandler;


		/// <summary>
		/// Обрабатывает сообщение.
		/// </summary>
		/// <param name="messageBody">Тело сообщения.</param>
		/// <returns>Задача обработки.</returns>
		public Task HandleAsync(dynamic messageBody)
		{
			return Task.Run((Action)(() => _messageHandler(messageBody)));
		}

		public void Dispose()
		{
			_unsubscribeAction();
		}
	}
}