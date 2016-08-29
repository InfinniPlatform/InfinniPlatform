namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части минуты.
    /// </summary>
    public interface ICronExpressionMinuteBuilder
    {
        /// <summary>
        /// Каждую минуту.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionMinuteBuilder Every();

        /// <summary>
        /// Каждую указанную минуту.
        /// </summary>
        /// <param name="minute">Минута (от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'M', где M - минута <paramref name="minute"/> от 0 до 59. Если значение <paramref name="minute"/>
        /// равно 5, то событие должно происходить на 5-й минуте каждого часа.
        /// </remarks>
        ICronExpressionMinuteBuilder Each(int minute);

        /// <summary>
        /// Каждую указанную минуту и через заданный интервал после нее.
        /// </summary>
        /// <param name="minute">Минута (от 0 до 59).</param>
        /// <param name="interval">Интервал в минутах.</param>
        /// <remarks>
        /// В CRON-выражении 'M/I', где M - минута <paramref name="minute"/> от 0 до 59, I - интервал в минутах <paramref name="interval"/>.
        /// Если значение <paramref name="minute"/> равно 5, а значение <paramref name="interval"/> равно 15, то событие должно происходить
        /// на 5-й, 20-й, 35-й и 50-й минутах каждого часа.
        /// </remarks>
        ICronExpressionMinuteBuilder Each(int minute, int interval);

        /// <summary>
        /// Каждую минуту из указанного списка.
        /// </summary>
        /// <param name="minutes">Список минут (каждая от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'M1,M2,M3,...,Mn', где M1, M2, M3, ..., Mn - минуты списка <paramref name="minutes"/>. Если значение
        /// <paramref name="minutes"/> представлено массивом <c>new[] { 10, 11, 12 }</c>, то событие должно происходить на 10-й, 11-й
        /// и 12-й минутах каждого часа.
        /// </remarks>
        ICronExpressionMinuteBuilder EachOfSet(params int[] minutes);

        /// <summary>
        /// Каждую минуту из указанного диапазона.
        /// </summary>
        /// <param name="minuteFrom">Начало диапазона минут (от 0 до 59).</param>
        /// <param name="minuteTo">Конец диапазона минут (от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'M1-M2', где M1 и M2 - соответственно начало <paramref name="minuteFrom"/> и конец <paramref name="minuteTo"/>
        /// диапазона минут. Если значение <paramref name="minuteFrom"/> равно 10, а значение <paramref name="minuteTo"/> равно 12,
        /// то событие должно происходить на 10-й, 11-й и 12-й минутах каждого часа.
        /// </remarks>
        ICronExpressionMinuteBuilder EachOfRange(int minuteFrom, int minuteTo);
    }
}