using System;
using System.Text;

namespace InfinniPlatform.Scheduler
{
    public class CronExpressionBuilder : ICronExpressionBuilder
    {
        private string _seconds;
        private string _minutes;
        private string _hours;
        private string _dayOfMonth;
        private string _month;
        private string _dayOfWeek;
        private string _year;


        public ICronExpressionBuilder Seconds(Action<ICronExpressionSecondBuilder> seconds)
        {
            var builder = new CronExpressionSecondBuilder();

            seconds?.Invoke(builder);

            _seconds = builder.Build();

            return this;
        }

        public ICronExpressionBuilder Minutes(Action<ICronExpressionMinuteBuilder> minutes)
        {
            var builder = new CronExpressionMinuteBuilder();

            minutes?.Invoke(builder);

            _minutes = builder.Build();

            return this;
        }

        public ICronExpressionBuilder Hours(Action<ICronExpressionHourBuilder> hours)
        {
            var builder = new CronExpressionHourBuilder();

            hours?.Invoke(builder);

            _hours = builder.Build();

            return this;
        }

        public ICronExpressionBuilder DayOfMonth(Action<ICronExpressionDayOfMonthBuilder> dayOfMonth)
        {
            var builder = new CronExpressionDayOfMonthBuilder();

            dayOfMonth?.Invoke(builder);

            _dayOfMonth = builder.Build();

            return this;
        }

        public ICronExpressionBuilder Month(Action<ICronExpressionMonthBuilder> month)
        {
            var builder = new CronExpressionMonthBuilder();

            month?.Invoke(builder);

            _month = builder.Build();

            return this;
        }

        public ICronExpressionBuilder DayOfWeek(Action<ICronExpressionDayOfWeekBuilder> dayOfWeek)
        {
            var builder = new CronExpressionDayOfWeekBuilder();

            dayOfWeek?.Invoke(builder);

            _dayOfWeek = builder.Build();

            return this;
        }

        public ICronExpressionBuilder Year(Action<ICronExpressionYearBuilder> year)
        {
            var builder = new CronExpressionYearBuilder();

            year?.Invoke(builder);

            _year = builder.Build();

            return this;
        }


        public string Build()
        {
            var expression = new StringBuilder();

            // Время: секунды, минуты, часы
            expression.AppendCronExpression(_seconds).Append(CronConstants.PartDelimiter);
            expression.AppendCronExpression(_minutes).Append(CronConstants.PartDelimiter);
            expression.AppendCronExpression(_hours).Append(CronConstants.PartDelimiter);

            // День: день месяца, месяц, день недели

            string dayOfMonth;
            string dayOfWeek;

            var anyDayOfMonth = string.IsNullOrEmpty(_dayOfMonth) || (_dayOfMonth == CronConstants.AllValues) || (_dayOfMonth == CronConstants.NoSpecificValue);
            var anyDayOfWeek = string.IsNullOrEmpty(_dayOfWeek) || (_dayOfWeek == CronConstants.AllValues) || (_dayOfWeek == CronConstants.NoSpecificValue);

            if (anyDayOfMonth && anyDayOfWeek)
            {
                dayOfMonth = CronConstants.AllValues;
                dayOfWeek = CronConstants.NoSpecificValue;
            }
            else if (anyDayOfMonth)
            {
                dayOfMonth = CronConstants.NoSpecificValue;
                dayOfWeek = _dayOfWeek;
            }
            else if (anyDayOfWeek)
            {
                dayOfMonth = _dayOfMonth;
                dayOfWeek = CronConstants.NoSpecificValue;
            }
            else
            {
                dayOfMonth = _dayOfMonth;
                dayOfWeek = _dayOfWeek;
            }

            expression.AppendCronExpression(dayOfMonth).Append(CronConstants.PartDelimiter);
            expression.AppendCronExpression(_month).Append(CronConstants.PartDelimiter);
            expression.AppendCronExpression(dayOfWeek);

            // Год (опционально)

            if (!string.IsNullOrEmpty(_year))
            {
                expression.Append(CronConstants.PartDelimiter).Append(_year);
            }

            return expression.ToString();
        }
    }
}