using System;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
	/// <summary>
	/// Шина сообщений.
	/// </summary>
	public interface IMessageBus : IDisposable
	{
		/// <summary>
		/// Возвращает точку обмена сообщениями.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена сообщениями.</param>
		IMessageExchange GetExchange(string exchangeName);
	}
}