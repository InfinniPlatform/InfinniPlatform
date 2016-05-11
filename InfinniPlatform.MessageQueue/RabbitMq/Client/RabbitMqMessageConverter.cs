using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Client
{
	/// <summary>
	/// Предоставляет методы для получения свойств сообщения из внутреннего формата RabbitMq и наоборот.
	/// </summary>
	sealed class RabbitMqMessageConverter
	{
		public RabbitMqMessageConverter(Func<IBasicProperties> propertiesFactory)
		{
			_propertiesFactory = propertiesFactory;
		}


		private readonly Func<IBasicProperties> _propertiesFactory;


		/// <summary>
		/// Получить свойства сообщений из формата RabbitMq. 
		/// </summary>
		public MessageProperties ConvertFrom(IBasicProperties properties)
		{
			if (properties != null)
			{
				return new MessageProperties
						   {
							   AppId = properties.AppId,
							   UserId = properties.UserId,
							   TypeName = properties.Type,
							   MessageId = properties.MessageId,
							   ContentType = properties.ContentType,
							   ContentEncoding = properties.ContentEncoding,
							   Headers = ConvertMessageHeadersFrom(properties.Headers)
						   };
			}

			return null;
		}

		/// <summary>
		/// Получить заголовок сообщения из формата RabbitMq. 
		/// </summary>
		public MessageHeaders ConvertMessageHeadersFrom(IDictionary<string, object> headers)
		{
			if (headers != null)
			{
				var result = new MessageHeaders();

				foreach (var item in headers)
				{
					var key = item.Key;
					var value = item.Value as byte[];

					if (key != null)
					{
						result[key] = value;
					}
				}

				return result;
			}

			return null;
		}


		/// <summary>
		/// Получить свойства сообщения в формате RabbitMq.
		/// </summary>
		public IBasicProperties ConvertTo(MessageProperties properties)
		{
			var result = _propertiesFactory();

			// Все сообщения отправляются с признаком "persistent". Иначе говоря, принято решение, что отправляющая сторона
			// не может решать, может ли сервер сохранять сообщения или нет. Это целиком и полностью зависит от настроек
			// очереди на стороне сервера. 

			result.DeliveryMode = 2;

			if (properties != null)
			{
				if (string.IsNullOrEmpty(properties.AppId) == false)
				{
					result.AppId = properties.AppId;
				}

				if (string.IsNullOrEmpty(properties.UserId) == false)
				{
					result.UserId = properties.UserId;
				}

				if (string.IsNullOrEmpty(properties.TypeName) == false)
				{
					result.Type = properties.TypeName;
				}

				if (string.IsNullOrEmpty(properties.MessageId) == false)
				{
					result.MessageId = properties.MessageId;
				}

				if (string.IsNullOrEmpty(properties.ContentType) == false)
				{
					result.ContentType = properties.ContentType;
				}

				if (string.IsNullOrEmpty(properties.ContentEncoding) == false)
				{
					result.ContentEncoding = properties.ContentEncoding;
				}

				if (properties.Headers != null)
				{
					result.Headers = ConvertMessageHeadersTo(properties.Headers);
				}
			}

			return result;
		}

		/// <summary>
		/// Получить заголовок сообщения в формате RabbitMq.
		/// </summary>
		public IDictionary<string, object> ConvertMessageHeadersTo(MessageHeaders headers)
		{
			if (headers != null)
			{
				var result = new Dictionary<string, object>();

				foreach (var item in headers)
				{
					result.Add(item.Key, item.Value);
				}

				return result;
			}

			return null;
		}
	}
}