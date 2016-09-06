﻿using System.Linq;

namespace InfinniPlatform.Scheduler.Contract
{
    internal class CronExpressionDayOfMonthBuilder : ICronExpressionDayOfMonthBuilder
    {
        public CronExpressionDayOfMonthBuilder()
        {
            _expression = CronConstants.AllValues;
        }


        private string _expression;


        public ICronExpressionDayOfMonthBuilder Every()
        {
            // Выражение '*'.
            _expression = CronConstants.AllValues;

            return this;
        }

        public ICronExpressionDayOfMonthBuilder Each(int dayOfMonth)
        {
            CronConstants.EnsureRange(nameof(dayOfMonth), dayOfMonth, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);

            // Добавляется выражение 'D'.
            _expression = _expression.AppendCronValue(dayOfMonth.ToString());

            return this;
        }

        public ICronExpressionDayOfMonthBuilder Each(int dayOfMonth, int interval)
        {
            CronConstants.EnsureRange(nameof(dayOfMonth), dayOfMonth, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);
            CronConstants.EnsurePositive(nameof(interval), interval);

            // Добавляется выражение 'D/I'.
            _expression = _expression.AppendCronValue(dayOfMonth + CronConstants.ValueInterval + interval);

            return this;
        }

        public ICronExpressionDayOfMonthBuilder EachOfSet(params int[] daysOfMonth)
        {
            CronConstants.EnsureRange(nameof(daysOfMonth), daysOfMonth, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);

            // Добавляется выражение 'D1,D2,D3,...,Dn'.
            _expression = _expression.AppendCronValue(string.Join(CronConstants.ValueDelimiter, daysOfMonth.Distinct()));

            return this;
        }

        public ICronExpressionDayOfMonthBuilder EachOfRange(int dayOfMonthFrom, int dayOfMonthTo)
        {
            CronConstants.EnsureRange(nameof(dayOfMonthFrom), dayOfMonthFrom, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);
            CronConstants.EnsureRange(nameof(dayOfMonthTo), dayOfMonthTo, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);
            CronConstants.EnsureRange(nameof(dayOfMonthTo), dayOfMonthTo, dayOfMonthFrom, dayOfMonthTo);

            // Добавляется выражение 'D1-D2'.
            _expression = _expression.AppendCronValue(dayOfMonthFrom + CronConstants.ValueRange + dayOfMonthTo);

            return this;
        }

        public ICronExpressionDayOfMonthBuilder EachLast(int beforeDays = 0)
        {
            CronConstants.EnsureNonNegative(nameof(beforeDays), beforeDays);

            // Выражение 'L' или 'L-d'. При использовании литералов L и W возможно определение только
            // единственной даты, то есть нельзя использовать список или диапазон дат.
            _expression = (beforeDays == 0) ? CronConstants.LastDay : CronConstants.LastDay + CronConstants.Minus + beforeDays;

            return this;
        }

        public ICronExpressionDayOfMonthBuilder EachNearestWeekday(int dayOfMonth)
        {
            CronConstants.EnsureRange(nameof(dayOfMonth), dayOfMonth, CronConstants.DayOfMonthMin, CronConstants.DayOfMonthMax);

            // Выражение 'DW'. При использовании литералов L и W возможно определение только
            // единственной даты, то есть нельзя использовать список или диапазон дат.
            _expression = dayOfMonth + CronConstants.Weekday;

            return this;
        }

        public ICronExpressionDayOfMonthBuilder EachLastWeekday()
        {
            // Выражение 'LW'. При использовании литералов L и W возможно определение только
            // единственной даты, то есть нельзя использовать список или диапазон дат.
            _expression = CronConstants.LastWeekday;

            return this;
        }


        public string Build()
        {
            return _expression;
        }
    }
}