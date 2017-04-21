using System;
using System.Linq;

namespace InfinniPlatform.Scheduler
{
    public class CronExpressionDayOfWeekBuilder : ICronExpressionDayOfWeekBuilder
    {
        public CronExpressionDayOfWeekBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionDayOfWeekBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek)
        {
            // Добавляется выражение 'D'.
            _expression = _expression.AppendCronValue(dayOfWeek.ToCronValue());

            return this;
        }

        public ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek, int interval)
        {
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'D/I'.
            _expression = _expression.AppendCronValue(dayOfWeek.ToCronValue() + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionDayOfWeekBuilder EachOfSet(params DayOfWeek[] daysOfWeek)
        {
            // Добавляется выражение 'D1,D2,D3,...,Dn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, daysOfWeek.Distinct().Select(i => i.ToCronValue())));

            return this;
        }

        public ICronExpressionDayOfWeekBuilder EachOfRange(DayOfWeek dayOfWeekFrom, DayOfWeek dayOfWeekTo)
        {
            CronConstants.EnsureRange(nameof(dayOfWeekTo), (int)dayOfWeekTo, (int)dayOfWeekFrom, (int)dayOfWeekTo);

            // Добавляется выражение 'D1-D2'.
            _expression = _expression.AppendCronValue(dayOfWeekFrom.ToCronValue() + CronConstants.ValueRange + dayOfWeekTo.ToCronValue());

            return this;
        }

        public ICronExpressionDayOfWeekBuilder EachLast(DayOfWeek dayOfWeek)
        {
            // Выражение 'DL'. При использовании литералов L и W возможно определение только
            // единственной даты, то есть нельзя использовать список или диапазон дат.
            _expression = _expression.AppendCronValue(dayOfWeek.ToCronValue() + CronConstants.LastDay);

            return this;
        }

        public ICronExpressionDayOfWeekBuilder EachNth(DayOfWeek dayOfWeek, int orderNumber)
        {
            CronConstants.EnsureRange(nameof(orderNumber), orderNumber, 1, 5);

            // Добавляется выражение 'D#n'.
            _expression = _expression.AppendCronValue(dayOfWeek.ToCronValue() + CronConstants.OrderNumber + orderNumber);

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}