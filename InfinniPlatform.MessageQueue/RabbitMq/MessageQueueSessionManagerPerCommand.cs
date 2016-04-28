using System;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Менеджер сессий очереди сообщений, реализующий стратегию времени жизни сессии на время выполнения команды.
	/// </summary>
	public sealed class MessageQueueSessionManagerPerCommand : IMessageQueueSessionManager
	{
		public MessageQueueSessionManagerPerCommand(IMessageQueueSessionFactory sessionFactory)
		{
			if (sessionFactory == null)
			{
				throw new ArgumentNullException("sessionFactory");
			}

			_sessionFactory = sessionFactory;
		}


		private readonly IMessageQueueSessionFactory _sessionFactory;


		public IMessageQueueSession OpenSession()
		{
			return _sessionFactory.OpenSession();
		}

		public void CloseSession(IMessageQueueSession session)
		{
			session.Dispose();
		}
	}
}