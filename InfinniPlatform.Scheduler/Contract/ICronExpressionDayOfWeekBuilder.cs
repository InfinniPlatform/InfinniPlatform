using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части дня недели.
    /// </summary>
    public interface ICronExpressionDayOfWeekBuilder
    {
        /// <summary>
        /// Каждый день недели.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Every();

        /// <summary>
        /// Каждый указанный день недели.
        /// </summary>
        /// <param name="dayOfWeek">День недели.</param>
        /// <remarks>
        /// В CRON-выражении 'D', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота).
        /// Если значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Friday"/>, то событие должно происходить
        /// каждую пятницу.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek);

        /// <summary>
        /// Каждый указанный день недели и через заданный интервал после него.
        /// </summary>
        /// <param name="dayOfWeek">День недели.</param>
        /// <param name="interval">Интервал в днях.</param>
        /// <remarks>
        /// В CRON-выражении 'D/I', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота), I - интервал в днях
        /// <paramref name="interval"/>. Если значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Tuesday"/>, а значение
        /// <paramref name="interval"/> равно 2, то событие должно происходить во вторник, четверг и субботу.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek, int interval);

        /// <summary>
        /// Каждый день недели из указанного списка.
        /// </summary>
        /// <param name="daysOfWeek">Список дней недели.</param>
        /// <remarks>
        /// В CRON-выражении 'D1,D2,D3,...,Dn', где D1, D2, D3, ..., Dn - дни месяца списка <paramref name="daysOfWeek"/>. Если значение
        /// <paramref name="daysOfWeek"/> представлено массивом <c>new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday }</c>,
        /// то событие должно происходить в понедельник, вторник и среду.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfSet(params DayOfWeek[] daysOfWeek);

        /// <summary>
        /// Каждый день недели из указанного диапазона.
        /// </summary>
        /// <param name="dayOfWeekFrom">Начало диапазона дней недели.</param>
        /// <param name="dayOfWeekTo">Конец диапазона дней недели.</param>
        /// <remarks>
        /// В CRON-выражении 'D1-D2', где D1 и D2 - соответственно начало <paramref name="dayOfWeekFrom"/> и конец <paramref name="dayOfWeekTo"/>
        /// диапазона дней недели. Если значение <paramref name="dayOfWeekFrom"/> равно <see cref="DayOfWeek.Monday"/>, а значение
        /// <paramref name="dayOfWeekTo"/> равно <see cref="DayOfWeek.Wednesday"/>, то событие должно происходить в понедельник,
        /// вторник и среду.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfRange(DayOfWeek dayOfWeekFrom, DayOfWeek dayOfWeekTo);

        /// <summary>
        /// Каждый последний указанный день недели в месяце.
        /// </summary>
        /// <param name="dayOfWeek">День недели.</param>
        /// <remarks>
        /// В CRON-выражении 'DL', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота).
        /// Если значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Friday"/>, то событие должно
        /// происходить в последнюю пятницу месяца.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachLast(DayOfWeek dayOfWeek);

        /// <summary>
        /// Каждый N-й указанный день недели в месяце.
        /// </summary>
        /// <param name="dayOfWeek">День недели.</param>
        /// <param name="orderNumber">Порядковый номер дня недели в месяце (начиная с 1).</param>
        /// <remarks>
        /// В CRON-выражении 'D#n', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота),
        /// n - номер дня недели в месяце <paramref name="orderNumber"/>. Если значение <paramref name="dayOfWeek"/>
        /// равно <see cref="DayOfWeek.Friday"/>, а значение <paramref name="orderNumber"/> равно 1, то событие должно
        /// происходить в первую пятницу месяца.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachNth(DayOfWeek dayOfWeek, int orderNumber);
    }
}