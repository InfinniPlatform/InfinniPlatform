using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Extensions;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Client
{
	/// <summary>
	/// Сессия очереди сообщений RabbitMq.
	/// </summary>
	sealed class RabbitMqSession : IMessageQueueSession
	{
		public RabbitMqSession(string host, int port)
		{
			_messageConverter = new RabbitMqMessageConverter(() => _channel.CreateBasicProperties());
			_connectionFactory = () => new ConnectionFactory { HostName = host, Port = port };
			_connection = _connectionFactory().CreateConnection();
			_channel = _connection.CreateModel();
		}


		private readonly RabbitMqMessageConverter _messageConverter;
		private readonly Func<ConnectionFactory> _connectionFactory;
		private IConnection _connection;
		private IModel _channel;


		/// <summary>
		/// Выполнить действие в другом подключении.
		/// </summary>
		/// <param name="action">Выполняемое действие.</param>
		/// <returns>
		/// Если в рамках подключения к RabbitMq происходит исключение, канал закрывается, и дальнейшая работа с ним невозможна. С одной стороны
		/// это вполне разумно, но иногда возникает необходимость в особой обработке, если при выполнении действия произошло исключение. Однако 
		/// такая обработка невозможна, если предыдущее действие было выполнено в рамках того же подключения. Например, протокол AMQP и его 
		/// реализация в RabbitMq не позволяют просматривать список существующих точек обмена и очередей, вместо этого есть команда "пассивного" 
		/// объявления точки обмена и очереди, которая принимает только имя создаваемого элемента, и в случае его отсутствия, выдает исключение. 
		/// После этого использовать этот канал не представляется возможным. Тем не менее, сама концепция реализуемой абстракции (сессия подключений), 
		/// предполагает работу только с одним подключением и возможностью проверки существования точки обмена и очереди. Поэтому подобные операции 
		/// осуществляются в рамках отдельного подключения. Этот же подход используется при автоматическом создании точки обмена в момент подписки 
		/// для обеспечения устойчивости к перезапускам сервера RabbitMq.
		/// </returns>
		private void ExecuteAnotherChannel(Action<IModel> action)
		{
			using (var connection = _connectionFactory().CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					action(channel);
				}
			}
		}


		/// <summary>
		/// Является ли сессия открытой и готовой к использованию.
		/// </summary>
		public bool IsOpen
		{
			get { return (_channel != null && _channel.IsOpen); }
		}


		/// <summary>
		/// Существует ли точка обмена.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена.</param>
		public bool ExchangeExists(string exchangeName)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException("exchangeName");
			}

			try
			{
				ExecuteAnotherChannel(channel => channel.ExchangeDeclarePassive(exchangeName));
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Fanout".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		public void CreateExchangeFanout(ExchangeConfig exchangeConfig)
		{
			CreateExchange(ExchangeType.Fanout, exchangeConfig);
		}

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Direct".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		public void CreateExchangeDirect(ExchangeConfig exchangeConfig)
		{
			CreateExchange(ExchangeType.Direct, exchangeConfig);
		}

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Direct".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		public void CreateExchangeTopic(ExchangeConfig exchangeConfig)
		{
			CreateExchange(ExchangeType.Topic, exchangeConfig);
		}

		/// <summary>
		/// Получить точку обмена сообщениями с типом "Headers".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		public void CreateExchangeHeaders(ExchangeConfig exchangeConfig)
		{
			CreateExchange(ExchangeType.Headers, exchangeConfig);
		}

		/// <summary>
		/// Удалить точку обмена.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена.</param>
		public void DeleteExchange(string exchangeName)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException();
			}

			_channel.ExchangeDelete(exchangeName);
		}

		private void CreateExchange(string exchangeType, ExchangeConfig exchangeConfig)
		{
			if (exchangeType == null)
			{
				throw new ArgumentNullException("exchangeType");
			}

			if (exchangeConfig == null)
			{
				throw new ArgumentNullException("exchangeConfig");
			}

			_channel.ExchangeDeclare(exchangeConfig.ExchangeName, exchangeType, exchangeConfig.ExchangeDurable, false, null);
		}


		/// <summary>
		/// Существует ли очередь сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		public bool QueueExists(string queueName)
		{
			if (string.IsNullOrWhiteSpace(queueName))
			{
				throw new ArgumentNullException("queueName");
			}

			try
			{
				ExecuteAnotherChannel(channel => channel.QueueDeclarePassive(queueName));
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Создать очередь сообщений типа "Fanout".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		public IMessageQueue CreateQueueFanout(QueueConfig queueConfig)
		{
			return CreateQueue(ExchangeType.Fanout, queueConfig, string.Empty, null);
		}

		/// <summary>
		/// Создать очередь сообщений типа "Direct".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingKey">Ключ маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		public IMessageQueue CreateQueueDirect(QueueConfig queueConfig, string routingKey)
		{
			return CreateQueue(ExchangeType.Direct, queueConfig, routingKey, null);
		}

		/// <summary>
		/// Создать очередь сообщений типа "Topic".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingPattern">Шаблон маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		public IMessageQueue CreateQueueTopic(QueueConfig queueConfig, string routingPattern)
		{
			return CreateQueue(ExchangeType.Topic, queueConfig, routingPattern, null);
		}

		/// <summary>
		/// Создать очередь сообщений типа "Headers".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingHeader">Заголовок маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		public IMessageQueue CreateQueueHeaders(QueueConfig queueConfig, MessageHeaders routingHeader)
		{
			return CreateQueue(ExchangeType.Headers, queueConfig, string.Empty, _messageConverter.ConvertMessageHeadersTo(routingHeader));
		}

		/// <summary>
		/// Удалить очередь сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		public void DeleteQueue(string queueName)
		{
			if (string.IsNullOrWhiteSpace(queueName))
			{
				throw new ArgumentNullException("queueName");
			}

			_channel.QueueDelete(queueName);
		}

		private IMessageQueue CreateQueue(string exchangeType, QueueConfig queueConfig, string routingKey, IDictionary<string, object> routingHeaders)
		{
			if (queueConfig == null)
			{
				throw new ArgumentNullException("queueConfig");
			}

			// В момент создания очереди точка обмена уже должна существовать. Если ее нет, то возможны два варианта - менее вероятный и наиболее
			// вероятный. Менее вероятный вариант, когда пытаются создать очередь, не создав предварительно точку обмена. Наиболее вероятный вариант,
			// когда перед созданием очереди сервер RabbitMq был перезапущен, а точка обмена была создана только на время работы сервера RabbitMq.
			// Код ниже проверяет существование точки обмена и, если ее нет, создает точку обмена, которая по типу обмена и времени жизни совпадает
			// с типом обмена и временем жизни создаваемой очереди. Подобное волевое решение основано на высокой вероятности того, что время жизни
			// очереди совпадает с временем жизни точки обмена. Таким образом, обеспечивается высокая устойчивость к перезапускам сервера RabbitMq.

			try
			{
				ExecuteAnotherChannel(channel => channel.ExchangeDeclarePassive(queueConfig.ExchangeName));
			}
			catch
			{
				ExecuteAnotherChannel(channel => channel.ExchangeDeclare(queueConfig.ExchangeName, exchangeType, queueConfig.QueueDurable, false, null));
			}

			_channel.BasicQos((uint)queueConfig.QueuePrefetchSize, (ushort)queueConfig.QueuePrefetchCount, false);
			_channel.QueueDeclare(queueConfig.QueueName, queueConfig.QueueDurable, false, false, null);
			_channel.QueueBind(queueConfig.QueueName, queueConfig.ExchangeName, routingKey ?? string.Empty, routingHeaders);

			var consumer = new QueueingBasicConsumer(_channel);
			_channel.BasicConsume(queueConfig.QueueName, false, queueConfig.QueueConsumerId ?? string.Empty, consumer);

			return new RabbitMqQueue(_channel, consumer.Queue, _messageConverter);
		}


		/// <summary>
		/// Опубликовать сообщение.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена сообщениями.</param>
		/// <param name="routingKey">Ключ маршрутизации сообщения.</param>
		/// <param name="properties">Свойства сообщения.</param>
		/// <param name="body">Тело сообщения.</param>
		public void Publish(string exchangeName, string routingKey, MessageProperties properties, byte[] body)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException("exchangeName");
			}

			_channel.BasicPublish(exchangeName, routingKey ?? string.Empty, _messageConverter.ConvertTo(properties), body);
		}


		public void Dispose()
		{
			if (_channel != null)
			{
				_channel.ExecuteSilent(c => c.Dispose());
				_channel = null;

				_connection.ExecuteSilent(c => c.Dispose());
				_connection = null;
			}
		}
	}
}