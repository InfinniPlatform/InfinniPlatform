using System.Threading;

namespace InfinniPlatform.MessageQueue.Tests
{
	sealed class CountdownConsumer : IQueueConsumer
	{
		private readonly CountdownEvent _completed;

		public CountdownConsumer(CountdownEvent completed)
		{
			_completed = completed;
		}

		public void Handle(Message message)
		{
			_completed.Signal();
		}
	}
}