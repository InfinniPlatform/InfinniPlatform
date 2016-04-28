using System;

using InfinniPlatform.Core.MessageQueue;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Политика подтверждения окончания выполнения действия, при которой подтверждение осуществляется после выполнения действия, вне зависимости от результата выполнения.
	/// </summary>
	public sealed class AlwaysAfterHandlingAcknowledgePolicy : IAcknowledgePolicy
	{
		public bool OnBefore()
		{
			return false;
		}

		public bool OnSuccess()
		{
			return true;
		}

		public bool OnFailure(Exception error)
		{
			return true;
		}
	}
}