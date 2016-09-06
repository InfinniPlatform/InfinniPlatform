namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет набор методов для определения времени свершения событий в части дня месяца.
    /// </summary>
    public interface ICronExpressionDayOfMonthBuilder
    {
        /// <summary>
        /// Каждый день месяца.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении '*'.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder Every();

        /// <summary>
        /// Каждый указанный день месяца.
        /// </summary>
        /// <param name="dayOfMonth">День месяца (от 1 до 31).</param>
        /// <remarks>
        /// В CRON-выражении 'D', где D - день месяца <paramref name="dayOfMonth"/> от 1 до 31. Если значение <paramref name="dayOfMonth"/>
        /// равно 5, то событие должно происходить 5-го числа каждого месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder Each(int dayOfMonth);

        /// <summary>
        /// Каждый указанный день месяца и через заданный интервал после него.
        /// </summary>
        /// <param name="dayOfMonth">День месяца (от 1 до 31).</param>
        /// <param name="interval">Интервал в днях.</param>
        /// <remarks>
        /// В CRON-выражении 'D/I', где D - день месяца <paramref name="dayOfMonth"/> от 1 до 31, I - интервал в днях <paramref name="interval"/>.
        /// Если значение <paramref name="dayOfMonth"/> равно 5, а значение <paramref name="interval"/> равно 6, то событие должно происходить
        /// 5-го, 11-го, 17-го и 29-го (если допустимо) числа каждого месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder Each(int dayOfMonth, int interval);

        /// <summary>
        /// Каждый день месяца из указанного списка.
        /// </summary>
        /// <param name="daysOfMonth">Список дней месяца (каждый от 1 до 31).</param>
        /// <remarks>
        /// В CRON-выражении 'D1,D2,D3,...,Dn', где D1, D2, D3, ..., Dn - дни месяца списка <paramref name="daysOfMonth"/>. Если значение
        /// <paramref name="daysOfMonth"/> представлено массивом <c>new[] { 10, 11, 12 }</c>, то событие должно происходить 10-го, 11-го
        /// и 12-го числа каждого месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder EachOfSet(params int[] daysOfMonth);

        /// <summary>
        /// Каждый день месяца из указанного диапазона.
        /// </summary>
        /// <param name="dayOfMonthFrom">Начало диапазона дней месяца (от 1 до 31).</param>
        /// <param name="dayOfMonthTo">Конец диапазона дней месяца (от 1 до 31).</param>
        /// <remarks>
        /// В CRON-выражении 'D1-D2', где D1 и D2 - соответственно начало <paramref name="dayOfMonthFrom"/> и конец <paramref name="dayOfMonthTo"/>
        /// диапазона дней месяца. Если значение <paramref name="dayOfMonthFrom"/> равно 10, а значение <paramref name="dayOfMonthTo"/> равно 12,
        /// то событие должно происходить 10-го, 11-го и 12-го числа каждого месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder EachOfRange(int dayOfMonthFrom, int dayOfMonthTo);

        /// <summary>
        /// Каждый последний день месяца.
        /// </summary>
        /// <param name="beforeDays">Количество дней до последнего дня месяца.</param>
        /// <remarks>
        /// В CRON-выражении 'L' или 'L-d', если d больше нуля, где d - количество дней до последнего дня месяца. Если значение
        /// <paramref name="beforeDays"/> равно нулю, то событие должно происходить в последний день каждого месяца. Если значение
        /// <paramref name="beforeDays"/> больше нуля, событие должно происходить за указанное количество дней до последнего дня
        /// каждого месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder EachLast(int beforeDays = 0);

        /// <summary>
        /// Каждый рабочий день (с понедельника по пятницу), наиболее близкий к указанному дню месяца.
        /// </summary>
        /// <param name="dayOfMonth">День месяца (от 1 до 31).</param>
        /// <remarks>
        /// В CRON-выражении 'DW', где D - день месяца <paramref name="dayOfMonth"/> от 1 до 31. Если значение <paramref name="dayOfMonth"/>
        /// равно 15, то событие должно происходить в рабочий день (с понедельника по пятницу), наиболее близкий к 15-му числу. Например,
        /// если 15-е число - это суббота, то событие произойдет 14-го числа в пятницу. Если 15-е число - это воскресенье, то событие
        /// произойдет 16-го числа в понедельник. Если 15-е число - это четверг, то событие произойдет 15-го числа в четверг. Однако,
        /// если значение <paramref name="dayOfMonth"/> равно 1 и 1-е число - это суббота, то событие произойдет 3-го числа в понедельник,
        /// поскольку данное правило работает только в рамках одного месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder EachNearestWeekday(int dayOfMonth);

        /// <summary>
        /// Каждый последний рабочий день месяца.
        /// </summary>
        /// <remarks>
        /// В CRON-выражении 'LW' (last weekday). Событие должно происходить в последний рабочий день месяца.
        /// </remarks>
        ICronExpressionDayOfMonthBuilder EachLastWeekday();
    }
}