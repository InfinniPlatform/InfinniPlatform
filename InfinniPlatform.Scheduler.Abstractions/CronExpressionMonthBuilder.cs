using System.Linq;

namespace InfinniPlatform.Scheduler
{
    public class CronExpressionMonthBuilder : ICronExpressionMonthBuilder
    {
        public CronExpressionMonthBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionMonthBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionMonthBuilder Each(Month month)
        {
            // Добавляется выражение 'M'.
            _expression = _expression.AppendCronValue(month.ToCronValue());

            return this;
        }

        public ICronExpressionMonthBuilder Each(Month month, int interval)
        {
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'M/I'.
            _expression = _expression.AppendCronValue(month.ToCronValue() + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionMonthBuilder EachOfSet(params Month[] months)
        {
            // Добавляется выражение 'M1,M2,M3,...,Mn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, months.Distinct().Select(i => i.ToCronValue())));

            return this;
        }

        public ICronExpressionMonthBuilder EachOfRange(Month monthFrom, Month monthTo)
        {
            CronConstants.EnsureRange(nameof(monthTo), (int)monthTo, (int)monthFrom, (int)monthTo);

            // Добавляется выражение 'M1-M2'.
            _expression = _expression.AppendCronValue(monthFrom.ToCronValue() + CronConstants.ValueRange + monthTo.ToCronValue());

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}