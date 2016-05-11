using System;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.RabbitMq
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class QueueConfigTest
	{
		private const string ExchangeName = "TestExchange";
		private const string QueueName = "TestQueue";


		[Test]
		public void ShouldSetName()
		{
			// When
			var target = new QueueConfig(ExchangeName, QueueName);

			// Then
			Assert.AreEqual(ExchangeName, target.ExchangeName);
			Assert.AreEqual(QueueName, target.QueueName);
		}

		[Test]
		public void ShouldSetDurable()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			// When
			config.Durable();

			// Then
			Assert.IsTrue(target.QueueDurable);
		}

		[Test]
		public void ShouldSetConsumerId()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			var value = Guid.NewGuid().ToString();

			// When
			config.ConsumerId(value);

			// Then
			Assert.AreEqual(value, target.QueueConsumerId);
		}

		[Test]
		public void ShouldSetConsumerError()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			Action<Exception> value = error => { };

			// When
			config.ConsumerError(value);

			// Then
			Assert.AreEqual(value, target.QueueConsumerError);
		}

		[Test]
		public void ShouldSetPrefetchSize()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			const int value = 12345;

			// When
			config.PrefetchSize(value);

			// Then
			Assert.AreEqual(value, target.QueuePrefetchSize);
		}

		[Test]
		public void ShouldSetPrefetchCount()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			const int value = 12345;

			// When
			config.PrefetchCount(value);

			// Then
			Assert.AreEqual(value, target.QueuePrefetchCount);
		}

		[Test]
		public void ShouldSetWorkerThreadCount()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			const int value = 5;

			// When
			config.WorkerThreadCount(value);

			// Then
			Assert.AreEqual(value, target.QueueWorkerThreadCount);
		}

		[Test]
		public void ShouldSetWorkerThreadError()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			Action<Exception> value = error => { };

			// When
			config.WorkerThreadError(value);

			// Then
			Assert.AreEqual(value, target.QueueWorkerThreadError);
		}

		[Test]
		public void ShouldSetMinListenTime()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			const int value = 60000;

			// When
			config.MinListenTime(value);

			// Then
			Assert.AreEqual(value, target.QueueMinListenTime);
		}

		[Test]
		public void ShouldSetAcknowledgePolicy()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			IAcknowledgePolicy value = new Mock<IAcknowledgePolicy>().Object;

			// When
			config.AcknowledgePolicy(value);

			// Then
			Assert.AreEqual(value, target.QueueAcknowledgePolicy);
		}

		[Test]
		public void ShouldSetRejectPolicy()
		{
			// Given
			var target = new QueueConfig(ExchangeName, QueueName);
			var config = (IQueueConfig)target;

			IRejectPolicy value = new Mock<IRejectPolicy>().Object;

			// When
			config.RejectPolicy(value);

			// Then
			Assert.AreEqual(value, target.QueueRejectPolicy);
		}
	}
}