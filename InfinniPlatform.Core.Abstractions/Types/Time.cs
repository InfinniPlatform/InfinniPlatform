using System;

namespace InfinniPlatform.Core.Types
{
    /// <summary>
    /// Время.
    /// </summary>
    /// <remarks>
    /// Определяет интервал времени. На уровне реализации интервал времени хранится в виде дробного числа типа <see cref="double"/>,
    /// содержащего количество секунд <see cref="TotalSeconds"/>. Поддерживает операции приведения и сравнения, в том числе c такими
    /// типами, как <see cref="double"/> и <see cref="TimeSpan"/>. Также предоставляет интерфейс для совершения арифметических операций -
    /// сложения <see cref="op_Addition(Time,Time)"/> и вычитания <see cref="op_Subtraction(Time,Time)"/>, в том числе с такими типами,
    /// как <see cref="double"/>, <see cref="TimeSpan"/>, <see cref="DateTime"/> и <see cref="DateTimeOffset"/>. Предназначен для
    /// использования в сценариях, в которых нужно работать только со временем или с интервалом времени.
    /// </remarks>
    [Serializable]
    public struct Time : IComparable, IComparable<Time>, IEquatable<Time>, IFormattable
    {
        private const double MillisecondsPerSecond = 1000d;
        private const double SecondsPerMillisecond = 0.001d;
        private const double SecondsPerMinute = 60d;
        private const double SecondsPerHour = SecondsPerMinute * 60d;
        private const double SecondsPerDay = SecondsPerHour * 24d;


        /// <summary>
        /// Нулевое время.
        /// </summary>
        public static readonly Time Zero = new Time(0);

        /// <summary>
        /// Текущее время в локальной временной зоне.
        /// </summary>
        public static Time Now => FromDateTime(DateTime.Now);

        /// <summary>
        /// Текущее время в нулевой временной зоне.
        /// </summary>
        public static Time UtcNow => FromDateTime(DateTime.UtcNow);


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="totalSeconds">Время, выраженное в секундах.</param>
        public Time(double totalSeconds)
        {
            TotalSeconds = totalSeconds;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="hours">Количество часов.</param>
        /// <param name="minutes">Количество минут.</param>
        /// <param name="seconds">Количество секунд.</param>
        public Time(int hours, int minutes, int seconds)
        {
            TotalSeconds = hours * SecondsPerHour + minutes * SecondsPerMinute + seconds;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="days">Количество дней.</param>
        /// <param name="hours">Количество часов.</param>
        /// <param name="minutes">Количество минут.</param>
        /// <param name="seconds">Количество секунд.</param>
        public Time(int days, int hours, int minutes, int seconds)
        {
            TotalSeconds = days * SecondsPerDay + hours * SecondsPerHour + minutes * SecondsPerMinute + seconds;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="days">Количество дней.</param>
        /// <param name="hours">Количество часов.</param>
        /// <param name="minutes">Количество минут.</param>
        /// <param name="seconds">Количество секунд.</param>
        /// <param name="milliseconds">Количество миллисекунд.</param>
        public Time(int days, int hours, int minutes, int seconds, int milliseconds)
        {
            TotalSeconds = days * SecondsPerDay + hours * SecondsPerHour + minutes * SecondsPerMinute + seconds + milliseconds * SecondsPerMillisecond;
        }


        /// <summary>
        /// Компонент времени для дней.
        /// </summary>
        public int Days => ToTimeSpan().Days;

        /// <summary>
        /// Время, выраженное в днях.
        /// </summary>
        public double TotalDays => ToTimeSpan().TotalDays;


        /// <summary>
        /// Компонент времени для часов.
        /// </summary>
        public int Hours => ToTimeSpan().Hours;

        /// <summary>
        /// Время, выраженное в часах.
        /// </summary>
        public double TotalHours => ToTimeSpan().TotalHours;


        /// <summary>
        /// Компонент времени для минут.
        /// </summary>
        public int Minutes => ToTimeSpan().Minutes;

        /// <summary>
        /// Время, выраженное в минутах.
        /// </summary>
        public double TotalMinutes => ToTimeSpan().TotalMinutes;


        /// <summary>
        /// Компонент времени для секунд.
        /// </summary>
        public int Seconds => ToTimeSpan().Seconds;

        /// <summary>
        /// Время, выраженное в секундах.
        /// </summary>
        public double TotalSeconds { get; }


        /// <summary>
        /// Компонент времени для миллисекунд.
        /// </summary>
        public int Milliseconds => ToTimeSpan().Milliseconds;

        /// <summary>
        /// Время, выраженное в миллисекундах.
        /// </summary>
        public double TotalMilliseconds => ToTimeSpan().TotalMilliseconds;


        /// <summary>
        /// Преобразует текущее значение времени в эквивалентное значение типа <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan ToTimeSpan()
        {
            return TimeSpan.FromSeconds(TotalSeconds);
        }


        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного.
        /// </summary>
        /// <param name="value">Значение времени.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и значения, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time Add(Time value)
        {
            return new Time(TotalSeconds + value.TotalSeconds);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного количества дней.
        /// </summary>
        /// <param name="value">Количество дней.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и количества дней, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time AddDays(double value)
        {
            return new Time(TotalSeconds + value * SecondsPerDay);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного количества часов.
        /// </summary>
        /// <param name="value">Количество часов.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и количества часов, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time AddHours(double value)
        {
            return new Time(TotalSeconds + value * SecondsPerHour);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного количества минут.
        /// </summary>
        /// <param name="value">Количество минут.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и количества минут, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time AddMinutes(double value)
        {
            return new Time(TotalSeconds + value * SecondsPerMinute);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного количества секунд.
        /// </summary>
        /// <param name="value">Количество секунд.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и количества секунд, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time AddSeconds(double value)
        {
            return new Time(TotalSeconds + value);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем добавления к текущему значению указанного количества миллисекунд.
        /// </summary>
        /// <param name="value">Количество миллисекунд.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно сумме текущего значения времени и количества миллисекунд, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time AddMilliseconds(double value)
        {
            return new Time(TotalSeconds + value * SecondsPerMillisecond);
        }

        /// <summary>
        /// Возвращает новое значение времени <see cref="Time"/>, полученное путем вычитания из текущего значения указанного.
        /// </summary>
        /// <param name="value">Значение времени.</param>
        /// <returns>
        /// Объект <see cref="Time"/>, значение которого равно разнице текущего значения времени и значения, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Time Subtract(Time value)
        {
            return new Time(TotalSeconds - value.TotalSeconds);
        }


        /// <summary>
        /// Сравнивает текущее значение и указанное.
        /// </summary>
        /// <param name="second">Значение для сравнение с текущим.</param>
        /// <returns>
        /// Целое число, определяющее отношение между текущим значением и <paramref name="second"/>.
        /// Значение меньше <c>0</c>, если текущее значение меньше, чем значение <paramref name="second"/>.
        /// Значение больше <c>0</c>, если текущее значение больше, чем значение <paramref name="second"/>.
        /// Значение равно <c>0</c>, если текущее значение равно значению <paramref name="second"/>.
        /// </returns>
        public int CompareTo(object second)
        {
            if (second == null)
            {
                return 1;
            }

            if (!(second is Time))
            {
                throw new ArgumentException();
            }

            return Compare(this, (Time)second);
        }

        /// <summary>
        /// Сравнивает текущее значение и указанное.
        /// </summary>
        /// <param name="second">Значение для сравнение с текущим.</param>
        /// <returns>
        /// Целое число, определяющее отношение между текущим значением и <paramref name="second"/>.
        /// Значение меньше <c>0</c>, если текущее значение меньше, чем значение <paramref name="second"/>.
        /// Значение больше <c>0</c>, если текущее значение больше, чем значение <paramref name="second"/>.
        /// Значение равно <c>0</c>, если текущее значение равно значению <paramref name="second"/>.
        /// </returns>
        public int CompareTo(Time second)
        {
            return Compare(this, second);
        }

        /// <summary>
        /// Сравнивает два значения типа <see cref="Time"/>.
        /// </summary>
        /// <param name="first">Первое значение.</param>
        /// <param name="second">Второе значение.</param>
        /// <returns>
        /// Целое число, определяющее отношение между значениями <paramref name="first"/> и <paramref name="second"/>.
        /// Значение меньше <c>0</c>, если значение <paramref name="first"/> меньше, чем значение <paramref name="second"/>.
        /// Значение больше <c>0</c>, если значение <paramref name="first"/> больше, чем значение <paramref name="second"/>.
        /// Значение равно <c>0</c>, если значение <paramref name="first"/> равно значению <paramref name="second"/>.
        /// </returns>
        public static int Compare(Time first, Time second)
        {
            if (first.TotalSeconds > second.TotalSeconds)
            {
                return 1;
            }

            if (first.TotalSeconds < second.TotalSeconds)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Проверяет равенство текущего значения с указанным.
        /// </summary>
        /// <param name="second">Значение для сравнение с текущим.</param>
        /// <returns>
        /// Значение <c>true</c>, если текущее значение равно значению <paramref name="second"/>, в противном случае - <c>false</c>.
        /// </returns>
        public override bool Equals(object second)
        {
            return (second is Time) && Equals(this, (Time)second);
        }

        /// <summary>
        /// Проверяет равенство текущего значения с указанным.
        /// </summary>
        /// <param name="second">Значение для сравнение с текущим.</param>
        /// <returns>
        /// Значение <c>true</c>, если текущее значение равно значению <paramref name="second"/>, в противном случае - <c>false</c>.
        /// </returns>
        public bool Equals(Time second)
        {
            return Equals(this, second);
        }

        /// <summary>
        /// Проверяет равенство двух значений типа <see cref="Time"/>.
        /// </summary>
        /// <param name="first">Первое значение.</param>
        /// <param name="second">Второе значение.</param>
        /// <returns>
        /// Значение <c>true</c>, если значение <paramref name="first"/> равно значению <paramref name="second"/>, в противном случае - <c>false</c>.
        /// </returns>
        public static bool Equals(Time first, Time second)
        {
            return ((long)(first.TotalSeconds * MillisecondsPerSecond) == (long)(second.TotalSeconds * MillisecondsPerSecond));
        }


        /// <summary>
        /// Возвращает хэш-код для данного экземпляра.
        /// </summary>
        /// <returns>
        /// Целочисленный хэш-код.
        /// </returns>
        public override int GetHashCode()
        {
            return TotalSeconds.GetHashCode();
        }


        /// <summary>
        /// Преобразует строковое представление времени в эквивалентный значение <see cref="Time"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая время, которое нужно преобразовать.</param>
        /// <returns>
        /// Значение <see cref="Time"/>, эквивалентное времени в параметре <paramref name="value"/>.
        /// </returns>
        public static Time Parse(string value)
        {
            return (Time)TimeSpan.Parse(value);
        }

        /// <summary>
        /// Преобразует строковое представление времени в эквивалентный значение <see cref="Time"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая время, которое нужно преобразовать.</param>
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <returns>
        /// Значение <see cref="Time"/>, эквивалентное времени в параметре <paramref name="value"/>.
        /// </returns>
        public static Time Parse(string value, IFormatProvider provider)
        {
            return (Time)TimeSpan.Parse(value, provider);
        }


        /// <summary>
        /// Преобразует строковое представление времени в эквивалентный значение <see cref="Time"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая время, которое нужно преобразовать.</param>
        /// <param name="result">Значение <see cref="Time"/>, эквивалентное времени в параметре <paramref name="value"/>.</param>
        /// <returns>
        /// Значение <c>true</c>, если параметр <paramref name="value"/> успешно преобразован, в противном случае — <c>false</c>.
        /// </returns>
        public static bool TryParse(string value, out Time result)
        {
            TimeSpan timeSpan;
            var success = TimeSpan.TryParse(value, out timeSpan);
            result = (Time)timeSpan;
            return success;
        }

        /// <summary>
        /// Преобразует строковое представление времени в эквивалентный значение <see cref="Time"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая время, которое нужно преобразовать.</param>
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <param name="result">Значение <see cref="Time"/>, эквивалентное времени в параметре <paramref name="value"/>.</param>
        /// <returns>
        /// Значение <c>true</c>, если параметр <paramref name="value"/> успешно преобразован, в противном случае — <c>false</c>.
        /// </returns>
        public static bool TryParse(string value, IFormatProvider provider, out Time result)
        {
            TimeSpan timeSpan;
            var success = TimeSpan.TryParse(value, provider, out timeSpan);
            result = (Time)timeSpan;
            return success;
        }


        /// <summary>
        /// Преобразует текущее значение времени <see cref="Time"/> в эквивалентное ему строковое представление.
        /// </summary>
        public override string ToString()
        {
            return ToShortTimeString();
        }

        /// <summary>
        /// Преобразует текущее значение времени <see cref="Time"/> в эквивалентное ему короткое строковое представление.
        /// </summary>
        public string ToShortTimeString()
        {
            var timeSpan = (TimeSpan)this;

            var shortTimeFormat = (TotalSeconds > 0)
                ? (timeSpan.Days == 0 ? @"hh\:mm\:ss" : @"d\.hh\:mm\:ss")
                : (timeSpan.Days == 0 ? @"\-hh\:mm\:ss" : @"\-d\.hh\:mm\:ss");

            return timeSpan.ToString(shortTimeFormat);
        }

        /// <summary>
        /// Преобразует текущее значение времени <see cref="Time"/> в эквивалентное ему длинное строковое представление.
        /// </summary>
        public string ToLongTimeString()
        {
            var timeSpan = (TimeSpan)this;

            var shortTimeFormat = (TotalSeconds > 0)
                ? (timeSpan.Days == 0 ? @"hh\:mm\:ss\.fffffff" : @"d\.hh\:mm\:ss\.fffffff")
                : (timeSpan.Days == 0 ? @"\-hh\:mm\:ss\.fffffff" : @"\-d\.hh\:mm\:ss\.fffffff");

            return timeSpan.ToString(shortTimeFormat);
        }

        /// <summary>
        /// Преобразует текущее значение времени <see cref="Time"/> в эквивалентное ему строковое представление.
        /// </summary>
        /// <param name="format">Строка стандартного или пользовательского формата времени.</param> 
        /// <remarks>
        /// Значение параметра <paramref name="format"/> должно удовлетворять требованиям к одноименному параметру в методе <see cref="TimeSpan.ToString(string)"/>.
        /// </remarks>
        public string ToString(string format)
        {
            return ((TimeSpan)this).ToString(format);
        }

        /// <summary>
        /// Преобразует текущее значение времени <see cref="Time"/> в эквивалентное ему строковое представление.
        /// </summary>
        /// <param name="format">Строка стандартного или пользовательского формата времени.</param> 
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <remarks>
        /// Значение параметра <paramref name="format"/> должно удовлетворять требованиям к одноименному параметру в методе <see cref="DateTime.ToString(string,IFormatProvider)"/>.
        /// </remarks>
        public string ToString(string format, IFormatProvider provider)
        {
            return ((TimeSpan)this).ToString(format, provider);
        }


        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, содержащее указанное количество дней.
        /// </summary>
        /// <param name="value">Количество дней.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, содержащее указанное количество дней.
        /// </returns>
        public static Time FromDays(double value)
        {
            return new Time(value * SecondsPerDay);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, содержащее указанное количество часов.
        /// </summary>
        /// <param name="value">Количество часов.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, содержащее указанное количество часов.
        /// </returns>
        public static Time FromHours(double value)
        {
            return new Time(value * SecondsPerHour);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, содержащее указанное количество минут.
        /// </summary>
        /// <param name="value">Количество минут.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, содержащее указанное количество минут.
        /// </returns>
        public static Time FromMinutes(double value)
        {
            return new Time(value * SecondsPerMinute);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, содержащее указанное количество секунд.
        /// </summary>
        /// <param name="value">Количество секунд.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, содержащее указанное количество секунд.
        /// </returns>
        public static Time FromSeconds(double value)
        {
            return new Time(value);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, содержащее указанное количество миллисекунд.
        /// </summary>
        /// <param name="value">Количество миллисекунд.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, содержащее указанное количество миллисекунд.
        /// </returns>
        public static Time FromMilliseconds(double value)
        {
            return new Time(value * SecondsPerMillisecond);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, равное указанному значению <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">Значение <see cref="TimeSpan"/>.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, равное указанному значению <see cref="TimeSpan"/>.
        /// </returns>
        public static Time FromTimeSpan(TimeSpan value)
        {
            return new Time(value.TotalSeconds);
        }

        /// <summary>
        /// Возвращает значение времени <see cref="Time"/>, равное части времени в значении <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">Значение <see cref="DateTime"/>.</param>
        /// <returns>
        /// Значение времени <see cref="Time"/>, равное части времени в значении <see cref="DateTime"/>.
        /// </returns>
        public static Time FromDateTime(DateTime value)
        {
            return new Time((value - value.Date).TotalSeconds);
        }


        public static explicit operator Time(double value) { return new Time(value); }
        public static explicit operator double(Time value) { return value.TotalSeconds; }

        public static explicit operator Time(TimeSpan value) { return FromTimeSpan(value); }
        public static explicit operator TimeSpan(Time value) { return value.ToTimeSpan(); }


        public static bool operator ==(Time left, Time right) { return Equals(left, right); }
        public static bool operator !=(Time left, Time right) { return !(left == right); }
        public static bool operator ==(Time left, double right) { return Equals(left, (Time)right); }
        public static bool operator !=(Time left, double right) { return !(left == right); }
        public static bool operator ==(double left, Time right) { return Equals((Time)left, right); }
        public static bool operator !=(double left, Time right) { return !(left == right); }


        public static bool operator <(Time left, Time right) { return left.TotalSeconds < right.TotalSeconds; }
        public static bool operator <=(Time left, Time right) { return left.TotalSeconds <= right.TotalSeconds; }
        public static bool operator >(Time left, Time right) { return left.TotalSeconds > right.TotalSeconds; }
        public static bool operator >=(Time left, Time right) { return left.TotalSeconds >= right.TotalSeconds; }
        public static bool operator <(Time left, double right) { return left.TotalSeconds < right; }
        public static bool operator <=(Time left, double right) { return left.TotalSeconds <= right; }
        public static bool operator >(Time left, double right) { return left.TotalSeconds > right; }
        public static bool operator >=(Time left, double right) { return left.TotalSeconds >= right; }
        public static bool operator <(double left, Time right) { return left < right.TotalSeconds; }
        public static bool operator <=(double left, Time right) { return left <= right.TotalSeconds; }
        public static bool operator >(double left, Time right) { return left > right.TotalSeconds; }
        public static bool operator >=(double left, Time right) { return left >= right.TotalSeconds; }


        public static Time operator +(Time value) { return value; }
        public static Time operator -(Time value) { return new Time(-value.TotalSeconds); }

        public static Time operator +(Time left, Time right) { return left.Add(right); }
        public static Time operator +(Time left, double right) { return (left + (Time)right); }
        public static Time operator +(double left, Time right) { return ((Time)left + right); }
        public static Time operator +(Time left, TimeSpan right) { return (left + (Time)right); }
        public static Time operator +(TimeSpan left, Time right) { return ((Time)left + right); }
        public static DateTime operator +(DateTime left, Time right) { return (left + (TimeSpan)right); }
        public static DateTimeOffset operator +(DateTimeOffset left, Time right) { return (left + (TimeSpan)right); }

        public static Time operator -(Time left, Time right) { return left.Subtract(right); }
        public static Time operator -(Time left, double right) { return (left - (Time)right); }
        public static Time operator -(double left, Time right) { return ((Time)left - right); }
        public static Time operator -(Time left, TimeSpan right) { return (left - (Time)right); }
        public static Time operator -(TimeSpan left, Time right) { return ((Time)left - right); }
        public static DateTime operator -(DateTime left, Time right) { return (left - (TimeSpan)right); }
        public static DateTimeOffset operator -(DateTimeOffset left, Time right) { return (left - (TimeSpan)right); }
    }
}