using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class CronExpressionYearBuilder : ICronExpressionYearBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CronExpressionYearBuilder"/>.
        /// </summary>
        public CronExpressionYearBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        /// <inheritdoc />
        public ICronExpressionYearBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionYearBuilder Each(int year)
        {
            CronConstants.EnsureRange(nameof(year), year, CronConstants.YearMin, CronConstants.YearMax);

            // Добавляется выражение 'Y'.
            _expression = _expression.AppendCronValue(year.ToString());

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionYearBuilder Each(int year, int interval)
        {
            CronConstants.EnsureRange(nameof(year), year, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'Y/I'.
            _expression = _expression.AppendCronValue(year + CronConstants.ValueInterval + interval);

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionYearBuilder EachOfSet(params int[] years)
        {
            CronConstants.EnsureRange(nameof(years), years, CronConstants.YearMin, CronConstants.YearMax);

            // Добавляется выражение 'Y1,Y2,Y3,...,Yn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, years.Distinct()));

            return this;
        }

        /// <inheritdoc />
        public ICronExpressionYearBuilder EachOfRange(int yearFrom, int yearTo)
        {
            CronConstants.EnsureRange(nameof(yearFrom), yearFrom, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsureRange(nameof(yearTo), yearTo, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsureRange(nameof(yearTo), yearTo, yearFrom, yearTo);

            // Добавляется выражение 'Y1-Y2'.
            _expression = _expression.AppendCronValue(yearFrom + CronConstants.ValueRange + yearTo);

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