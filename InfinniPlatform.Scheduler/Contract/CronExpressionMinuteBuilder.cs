using System.Linq;

namespace InfinniPlatform.Scheduler.Contract
{
    internal class CronExpressionMinuteBuilder : ICronExpressionMinuteBuilder
    {
        public CronExpressionMinuteBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionMinuteBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionMinuteBuilder Each(int minute)
        {
            CronConstants.EnsureRange(nameof(minute), minute, CronConstants.MinuteMin, CronConstants.MinuteMax);

            // Добавляется выражение 'M'.
            _expression = _expression.AppendCronValue(minute.ToString());

            return this;
        }

        public ICronExpressionMinuteBuilder Each(int minute, int interval)
        {
            CronConstants.EnsureRange(nameof(minute), minute, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'M/I'.
            _expression = _expression.AppendCronValue(minute + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionMinuteBuilder EachOfSet(params int[] minutes)
        {
            CronConstants.EnsureRange(nameof(minutes), minutes, CronConstants.MinuteMin, CronConstants.MinuteMax);

            // Добавляется выражение 'M1,M2,M3,...,Mn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, minutes.Distinct()));

            return this;
        }

        public ICronExpressionMinuteBuilder EachOfRange(int minuteFrom, int minuteTo)
        {
            CronConstants.EnsureRange(nameof(minuteFrom), minuteFrom, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsureRange(nameof(minuteTo), minuteTo, CronConstants.MinuteMin, CronConstants.MinuteMax);
            CronConstants.EnsureRange(nameof(minuteTo), minuteTo, minuteFrom, minuteTo);

            // Добавляется выражение 'M1-M2'.
            _expression = _expression.AppendCronValue(minuteFrom + CronConstants.ValueRange + minuteTo);

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}