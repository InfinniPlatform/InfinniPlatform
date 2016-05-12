using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Sdk.Queues.Outdated;

namespace InfinniPlatform.MessageQueue.Tests.Outdated
{
	sealed class TestConsumer : IQueueConsumer
	{
		private readonly List<string> _messages;
		private readonly CountdownEvent _completed;

		public TestConsumer(CountdownEvent completed)
		{
			_messages = new List<string>();
			_completed = completed;
		}

		// ReSharper disable MemberHidesStaticFromOuterClass

		public string[] Messages
		{
			get { return _messages.ToArray(); }
		}

		// ReSharper restore MemberHidesStaticFromOuterClass

		public void Handle(Message message)
		{
			// Блокировка нужна, чтобы корректно посчитать все полученные сообщения, поскольку
			// прослушиватель может быть доступен из нескольких рабочих потоков

			lock (this)
			{
				_messages.Add(message.GetBodyObject<string>());
			}

			_completed.Signal();
		}
	}
}