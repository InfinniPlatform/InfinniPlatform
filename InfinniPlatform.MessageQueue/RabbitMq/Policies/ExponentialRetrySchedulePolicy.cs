using System;

using InfinniPlatform.MessageQueue.Properties;

namespace InfinniPlatform.MessageQueue.RabbitMq.Policies
{
	/// <summary>
	/// Политика экспоненциальной задержки между попытками повторного выполнения действия.
	/// </summary>
	public sealed class ExponentialRetrySchedulePolicy : IRetrySchedulePolicy
	{
		private readonly int _baseDelayMs;
		private readonly int _maxDelayMs;
		private readonly int _maxAttempts;


		public ExponentialRetrySchedulePolicy(int baseDelayMs, int maxDelayMs)
		{
			if (baseDelayMs <= 0)
			{
				throw new ArgumentOutOfRangeException("baseDelayMs", Resources.ReconnectionDelayShouldBePositive);
			}

			if (maxDelayMs <= 0)
			{
				throw new ArgumentOutOfRangeException("maxDelayMs", Resources.ReconnectionDelayShouldBePositive);
			}

			if (maxDelayMs < baseDelayMs)
			{
				throw new ArgumentOutOfRangeException("maxDelayMs", Resources.ReconnectionMaxDelayShouldBeGreaterThanBaseDelay);
			}

			_baseDelayMs = baseDelayMs;
			_maxDelayMs = maxDelayMs;

			_maxAttempts = CalculateMaxAttempts(baseDelayMs);
		}


		private static int CalculateMaxAttempts(int baseDelayMs)
		{
			var num = ((baseDelayMs & (baseDelayMs - 1)) == 0) ? 0 : 1;

			return (32 - LeadingZeros(int.MaxValue / baseDelayMs) - num);
		}

		private static int LeadingZeros(int value)
		{
			var num = 0;

			while (value != 0)
			{
				value = value >> 1;
				num++;
			}

			return (32 - num);
		}


		public IRetrySchedule NewSchedule()
		{
			return new ExponentialSchedule(this);
		}


		class ExponentialSchedule : IRetrySchedule
		{
			private int _attempts;
			private readonly ExponentialRetrySchedulePolicy _policy;


			public ExponentialSchedule(ExponentialRetrySchedulePolicy policy)
			{
				_policy = policy;
			}


			public int NextDelayMs()
			{
				if (_attempts >= _policy._maxAttempts)
				{
					return _policy._maxDelayMs;
				}

				return Math.Min(_policy._baseDelayMs * (1 << _attempts++), _policy._maxDelayMs);
			}
		}
	}
}