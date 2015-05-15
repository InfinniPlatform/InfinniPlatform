using System;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.RabbitMq
{
	/// <summary>
	/// Свойства точки обмена сообщениями.
	/// </summary>
	public sealed class ExchangeConfig : IExchangeConfig
	{
		public ExchangeConfig(string exchangeName)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException("exchangeName");
			}

			ExchangeName = exchangeName;
		}


		/// <summary>
		/// Наименование точки обмена.
		/// </summary>
		public string ExchangeName { get; private set; }

		/// <summary>
		/// Сохранять настройки точки обмена на диске.
		/// </summary>
		public bool ExchangeDurable { get; set; }


		IExchangeConfig IExchangeConfig.Durable()
		{
			ExchangeDurable = true;

			return this;
		}
	}
}