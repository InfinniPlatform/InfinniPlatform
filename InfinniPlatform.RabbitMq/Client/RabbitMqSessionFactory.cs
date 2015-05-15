using InfinniPlatform.Api.Settings;


using RabbitMQ.Client;

namespace InfinniPlatform.RabbitMq.Client
{
	/// <summary>
	/// Фабрика сессий очереди сообщений RabbitMq.
	/// </summary>
	public sealed class RabbitMqSessionFactory : IMessageQueueSessionFactory
	{
		public RabbitMqSessionFactory()
		{
			_port = AppSettings.GetValue(PortConfig, AmqpTcpEndpoint.UseDefaultPort);
			_host = AppSettings.GetValue(HostConfig, "localhost");
		}


		public const string PortConfig = "RabbitMqPort";
		public const string HostConfig = "RabbitMqHost";


		private readonly int _port;
		private readonly string _host;


		/// <summary>
		/// Открыть сессию.
		/// </summary>
		public IMessageQueueSession OpenSession()
		{
			return new RabbitMqSession(_host, _port);
		}
	}
}