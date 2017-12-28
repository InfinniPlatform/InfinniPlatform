using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class CronExpressionSecondBuilder : ICronExpressionSecondBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CronExpressionSecondBuilder"/>.
        /// </summary>
        public CronExpressionSecondBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        /// <inheritdoc />
        public ICronExpressionSecondBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionSecondBuilder Each(int second)
        {
            CronConstants.EnsureRange(nameof(second), second, CronConstants.SecondMin, CronConstants.SecondMax);

            // Добавляется выражение 'S'.
            _expression = _expression.AppendCronValue(second.ToString());

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionSecondBuilder Each(int second, int interval)
        {
            CronConstants.EnsureRange(nameof(second), second, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'S/I'.
            _expression = _expression.AppendCronValue(second + CronConstants.ValueInterval + interval);

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionSecondBuilder EachOfSet(params int[] seconds)
        {
            CronConstants.EnsureRange(nameof(seconds), seconds, CronConstants.SecondMin, CronConstants.SecondMax);

            // Добавляется выражение 'S1,S2,S3,...,Sn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, seconds.Distinct()));

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionSecondBuilder EachOfRange(int secondFrom, int secondTo)
        {
            CronConstants.EnsureRange(nameof(secondFrom), secondFrom, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsureRange(nameof(secondTo), secondTo, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsureRange(nameof(secondTo), secondTo, secondFrom, secondTo);

            // Добавляется выражение 'S1-S2'.
            _expression = _expression.AppendCronValue(secondFrom + CronConstants.ValueRange + secondTo);

            return this;
        }


        /// <summary>
        /// Returns current expressions.
        /// </summary>
        public string Build()
        {
            return _expression;
        }
    }
}