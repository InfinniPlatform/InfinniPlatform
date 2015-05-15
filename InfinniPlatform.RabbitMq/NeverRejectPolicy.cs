using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.RabbitMq
{
	/// <summary>
	/// Политика подтверждения отказа от выполнения действия, при которой действие выполняется всегда.
	/// </summary>
	public sealed class NeverRejectPolicy : IRejectPolicy
	{
		public bool MustReject()
		{
			return false;
		}
	}
}