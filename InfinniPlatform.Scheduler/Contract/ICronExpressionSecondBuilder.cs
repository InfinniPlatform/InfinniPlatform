namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части секунды.
    /// </summary>
    public interface ICronExpressionSecondBuilder
    {
        /// <summary>
        /// Каждую секунду.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionSecondBuilder Every();

        /// <summary>
        /// Каждую указанную секунду.
        /// </summary>
        /// <param name="second">Секунда (от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'S', где S - секунда <paramref name="second"/> от 0 до 59. Если значение <paramref name="second"/>
        /// равно 5, то событие должно происходить на 5-й секунде каждой минуты.
        /// </remarks>
        ICronExpressionSecondBuilder Each(int second);

        /// <summary>
        /// Каждую указанную секунду и через заданный интервал после нее.
        /// </summary>
        /// <param name="second">Секунда (от 0 до 59).</param>
        /// <param name="interval">Интервал в секундах.</param>
        /// <remarks>
        /// В CRON-выражении 'S/I', где S - секунда <paramref name="second"/> от 0 до 59, I - интервал в секундах <paramref name="interval"/>.
        /// Если значение <paramref name="second"/> равно 5, а значение <paramref name="interval"/> равно 15, то событие должно происходить
        /// на 5-й, 20-й, 35-й и 50-й секундах каждой минуты.
        /// </remarks>
        ICronExpressionSecondBuilder Each(int second, int interval);

        /// <summary>
        /// Каждую секунду из указанного списка.
        /// </summary>
        /// <param name="seconds">Список секунд (каждая от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'S1,S2,S3,...,Sn', где S1, S2, S3, ..., Sn - секунды списка <paramref name="seconds"/>. Если значение
        /// <paramref name="seconds"/> представлено массивом <c>new[] { 10, 11, 12 }</c>, то событие должно происходить на 10-й, 11-й
        /// и 12-й секундах каждой минуты.
        /// </remarks>
        ICronExpressionSecondBuilder EachOfSet(params int[] seconds);

        /// <summary>
        /// Каждую секунду из указанного диапазона.
        /// </summary>
        /// <param name="secondFrom">Начало диапазона секунд (от 0 до 59).</param>
        /// <param name="secondTo">Конец диапазона секунд (от 0 до 59).</param>
        /// <remarks>
        /// В CRON-выражении 'S1-S2', где S1 и S2 - соответственно начало <paramref name="secondFrom"/> и конец <paramref name="secondTo"/>
        /// диапазона секунд. Если значение <paramref name="secondFrom"/> равно 10, а значение <paramref name="secondTo"/> равно 12,
        /// то событие должно происходить на 10-й, 11-й и 12-й секундах каждой минуты.
        /// </remarks>
        ICronExpressionSecondBuilder EachOfRange(int secondFrom, int secondTo);
    }
}