using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий.
    /// </summary>
    public interface ICronExpressionBuilder
    {
        /// <summary>
        /// Секунды.
        /// </summary>
        ICronExpressionBuilder Seconds(Action<ICronExpressionSecondBuilder> seconds);

        /// <summary>
        /// Минуты.
        /// </summary>
        ICronExpressionBuilder Minutes(Action<ICronExpressionMinuteBuilder> minutes);

        /// <summary>
        /// Часы.
        /// </summary>
        ICronExpressionBuilder Hours(Action<ICronExpressionHourBuilder> hours);

        /// <summary>
        /// Дни месяца.
        /// </summary>
        ICronExpressionBuilder DayOfMonth(Action<ICronExpressionDayOfMonthBuilder> dayOfMonth);

        /// <summary>
        /// Месяцы.
        /// </summary>
        ICronExpressionBuilder Month(Action<ICronExpressionMonthBuilder> month);

        /// <summary>
        /// Дни недели.
        /// </summary>
        ICronExpressionBuilder DayOfWeek(Action<ICronExpressionDayOfWeekBuilder> dayOfWeek);

        /// <summary>
        /// Годы.
        /// </summary>
        ICronExpressionBuilder Year(Action<ICronExpressionYearBuilder> year);
    }
}