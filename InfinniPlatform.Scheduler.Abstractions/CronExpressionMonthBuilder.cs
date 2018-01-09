using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class CronExpressionMonthBuilder : ICronExpressionMonthBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CronExpressionMonthBuilder"/>.
        /// </summary>
        public CronExpressionMonthBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        /// <inheritdoc />
        public ICronExpressionMonthBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMonthBuilder Each(Month month)
        {
            // Добавляется выражение 'M'.
            _expression = _expression.AppendCronValue(month.ToCronValue());

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMonthBuilder Each(Month month, int interval)
        {
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'M/I'.
            _expression = _expression.AppendCronValue(month.ToCronValue() + CronConstants.ValueInterval + interval);

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMonthBuilder EachOfSet(params Month[] months)
        {
            // Добавляется выражение 'M1,M2,M3,...,Mn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, months.Distinct().Select(i => i.ToCronValue())));

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionMonthBuilder EachOfRange(Month monthFrom, Month monthTo)
        {
            CronConstants.EnsureRange(nameof(monthTo), (int)monthTo, (int)monthFrom, (int)monthTo);

            // Добавляется выражение 'M1-M2'.
            _expression = _expression.AppendCronValue(monthFrom.ToCronValue() + CronConstants.ValueRange + monthTo.ToCronValue());

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