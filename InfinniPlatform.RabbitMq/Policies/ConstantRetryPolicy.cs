using System;

using InfinniPlatform.RabbitMq.Properties;

namespace InfinniPlatform.RabbitMq.Policies
{
	/// <summary>
	/// Политика постоянного количества возможных попыток повторного выполнения действия после неудачной попытки.
	/// </summary>
	public sealed class ConstantRetryPolicy : IRetryPolicy
	{
		private readonly int _maxRetries;
		private readonly IRetryDecisionProvider _decisionProvider;


		public ConstantRetryPolicy(IRetryDecisionProvider decisionProvider, int maxRetries)
		{
			if (decisionProvider == null)
			{
				throw new ArgumentNullException("decisionProvider");
			}

			if (maxRetries <= 0)
			{
				throw new ArgumentOutOfRangeException("maxRetries", Resources.RetryCountShouldBePositive);
			}

			_maxRetries = maxRetries;
			_decisionProvider = decisionProvider;
		}


		public IRetryScope NewScope()
		{
			return new ConstantRetryScope(this);
		}


		class ConstantRetryScope : IRetryScope
		{
			private int _retries;
			private readonly ConstantRetryPolicy _policy;


			public ConstantRetryScope(ConstantRetryPolicy policy)
			{
				_policy = policy;
			}


			public RetryDecision OnError(Exception error)
			{
				var decision = _policy._decisionProvider.GetRetryDecision(error);

				if (decision == RetryDecision.Retry)
				{
					if (_retries < _policy._maxRetries)
					{
						++_retries;
					}
					else
					{
						decision = RetryDecision.Rethrow;
					}
				}

				return decision;
			}
		}
	}
}