using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// ѕредоставл€ет набор методов дл€ определени€ времени свершени€ событий в части дн€ недели.
    /// </summary>
    public interface ICronExpressionDayOfWeekBuilder
    {
        /// <summary>
        ///  аждый день недели.
        /// </summary>
        /// <remarks>
        /// ¬ CRON-выражении '*'.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Every();

        /// <summary>
        ///  аждый указанный день недели.
        /// </summary>
        /// <param name="dayOfWeek">ƒень недели.</param>
        /// <remarks>
        /// ¬ CRON-выражении 'D', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота).
        /// ≈сли значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Friday"/>, то событие должно происходить
        /// каждую п€тницу.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek);

        /// <summary>
        ///  аждый указанный день недели и через заданный интервал после него.
        /// </summary>
        /// <param name="dayOfWeek">ƒень недели.</param>
        /// <param name="interval">»нтервал в дн€х.</param>
        /// <remarks>
        /// ¬ CRON-выражении 'D/I', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота), I - интервал в дн€х
        /// <paramref name="interval"/>. ≈сли значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Tuesday"/>, а значение
        /// <paramref name="interval"/> равно 2, то событие должно происходить во вторник, четверг и субботу.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder Each(DayOfWeek dayOfWeek, int interval);

        /// <summary>
        ///  аждый день недели из указанного списка.
        /// </summary>
        /// <param name="daysOfWeek">—писок дней недели.</param>
        /// <remarks>
        /// ¬ CRON-выражении 'D1,D2,D3,...,Dn', где D1, D2, D3, ..., Dn - дни мес€ца списка <paramref name="daysOfWeek"/>. ≈сли значение
        /// <paramref name="daysOfWeek"/> представлено массивом <c>new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday }</c>,
        /// то событие должно происходить в понедельник, вторник и среду.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfSet(params DayOfWeek[] daysOfWeek);

        /// <summary>
        ///  аждый день недели из указанного диапазона.
        /// </summary>
        /// <param name="dayOfWeekFrom">Ќачало диапазона дней недели.</param>
        /// <param name="dayOfWeekTo"> онец диапазона дней недели.</param>
        /// <remarks>
        /// ¬ CRON-выражении 'D1-D2', где D1 и D2 - соответственно начало <paramref name="dayOfWeekFrom"/> и конец <paramref name="dayOfWeekTo"/>
        /// диапазона дней недели. ≈сли значение <paramref name="dayOfWeekFrom"/> равно <see cref="DayOfWeek.Monday"/>, а значение
        /// <paramref name="dayOfWeekTo"/> равно <see cref="DayOfWeek.Wednesday"/>, то событие должно происходить в понедельник,
        /// вторник и среду.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachOfRange(DayOfWeek dayOfWeekFrom, DayOfWeek dayOfWeekTo);

        /// <summary>
        ///  аждый последний указанный день недели в мес€це.
        /// </summary>
        /// <param name="dayOfWeek">ƒень недели.</param>
        /// <remarks>
        /// ¬ CRON-выражении 'DL', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота).
        /// ≈сли значение <paramref name="dayOfWeek"/> равно <see cref="DayOfWeek.Friday"/>, то событие должно
        /// происходить в последнюю п€тницу мес€ца.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachLast(DayOfWeek dayOfWeek);

        /// <summary>
        ///  аждый N-й указанный день недели в мес€це.
        /// </summary>
        /// <param name="dayOfWeek">ƒень недели.</param>
        /// <param name="orderNumber">ѕор€дковый номер дн€ недели в мес€це (начина€ с 1).</param>
        /// <remarks>
        /// ¬ CRON-выражении 'D#n', где D - день недели <paramref name="dayOfWeek"/> от 1 (воскресенье) до 7 (суббота),
        /// n - номер дн€ недели в мес€це <paramref name="orderNumber"/>. ≈сли значение <paramref name="dayOfWeek"/>
        /// равно <see cref="DayOfWeek.Friday"/>, а значение <paramref name="orderNumber"/> равно 1, то событие должно
        /// происходить в первую п€тницу мес€ца.
        /// </remarks>
        ICronExpressionDayOfWeekBuilder EachNth(DayOfWeek dayOfWeek, int orderNumber);
    }
}