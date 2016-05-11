using System;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Исполнитель команд к очереди сообщений.
	/// </summary>
	public sealed class MessageQueueCommandExecutor : IMessageQueueCommandExecutor
	{
		public MessageQueueCommandExecutor(ICommandExecutor commandExecutor, IMessageQueueSessionManager sessionManager)
		{
			if (commandExecutor == null)
			{
				throw new ArgumentNullException("commandExecutor");
			}

			if (sessionManager == null)
			{
				throw new ArgumentNullException("sessionManager");
			}

			_commandExecutor = commandExecutor;
			_sessionManager = sessionManager;
		}


		private readonly ICommandExecutor _commandExecutor;
		private readonly IMessageQueueSessionManager _sessionManager;


		/// <summary>
		/// Выполнить команду.
		/// </summary>
		/// <param name="command">Команда.</param>
		public void Execute(Action<IMessageQueueSession> command)
		{
			_commandExecutor.Execute(() =>
										 {
											 var session = _sessionManager.OpenSession();

											 try
											 {
												 command(session);
											 }
											 finally
											 {
												 _sessionManager.CloseSession(session);
											 }
										 });
		}
	}
}