using System;
using System.Threading;

using InfinniPlatform.Core.Extensions;
using InfinniPlatform.Sdk.Queues.Outdated;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Менеджер для управления рабочим потоком очереди сообщений.
	/// </summary>
	sealed class MessageQueueWorker : IMessageQueueWorker, IDisposable
	{
		public MessageQueueWorker(QueueConfig queueConfig, Func<IQueueConsumer> consumerFactory, IMessageQueueCommandExecutor commandExecutor, Func<IMessageQueueSession, IMessageQueue> messageQueueFactory)
		{
			if (queueConfig == null)
			{
				throw new ArgumentNullException("queueConfig");
			}

			if (consumerFactory == null)
			{
				throw new ArgumentNullException("consumerFactory");
			}

			if (commandExecutor == null)
			{
				throw new ArgumentNullException("commandExecutor");
			}

			if (messageQueueFactory == null)
			{
				throw new ArgumentNullException("messageQueueFactory");
			}

			_queueConfig = queueConfig;
			_consumerFactory = consumerFactory;
			_commandExecutor = commandExecutor;
			_messageQueueFactory = messageQueueFactory;

			_startedEvent = new CountdownEvent(queueConfig.QueueWorkerThreadCount);
			_stoppedEvent = new CountdownEvent(queueConfig.QueueWorkerThreadCount);
		}


		private readonly QueueConfig _queueConfig;
		private readonly Func<IQueueConsumer> _consumerFactory;
		private readonly IMessageQueueCommandExecutor _commandExecutor;
		private readonly Func<IMessageQueueSession, IMessageQueue> _messageQueueFactory;

		private bool _canListen;
		private ResourceManager _queues;
		private readonly CountdownEvent _startedEvent;
		private readonly CountdownEvent _stoppedEvent;


		/// <summary>
		/// Запустить рабочий поток.
		/// </summary>
		public void Start()
		{
			if (IsRunning() == false)
			{
				lock (this)
				{
					if (IsRunning() == false)
					{
						// Сброс счетчиков

						_startedEvent.Reset();
						_stoppedEvent.Reset();

						// Запуск рабочих потоков производится с использованием пула потоков.
						// Количество рабочих потоков определяется конфигурацией очереди.

						_canListen = true;
						_queues = new ResourceManager();

						for (var i = 0; i < _queueConfig.QueueWorkerThreadCount; ++i)
						{
							ThreadPool.QueueUserWorkItem(WorkerThread);
						}

						_startedEvent.Wait();
					}
				}
			}
		}

		/// <summary>
		/// Остановить рабочий поток.
		/// </summary>
		public void Stop()
		{
			if (IsRunning())
			{
				lock (this)
				{
					if (IsRunning())
					{
						// Остановка рабочих потоков производится путем закрытия всех прослушиваемых очередей.
						// Это приводит к штатному завершению цикла обработки сообщений.

						_canListen = false;
						_queues.Dispose();
						_stoppedEvent.Wait();

						// Сброс счетчиков

						_startedEvent.Reset();
						_stoppedEvent.Reset();
					}
				}
			}
		}

		/// <summary>
		/// Освободить используемые ресурсы.
		/// </summary>
		public void Dispose()
		{
			Stop();
		}


		private void WorkerThread(object state)
		{
			_startedEvent.Signal();

			try
			{
				// Рабочий поток пытается выполнить прослушивание до тех пор, пока не будет вызван метод остановки рабочего потока.
				// Если функция прослушивания прерывается по какой-либо причине, например, в случае длительного или частого обрыва
				// соединения с сервером очереди сообщений, производится проверка того, что рабочий поток отработал достаточно долго.
				// Если так, то это считается временным сбоем, следовательно, можно предпринять еще одну попытку выполнить прослушивание.

				while (_canListen)
				{
					var listenStartTime = DateTimeOffset.UtcNow;

					try
					{
						// Прослушивание выполняется под управлением исполнителя команд, который обеспечивает стратегию повторного
						// выполнения команды (прослушивателя) в случае неудачи, например, в случае обрыва соединения с сервером
						// очереди сообщений.

						_commandExecutor.Execute(session =>
													 {
														 using (var queue = _messageQueueFactory(session))
														 {
															 _queues.RegisterObject(queue);

															 ListenQueue(queue);
														 }
													 });
					}
					catch (Exception exception)
					{
						OnWorkerThreadError(exception);
					}

					var listenStopTime = DateTimeOffset.UtcNow;

					var listeningTime = (listenStopTime - listenStartTime);

					// Если прослушивание выполнялось не долго, значит, установить соединение с сервером
					// очереди сообщений так и не удалось. Следовательно, рабочий поток следует прервать.

					if (listeningTime.Milliseconds < _queueConfig.QueueMinListenTime)
					{
						break;
					}
				}
			}
			catch
			{
			}
			finally
			{
				_stoppedEvent.Signal();
			}
		}

		private void ListenQueue(IMessageQueue messageQueue)
		{
			// Извлечение сообщения из очереди, создание обработчика, обработка сообщения, и так до тех пор,
			// пока очередь сообщений не будет закрыта - по требованию, либо по причине сетевого сбоя.

			while (true)
			{
				try
				{
					var message = messageQueue.Dequeue();

					HandleMessage(messageQueue, message);
				}
				catch (Exception exception)
				{
					if (_canListen)
					{
						OnWorkerThreadError(exception);

						throw;
					}

					break;
				}
			}
		}

		private bool IsRunning()
		{
			return _startedEvent.Wait(0) && !_stoppedEvent.Wait(0);
		}

		private void OnConsumerError(Exception error)
		{
			if (_queueConfig.QueueConsumerError != null)
			{
				_queueConfig.ExecuteSilent(c => c.QueueConsumerError(error));
			}
		}

		private void OnWorkerThreadError(Exception error)
		{
			if (_queueConfig.QueueWorkerThreadError != null)
			{
				_queueConfig.ExecuteSilent(c => c.QueueWorkerThreadError(error));
			}
		}

		private void HandleMessage(IMessageQueue messageQueue, Message message)
		{
			var rejectPolicy = _queueConfig.QueueRejectPolicy;

			// Если не отказываемся обрабатывать сообщение
			if (rejectPolicy == null || rejectPolicy.MustReject() == false)
			{
				var acknowledgePolicy = _queueConfig.QueueAcknowledgePolicy;

				if (acknowledgePolicy != null)
				{
					var acknowledge = false;

					// Подтверждение окончания обработки сообщения, если разрешено
					if (acknowledgePolicy.OnBefore())
					{
						messageQueue.Acknowledge(message);
						acknowledge = true;
					}

					var consumer = _consumerFactory();

					try
					{
						consumer.Handle(message);

						// Подтверждение окончания обработки сообщения, если разрешено
						if (!acknowledge && acknowledgePolicy.OnSuccess())
						{
							messageQueue.Acknowledge(message);
						}
					}
					catch (Exception exception)
					{
						OnConsumerError(exception);

						// Подтверждение окончания обработки сообщения, если разрешено
						if (!acknowledge && acknowledgePolicy.OnFailure(exception))
						{
							messageQueue.Acknowledge(message);
						}
					}
				}
				else
				{
					var consumer = _consumerFactory();

					try
					{
						consumer.Handle(message);
					}
					catch (Exception exception)
					{
						OnConsumerError(exception);
					}
				}
			}
			else
			{
				messageQueue.Reject(message);
			}
		}
	}
}