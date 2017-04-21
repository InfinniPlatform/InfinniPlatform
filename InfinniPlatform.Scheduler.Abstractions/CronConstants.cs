using System;
using System.Linq;
using System.Text;

using InfinniPlatform.Scheduler.Properties;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Предоставляет набор констант и методов для CRON-выражений.
    /// </summary>
    public static class CronConstants
    {
        /// <summary>
        /// Знак разделителя частей выражения.
        /// </summary>
        public const string PartDelimiter = " ";

        /// <summary>
        /// Знак любого допустимого значения.
        /// </summary>
        public const string AllValues = "*";

        /// <summary>
        /// Знак отсутствия определенного значения.
        /// </summary>
        public const string NoSpecificValue = "?";

        /// <summary>
        /// Знак разделителя значений.
        /// </summary>
        public const string ValueDelimiter = ",";

        /// <summary>
        /// Знак интервала значений.
        /// </summary>
        public const string ValueInterval = "/";

        /// <summary>
        /// Знак диапазона значений.
        /// </summary>
        public const string ValueRange = "-";

        /// <summary>
        /// Знак порядкового номер дня недели в месяце.
        /// </summary>
        public const string OrderNumber = "#";

        /// <summary>
        /// Знак минуса для указания количества дней до последнего дня месяца.
        /// </summary>
        public const string Minus = "-";

        /// <summary>
        /// Знак последнего дня месяца.
        /// </summary>
        public const string LastDay = "L";

        /// <summary>
        /// Знак рабочего дня недели (с понедельника по пятницу).
        /// </summary>
        public const string Weekday = "W";

        /// <summary>
        /// Знак последнего рабочего дня месяца (с понедельника по пятницу).
        /// </summary>
        public const string LastWeekday = LastDay + Weekday;


        /// <summary>
        /// Минимально возможное значение для секунды.
        /// </summary>
        public const int SecondMin = 0;

        /// <summary>
        /// Максимально возможное значение для секунды.
        /// </summary>
        public const int SecondMax = 59;


        /// <summary>
        /// Минимально возможное значение для минуты.
        /// </summary>
        public const int MinuteMin = 0;

        /// <summary>
        /// Максимально возможное значение для минуты.
        /// </summary>
        public const int MinuteMax = 59;


        /// <summary>
        /// Минимально возможное значение для часа.
        /// </summary>
        public const int HourMin = 0;

        /// <summary>
        /// Максимально возможное значение для часа.
        /// </summary>
        public const int HourMax = 23;


        /// <summary>
        /// Минимально возможное значение для дня месяца.
        /// </summary>
        public const int DayOfMonthMin = 1;

        /// <summary>
        /// Максимально возможное значение для дня месяца.
        /// </summary>
        public const int DayOfMonthMax = 31;


        /// <summary>
        /// Минимально возможное значение для номера дня недели.
        /// </summary>
        public const int DayOfWeekMin = 1;

        /// <summary>
        /// Максимально возможное значение для номера дня недели.
        /// </summary>
        public const int DayOfWeekMax = 7;


        /// <summary>
        /// Минимально возможное значение для номера месяца.
        /// </summary>
        public const int MonthMin = 1;

        /// <summary>
        /// Максимально возможное значение для номера месяца.
        /// </summary>
        public const int MonthMax = 12;


        /// <summary>
        /// Минимально возможное значение для года.
        /// </summary>
        public const int YearMin = 1970;

        /// <summary>
        /// Максимально возможное значение для года.
        /// </summary>
        public const int YearMax = 2099;


        /// <summary>
        /// Проверяет, что указанное значение входит в допустимый диапазон.
        /// </summary>
        /// <param name="name">Имя аргумента.</param>
        /// <param name="value">Значение.</param>
        /// <param name="valueMin">Минимально возможное значение.</param>
        /// <param name="valueMax">Максимально возможное значение.</param>
        /// <exception cref="ArgumentOutOfRangeException">Значение <paramref name="value"/> не входит в допустимый диапазон.</exception>
        public static void EnsureRange(string name, int value, int valueMin, int valueMax)
        {
            if (value < valueMin || value > valueMax)
            {
                throw new ArgumentOutOfRangeException(name, string.Format(Resources.ValueMustBeWithinTheRange, name, valueMin, valueMax));
            }
        }

        /// <summary>
        /// Проверяет, что значение элементов указанного массива входят в допустимый диапазон.
        /// </summary>
        /// <param name="name">Имя аргумента.</param>
        /// <param name="values">Массив значений.</param>
        /// <param name="valueMin">Минимально возможное значение.</param>
        /// <param name="valueMax">Максимально возможное значение.</param>
        /// <exception cref="ArgumentNullException">Значение <paramref name="values"/> равно <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Массив <paramref name="values"/> не содержит ни одного элемента.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значение одного из элементов массива <paramref name="values"/> не входит в допустимый диапазон.</exception>
        public static void EnsureRange(string name, int[] values, int valueMin, int valueMax)
        {
            if (values == null)
            {
                throw new ArgumentNullException(name, string.Format(Resources.ArrayCannotBeNullOrEmpty, name));
            }

            if (values.Length <= 0)
            {
                throw new ArgumentException(string.Format(Resources.ArrayCannotBeNullOrEmpty, name), name);
            }

            if (values.Any(second => second < valueMin || second > valueMax))
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.ArrayItemMustBeWithinTheRange, name, valueMin, valueMax));
            }
        }

        /// <summary>
        /// Проверяет, что указанное значение является положительным числом.
        /// </summary>
        /// <param name="name">Имя аргумента.</param>
        /// <param name="value">Значение.</param>
        /// <exception cref="ArgumentOutOfRangeException">Значение <paramref name="value"/> меньше или равно 0.</exception>
        public static void EnsurePositive(string name, int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Проверяет, что указанное значение является неотрицательным числом.
        /// </summary>
        /// <param name="name">Имя аргумента.</param>
        /// <param name="value">Значение.</param>
        /// <exception cref="ArgumentOutOfRangeException">Значение <paramref name="value"/> меньше 0.</exception>
        public static void EnsureNonNegative(string name, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }


        /// <summary>
        /// Возвращает день недели для CRON-выражения.
        /// </summary>
        /// <param name="dayOfWeek">День недели.</param>
        /// <returns>День недели для CRON-выражения.</returns>
        public static string ToCronValue(this DayOfWeek dayOfWeek)
        {
            return ((int)dayOfWeek + 1).ToString();
        }

        /// <summary>
        /// Возвращает месяц для CRON-выражения.
        /// </summary>
        /// <param name="month">Месяц.</param>
        /// <returns>Месяц для CRON-выражения.</returns>
        public static string ToCronValue(this Month month)
        {
            return ((int)month).ToString();
        }


        /// <summary>
        /// Добавляет дополнительное CRON-выражение для значения.
        /// </summary>
        /// <param name="valueExpression">CRON-выражение для значения.</param>
        /// <param name="valueSubExpression">Дополнительное CRON-выражение для значения.</param>
        /// <returns>Новое CRON-выражение для значения.</returns>
        public static string AppendCronValue(this string valueExpression, string valueSubExpression)
        {
            if (valueExpression == AllValues)
            {
                valueExpression = valueSubExpression;
            }
            else
            {
                valueExpression += ValueDelimiter + valueSubExpression;
            }

            return valueExpression;
        }

        /// <summary>
        /// Добавляет дополнительное CRON-выражение.
        /// </summary>
        /// <param name="expression">CRON-выражение.</param>
        /// <param name="subExpression">Дополнительное CRON-выражение.</param>
        /// <returns>Новое CRON-выражение.</returns>
        public static StringBuilder AppendCronExpression(this StringBuilder expression, string subExpression)
        {
            expression.Append(string.IsNullOrEmpty(subExpression) ? AllValues : subExpression);

            return expression;
        }
    }
}