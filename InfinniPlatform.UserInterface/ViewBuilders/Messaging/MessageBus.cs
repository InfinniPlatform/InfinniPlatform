using System;
using System.Collections.Concurrent;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
	/// <summary>
	/// Шина сообщений.
	/// </summary>
	public sealed class MessageBus : IMessageBus
	{
		public MessageBus()
		{
			_messageQueue = new MessageQueue<MessageRequest>(MessageHandleAsync);
			_messageExchanges = new ConcurrentDictionary<string, MessageExchange>();
		}


		private readonly MessageQueue<MessageRequest> _messageQueue;
		private readonly ConcurrentDictionary<string, MessageExchange> _messageExchanges;


		private async void MessageHandleAsync(MessageRequest request)
		{
			try
			{
				var exchange = GetInternalExchange(request.ExchangeName);
				await exchange.HandleAsync(request.MessageType, request.MessageBody);

				request.OnSuccessComplete();
			}
			catch (Exception error)
			{
				request.OnErrorComplete(error);
			}
		}


		/// <summary>
		/// Возвращает точку обмена сообщениями.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена сообщениями.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public IMessageExchange GetExchange(string exchangeName)
		{
			return GetInternalExchange(exchangeName);
		}

		private MessageExchange GetInternalExchange(string exchangeName)
		{
			if (exchangeName == null)
			{
				throw new ArgumentNullException("exchangeName");
			}

			return _messageExchanges.GetOrAdd(exchangeName, key => new MessageExchange(key, _messageQueue));
		}


		public void Dispose()
		{
			_messageQueue.Dispose();
		}
	}
}