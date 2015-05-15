using System;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.RabbitMq
{
	/// <summary>
	/// Политика подтверждения окончания выполнения действия, при которой подтверждение осуществляется только после успешного выполнения действия.
	/// </summary>
	public sealed class OnlyOnSuccessHandlingAcknowledgePolicy : IAcknowledgePolicy
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
			return false;
		}
	}
}