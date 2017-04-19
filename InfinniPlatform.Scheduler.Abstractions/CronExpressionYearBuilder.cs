using System.Linq;

namespace InfinniPlatform.Scheduler
{
    public class CronExpressionYearBuilder : ICronExpressionYearBuilder
    {
        public CronExpressionYearBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionYearBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionYearBuilder Each(int year)
        {
            CronConstants.EnsureRange(nameof(year), year, CronConstants.YearMin, CronConstants.YearMax);

            // Добавляется выражение 'Y'.
            _expression = _expression.AppendCronValue(year.ToString());

            return this;
        }

        public ICronExpressionYearBuilder Each(int year, int interval)
        {
            CronConstants.EnsureRange(nameof(year), year, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'Y/I'.
            _expression = _expression.AppendCronValue(year + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionYearBuilder EachOfSet(params int[] years)
        {
            CronConstants.EnsureRange(nameof(years), years, CronConstants.YearMin, CronConstants.YearMax);

            // Добавляется выражение 'Y1,Y2,Y3,...,Yn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, years.Distinct()));

            return this;
        }

        public ICronExpressionYearBuilder EachOfRange(int yearFrom, int yearTo)
        {
            CronConstants.EnsureRange(nameof(yearFrom), yearFrom, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsureRange(nameof(yearTo), yearTo, CronConstants.YearMin, CronConstants.YearMax);
            CronConstants.EnsureRange(nameof(yearTo), yearTo, yearFrom, yearTo);

            // Добавляется выражение 'Y1-Y2'.
            _expression = _expression.AppendCronValue(yearFrom + CronConstants.ValueRange + yearTo);

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}