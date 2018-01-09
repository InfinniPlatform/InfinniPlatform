using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class CronExpressionMinuteBuilder : ICronExpressionMinuteBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CronExpressionMinuteBuilder"/>.
        /// </summary>
        public CronExpressionMinuteBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        /// <inheritdoc />
        public ICronExpressionMinuteBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMinuteBuilder Each(int minute)
        {
            CronConstants.EnsureRange(nameof(minute), minute, CronConstants.MinuteMin, CronConstants.MinuteMax);

            // Добавляется выражение 'M'.
            _expression = _expression.AppendCronValue(minute.ToString());

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMinuteBuilder Each(int minute, int interval)
        {
            CronConstants.EnsureRange(nameof(minute), minute, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'M/I'.
            _expression = _expression.AppendCronValue(minute + CronConstants.ValueInterval + interval);

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMinuteBuilder EachOfSet(params int[] minutes)
        {
            CronConstants.EnsureRange(nameof(minutes), minutes, CronConstants.MinuteMin, CronConstants.MinuteMax);

            // Добавляется выражение 'M1,M2,M3,...,Mn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, minutes.Distinct()));

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMinuteBuilder EachOfRange(int minuteFrom, int minuteTo)
        {
            CronConstants.EnsureRange(nameof(minuteFrom), minuteFrom, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsureRange(nameof(minuteTo), minuteTo, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsureRange(nameof(minuteTo), minuteTo, minuteFrom, minuteTo);

            // Добавляется выражение 'M1-M2'.
            _expression = _expression.AppendCronValue(minuteFrom + CronConstants.ValueRange + minuteTo);

            return this;
        }


        /// <summary>
        /// Returns current expression.
        /// </summary>
        public string Build()
        {
            return _expression;
        }
    }
}