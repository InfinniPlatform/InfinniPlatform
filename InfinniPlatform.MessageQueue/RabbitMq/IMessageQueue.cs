﻿using System;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Очередь сообщений.
	/// </summary>
	public interface IMessageQueue : IDisposable
	{
		/// <summary>
		/// Получить очередное сообщение.
		/// </summary>
		/// <returns>Сообщение очереди.</returns>
		Message Dequeue();

		/// <summary>
		/// Подтвердить окончание обработки сообщения.
		/// </summary>
		/// <param name="message">Сообщение очереди.</param>
		/// <remarks>
		/// Подтверждение окончания обработки сообщения сигнализирует серверу очереди сообщений о том, что сообщение обработано и может быть
		/// удалено из очереди. Если сообщение было прочитано, но подтверждения об окончании не поступило, оно остается на сервере до тех пор,
		/// пока не поступит сигнал подтверждения. Это может обеспечить определенную устойчивость к сбоям, которые могут возникнуть на стороне
		/// потребителя. В свою очередь отправитель сообщения может получить уведомление о том, что оно было получено и обработано.
		/// </remarks>
		void Acknowledge(Message message);

		/// <summary>
		/// Отказаться от обработки сообщения.
		/// </summary>
		/// <param name="message">Сообщение очереди.</param>
		/// <remarks>
		/// Отказ от обработки сообщения может использоваться в случае, когда потребитель не готов по каким-либо причинам обработать данное
		/// сообщение. В этом случае сервер очереди сообщений попытается отправить это сообщение другому потребителю. И так до тех пор, пока 
		/// сообщение не будет обработано и серверу не поступит сигнал подтверждения. Однако это абсолютно не гарантирует того, что потребитель,
		/// который когда-либо отказался от обработки данного сообщения, никогда не получит его снова. Механизм отказа от обработки сообщения
		/// ни в коем случае не следует использовать для отсеивания ненужных сообщений. Например, отказ может произойти по причине слишком
		/// большой загруженности узла, на котором осуществляется обработка.
		/// </remarks>
		void Reject(Message message);
	}
}