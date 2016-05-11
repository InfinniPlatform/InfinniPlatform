using System;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Менеджер сессий очереди сообщений, реализующий стратегию времени жизни сессии на время работы потока.
	/// </summary>
	public sealed class MessageQueueSessionManagerPerThread : IMessageQueueSessionManager
	{
		public MessageQueueSessionManagerPerThread(IMessageQueueSessionFactory sessionFactory)
		{
			if (sessionFactory == null)
			{
				throw new ArgumentNullException("sessionFactory");
			}

			_sessionFactory = sessionFactory;
		}


		private readonly IMessageQueueSessionFactory _sessionFactory;


		[ThreadStatic]
		private static IMessageQueueSession _session;

		public IMessageQueueSession OpenSession()
		{
			if (_session == null)
			{
				_session = _sessionFactory.OpenSession();
			}
			else if (_session.IsOpen == false)
			{
				_session.Dispose();
				_session = _sessionFactory.OpenSession();
			}

			return _session;
		}

		public void CloseSession(IMessageQueueSession session)
		{
		}
	}
}