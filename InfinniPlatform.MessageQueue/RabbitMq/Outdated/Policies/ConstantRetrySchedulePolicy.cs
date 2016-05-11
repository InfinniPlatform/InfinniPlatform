using System;

using InfinniPlatform.MessageQueue.Properties;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies
{
	/// <summary>
	/// Политика постоянной задержки между попытками повторного выполнения действия.
	/// </summary>
	public sealed class ConstantRetrySchedulePolicy : IRetrySchedulePolicy
	{
		private readonly int _delayMs;


		public ConstantRetrySchedulePolicy(int constantDelayMs)
		{
			if (constantDelayMs <= 0L)
			{
				throw new ArgumentOutOfRangeException("constantDelayMs", Resources.ReconnectionDelayShouldBePositive);
			}

			_delayMs = constantDelayMs;
		}


		public IRetrySchedule NewSchedule()
		{
			return new ConstantSchedule(this);
		}


		class ConstantSchedule : IRetrySchedule
		{
			private readonly ConstantRetrySchedulePolicy _policy;


			public ConstantSchedule(ConstantRetrySchedulePolicy policy)
			{
				_policy = policy;
			}


			public int NextDelayMs()
			{
				return _policy._delayMs;
			}
		}
	}
}