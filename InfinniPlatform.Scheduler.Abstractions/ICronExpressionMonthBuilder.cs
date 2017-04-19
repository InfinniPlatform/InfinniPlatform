namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части месяца.
    /// </summary>
    public interface ICronExpressionMonthBuilder
    {
        /// <summary>
        /// Каждый месяц.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionMonthBuilder Every();

        /// <summary>
        /// Каждый указанный месяц.
        /// </summary>
        /// <param name="month">Месяц.</param>
        /// <remarks>
        /// В CRON-выражении 'M', где M - месяц <paramref name="month"/> от 1 (январь) до 12 (декабрь).
        /// Если значение <paramref name="month"/> равно <see cref="Month.January"/>, то событие должно
        /// происходить каждый январь.
        /// </remarks>
        ICronExpressionMonthBuilder Each(Month month);

        /// <summary>
        /// Каждый указанный месяц и через заданный интервал после него.
        /// </summary>
        /// <param name="month">Месяц.</param>
        /// <param name="interval">Интервал в месяцах.</param>
        /// <remarks>
        /// В CRON-выражении 'M/I', где M - месяц <paramref name="month"/> от 1 (январь) до 12 (декабрь), I - интервал в месяцах
        /// <paramref name="interval"/>. Если значение <paramref name="month"/> равно <see cref="Month.January"/>, а значение
        /// <paramref name="interval"/> равно 3, то событие должно происходить в январе, апреле, июле и и октябре.
        /// </remarks>
        ICronExpressionMonthBuilder Each(Month month, int interval);

        /// <summary>
        /// Каждый месяц из указанного списка.
        /// </summary>
        /// <param name="months">Список месяцев.</param>
        /// <remarks>
        /// В CRON-выражении 'M1,M2,M3,...,Mm', где M1, M2, M3, ..., Mn - месяцы списка <paramref name="months"/>. Если значение
        /// <paramref name="months"/> представлено массивом <c>new[] { Month.January, Month.February, Month.March }</c>,
        /// то событие должно происходить в январе, феврале и марте.
        /// </remarks>
        ICronExpressionMonthBuilder EachOfSet(params Month[] months);

        /// <summary>
        /// Каждый месяц из указанного диапазона.
        /// </summary>
        /// <param name="monthFrom">Начало диапазона месяцев.</param>
        /// <param name="monthTo">Конец диапазона месяцев.</param>
        /// <remarks>
        /// В CRON-выражении 'M1-M2', где M1 и M2 - соответственно начало <paramref name="monthFrom"/> и конец <paramref name="monthTo"/>
        /// диапазона месяцев. Если значение <paramref name="monthFrom"/> равно <see cref="Month.January"/>, а значение <paramref name="monthTo"/>
        /// равно <see cref="Month.March"/>, то событие должно происходить в январе, феврале и марте.
        /// </remarks>
        ICronExpressionMonthBuilder EachOfRange(Month monthFrom, Month monthTo);
    }
}