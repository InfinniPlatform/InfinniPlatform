using System;
using System.Collections.Generic;

using InfinniPlatform.Extensions;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.RabbitMq
{
	/// <summary>
	/// Сервис для управления рабочими потоками очередей сообщений.
	/// </summary>
	public sealed class MessageQueueListener : IMessageQueueListener, IMessageQueueWorkerContainer
	{
		private readonly Dictionary<string, IMessageQueueWorker> _queueWorkers
			= new Dictionary<string, IMessageQueueWorker>();


		/// <summary>
		/// Зарегистрировать рабочий поток очереди сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		/// <param name="queueWorker">Интерфейс для управления рабочим потоком очереди сообщений.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void RegisterWorker(string queueName, IMessageQueueWorker queueWorker)
		{
			ForWorker(queueName, previousQueueWorker =>
									 {
										 if (previousQueueWorker != null)
										 {
											 previousQueueWorker.ExecuteSilent(w => w.Stop());
										 }

										 _queueWorkers[queueName] = queueWorker;
									 },
					  ifExists: false);
		}

		/// <summary>
		/// Отменить регистрацию рабочего потока очереди сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void UnregisterWorker(string queueName)
		{
			ForWorker(queueName, queueWorker =>
									 {
										 _queueWorkers.Remove(queueName);

										 queueWorker.ExecuteSilent(w => w.Stop());
									 });
		}


		/// <summary>
		/// Запустить прослушивание очереди.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		public void StartListen(string queueName)
		{
			ForWorker(queueName, queueWorker => queueWorker.Start());
		}

		/// <summary>
		/// Остановить прослушивание очереди.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		public void StopListen(string queueName)
		{
			ForWorker(queueName, queueWorker => queueWorker.Stop());
		}


		/// <summary>
		/// Запустить прослушивание всех очередей.
		/// </summary>
		public void StartListenAll()
		{
			ForEachWorkers(queueWorker => queueWorker.Start());
		}

		/// <summary>
		/// Остановить прослушивание всех очередей.
		/// </summary>
		public void StopListenAll()
		{
			ForEachWorkers(queueWorker => queueWorker.Stop());
		}



		private void ForWorker(string queueName, Action<IMessageQueueWorker> action, bool ifExists = true)
		{
			if (string.IsNullOrWhiteSpace(queueName))
			{
				throw new ArgumentNullException("queueName");
			}

			// В данном случае блокировка нужна, чтобы не остались "забытые" рабочие потоки.
			// Сама блокировка не страшна, поскольку данный метод не участвует в нагрузочных сценариях.

			lock (this)
			{
				IMessageQueueWorker queueWorker;

				if (_queueWorkers.TryGetValue(queueName, out queueWorker) || ifExists == false)
				{
					action(queueWorker);
				}
			}
		}

		private void ForEachWorkers(Action<IMessageQueueWorker> action)
		{
			var errors = new List<Exception>();

			// В данном случае блокировка нужна, чтобы не остались "забытые" рабочие потоки.
			// Сама блокировка не страшна, поскольку данный метод не участвует в нагрузочных сценариях.

			lock (this)
			{
				foreach (var queueWorker in _queueWorkers.Values)
				{
					try
					{
						action(queueWorker);
					}
					catch (Exception error)
					{
						errors.Add(error);
					}
				}
			}

			if (errors.Count > 0)
			{
				throw new AggregateException(errors);
			}
		}
	}
}