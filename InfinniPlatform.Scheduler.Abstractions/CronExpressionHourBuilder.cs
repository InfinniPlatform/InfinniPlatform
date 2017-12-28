using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class CronExpressionHourBuilder : ICronExpressionHourBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CronExpressionHourBuilder"/>.
        /// </summary>
        public CronExpressionHourBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        /// <inheritdoc />
        public ICronExpressionHourBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionHourBuilder Each(int hour)
        {
            CronConstants.EnsureRange(nameof(hour), hour, CronConstants.HourMin, CronConstants.HourMax);

            // Добавляется выражение 'H'.
            _expression = _expression.AppendCronValue(hour.ToString());

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionHourBuilder Each(int hour, int interval)
        {
            CronConstants.EnsureRange(nameof(hour), hour, CronConstants.HourMin, CronConstants.HourMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'H/I'.
            _expression = _expression.AppendCronValue(hour + CronConstants.ValueInterval + interval);

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionHourBuilder EachOfSet(params int[] hours)
        {
            CronConstants.EnsureRange(nameof(hours), hours, CronConstants.HourMin, CronConstants.HourMax);

            // Добавляется выражение 'H1,H2,H3,...,Hn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, hours.Distinct()));

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionHourBuilder EachOfRange(int hourFrom, int hourTo)
        {
            CronConstants.EnsureRange(nameof(hourFrom), hourFrom, CronConstants.HourMin, CronConstants.HourMax);
            CronConstants.EnsureRange(nameof(hourTo), hourTo, CronConstants.HourMin, CronConstants.HourMax);
            CronConstants.EnsureRange(nameof(hourTo), hourTo, hourFrom, hourTo);

            // Добавляется выражение 'H1-H2'.
            _expression = _expression.AppendCronValue(hourFrom + CronConstants.ValueRange + hourTo);

            return this;
        }


        /// <summary>
        /// Returns current expression
        /// </summary>
        public string Build()
        {
            return _expression;
        }
    }
}