namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части часа.
    /// </summary>
    public interface ICronExpressionHourBuilder
    {
        /// <summary>
        /// Каждый час.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionHourBuilder Every();

        /// <summary>
        /// Каждый указанный час.
        /// </summary>
        /// <param name="hour">Час (от 0 до 23).</param>
        /// <remarks>
        /// В CRON-выражении 'H', где H - час <paramref name="hour"/> от 0 до 23. Если значение <paramref name="hour"/>
        /// равно 5, то событие должно происходить в 5-м часу каждого дня.
        /// </remarks>
        ICronExpressionHourBuilder Each(int hour);

        /// <summary>
        /// Каждый указанный час и через заданный интервал после него.
        /// </summary>
        /// <param name="hour">Минута (от 0 до 23).</param>
        /// <param name="interval">Интервал в часах.</param>
        /// <remarks>
        /// В CRON-выражении 'H/I', где H - час <paramref name="hour"/> от 0 до 23, I - интервал в часах <paramref name="interval"/>.
        /// Если значение <paramref name="hour"/> равно 5, а значение <paramref name="interval"/> равно 6, то событие должно происходить
        /// в 5-м, 11-м, 17-м и 23-м часах каждого дня.
        /// </remarks>
        ICronExpressionHourBuilder Each(int hour, int interval);

        /// <summary>
        /// Каждый час из указанного списка.
        /// </summary>
        /// <param name="hours">Список часов (каждый от 0 до 23).</param>
        /// <remarks>
        /// В CRON-выражении 'H1,H2,H3,...,Hn', где H1, H2, H3, ..., Hn - часы списка <paramref name="hours"/>. Если значение
        /// <paramref name="hours"/> представлено массивом <c>new[] { 10, 11, 12 }</c>, то событие должно происходить в 10-м, 11-м
        /// и 12-м часах каждого дня.
        /// </remarks>
        ICronExpressionHourBuilder EachOfSet(params int[] hours);

        /// <summary>
        /// Каждый час из указанного диапазона.
        /// </summary>
        /// <param name="hourFrom">Начало диапазона часов (от 0 до 23).</param>
        /// <param name="hourTo">Конец диапазона часов (от 0 до 23).</param>
        /// <remarks>
        /// В CRON-выражении 'H1-H2', где H1 и H2 - соответственно начало <paramref name="hourFrom"/> и конец <paramref name="hourTo"/>
        /// диапазона часов. Если значение <paramref name="hourFrom"/> равно 10, а значение <paramref name="hourTo"/> равно 12,
        /// то событие должно происходить в 10-м, 11-м и 12-м часах каждого дня.
        /// </remarks>
        ICronExpressionHourBuilder EachOfRange(int hourFrom, int hourTo);
    }
}