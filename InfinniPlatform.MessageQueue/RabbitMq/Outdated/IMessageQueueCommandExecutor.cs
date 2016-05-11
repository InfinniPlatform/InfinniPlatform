using System;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Исполнитель команд к очереди сообщений.
	/// </summary>
	/// <returns>
	/// Исполнитель команд определяет стратегию выполнения команды к очереди сообщений. Например, может реализовывать
	/// стратегию повторного выполнения команды в случае неудачи или стратегию балансировки нагрузки на шину сообщений.
	/// Таким образом, можно обеспечить необходимый уровень устойчивости к возможным системным сбоям.
	/// </returns>
	public interface IMessageQueueCommandExecutor
	{
		/// <summary>
		/// Выполнить команду.
		/// </summary>
		/// <param name="command">Команда.</param>
		void Execute(Action<IMessageQueueSession> command);
	}
}