namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в года.
    /// </summary>
    public interface ICronExpressionYearBuilder
    {
        /// <summary>
        /// Каждую год.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionYearBuilder Every();

        /// <summary>
        /// Каждый указанный год.
        /// </summary>
        /// <param name="year">Год (от 1970 до 2099).</param>
        /// <remarks>
        /// В CRON-выражении 'Y', где Y - год <paramref name="year"/> от 1970 до 2099. Если значение <paramref name="year"/>
        /// равно 2016, то событие должно происходить в 2016 году.
        /// </remarks>
        ICronExpressionYearBuilder Each(int year);

        /// <summary>
        /// Каждый указанный год и через заданный интервал после него.
        /// </summary>
        /// <param name="year">Год (от 1970 до 2099).</param>
        /// <param name="interval">Интервал в годах.</param>
        /// <remarks>
        /// В CRON-выражении 'Y/I', где Y - год <paramref name="year"/> от 1970 до 2099, I - интервал в годах <paramref name="interval"/>.
        /// Если значение <paramref name="year"/> равно 2016, а значение <paramref name="interval"/> равно 10, то событие должно происходить
        /// в 2016, 2026, 2036 и т.д.
        /// </remarks>
        ICronExpressionYearBuilder Each(int year, int interval);

        /// <summary>
        /// Каждый год из указанного списка.
        /// </summary>
        /// <param name="years">Список годов (каждый от 1970 до 2099).</param>
        /// <remarks>
        /// В CRON-выражении 'Y1,Y2,Y3,...,Yn', где Y1, Y2, Y3, ..., Yn - годы списка <paramref name="years"/>. Если значение
        /// <paramref name="years"/> представлено массивом <c>new[] { 2016, 2017, 2018 }</c>, то событие должно происходить
        /// в 2016, 2017 и 2018 году.
        /// </remarks>
        ICronExpressionYearBuilder EachOfSet(params int[] years);

        /// <summary>
        /// Каждый год из указанного диапазона.
        /// </summary>
        /// <param name="yearFrom">Начало диапазона лет (от 1970 до 2099).</param>
        /// <param name="yearTo">Конец диапазона лет (от 1970 до 2099).</param>
        /// <remarks>
        /// В CRON-выражении 'Y1-Y2', где Y1 и Y2 - соответственно начало <paramref name="yearFrom"/> и конец <paramref name="yearTo"/>
        /// диапазона лет. Если значение <paramref name="yearFrom"/> равно 2016, а значение <paramref name="yearTo"/> равно 2018,
        /// то событие должно происходить в 2016, 2017 и 2018 году.
        /// </remarks>
        ICronExpressionYearBuilder EachOfRange(int yearFrom, int yearTo);
    }
}