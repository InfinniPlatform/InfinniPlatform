using System.Linq;

namespace InfinniPlatform.Scheduler
{
    public class CronExpressionSecondBuilder : ICronExpressionSecondBuilder
    {
        public CronExpressionSecondBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionSecondBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionSecondBuilder Each(int second)
        {
            CronConstants.EnsureRange(nameof(second), second, CronConstants.SecondMin, CronConstants.SecondMax);

            // Добавляется выражение 'S'.
            _expression = _expression.AppendCronValue(second.ToString());

            return this;
        }

        public ICronExpressionSecondBuilder Each(int second, int interval)
        {
            CronConstants.EnsureRange(nameof(second), second, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'S/I'.
            _expression = _expression.AppendCronValue(second + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionSecondBuilder EachOfSet(params int[] seconds)
        {
            CronConstants.EnsureRange(nameof(seconds), seconds, CronConstants.SecondMin, CronConstants.SecondMax);

            // Добавляется выражение 'S1,S2,S3,...,Sn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, seconds.Distinct()));

            return this;
        }

        public ICronExpressionSecondBuilder EachOfRange(int secondFrom, int secondTo)
        {
            CronConstants.EnsureRange(nameof(secondFrom), secondFrom, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsureRange(nameof(secondTo), secondTo, CronConstants.SecondMin, CronConstants.SecondMax);
            CronConstants.EnsureRange(nameof(secondTo), secondTo, secondFrom, secondTo);

            // Добавляется выражение 'S1-S2'.
            _expression = _expression.AppendCronValue(secondFrom + CronConstants.ValueRange + secondTo);

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}