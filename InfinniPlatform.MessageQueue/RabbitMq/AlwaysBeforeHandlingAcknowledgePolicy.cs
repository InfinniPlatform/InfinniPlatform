using System;

using InfinniPlatform.Core.MessageQueue;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Политика подтверждения окончания выполнения действия, при которой подтверждение осуществляется перед выполнением действия.
	/// </summary>
	public sealed class AlwaysBeforeHandlingAcknowledgePolicy : IAcknowledgePolicy
	{
		public bool OnBefore()
		{
			return true;
		}

		public bool OnSuccess()
		{
			return false;
		}

		public bool OnFailure(Exception error)
		{
			return false;
		}
	}
}