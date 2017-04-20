using System;
using System.Globalization;

namespace InfinniPlatform.Types
{
    /// <summary>
    /// Дата.
    /// </summary>
    /// <remarks>
    /// Определяет дату, которая содержит год, месяц и день. При этом не учитывается временная зона. На уровне реализации
    /// дата хранится в виде целого числа типа <see cref="long"/>, содержащего <see cref="UnixTime"/> - количество секунд,
    /// начиная с "1970-01-01T00:00:00Z". Поддерживает операции приведения и сравнения, в том числе c такими типами, как
    /// <see cref="long"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>. Предназначен для использования в сценариях,
    /// в которых не нужно учитывать временную зону, а важна только календарная дата.
    /// </remarks>
    [Serializable]
    public struct Date : IComparable, IComparable<Date>, IEquatable<Date>, IFormattable
    {
        /// <summary>
        /// Момент начала отсчета для Unix Time.
        /// </summary>
        public static readonly DateTime UnixTimeZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        /// <summary>
        /// Текущая дата в локальной временной зоне.
        /// </summary>
        public static Date Now => (Date)DateTime.Now;

        /// <summary>
        /// Текущая дата в нулевой временной зоне.
        /// </summary>
        public static Date UtcNow => (Date)DateTime.UtcNow;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="unixTime">Время в формате Unix Time.</param>
        public Date(long unixTime)
        {
            UnixTime = unixTime;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="month">Месяц.</param>
        /// <param name="day">День.</param>
        public Date(int year, int month, int day)
        {
            UnixTime = (long)new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc).Subtract(UnixTimeZero).TotalSeconds;
        }


        /// <summary>
        /// Дата в формате Unix Time.
        /// </summary>
        public long UnixTime { get; }


        /// <summary>
        /// Год.
        /// </summary>
        public int Year => ToUtcDateTime().Year;

        /// <summary>
        /// Месяц.
        /// </summary>
        public int Month => ToUtcDateTime().Month;

        /// <summary>
        /// День.
        /// </summary>
        public int Day => ToUtcDateTime().Day;


        /// <summary>
        /// Возвращает новое значение даты <see cref="Date"/>, полученное путем добавления к текущему значению указанного количества лет.
        /// </summary>
        /// <param name="value">Количество лет.</param>
        /// <returns>
        /// Объект <see cref="Date"/>, значение которого равно сумме текущего значения даты и количества лет, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Date AddYears(int value)
        {
            return (Date)ToUtcDateTime().AddYears(value);
        }

        /// <summary>
        /// Возвращает новое значение даты <see cref="Date"/>, полученное путем добавления к текущему значению указанного количества месяцев.
        /// </summary>
        /// <param name="value">Количество месяцев.</param>
        /// <returns>
        /// Объект <see cref="Date"/>, значение которого равно сумме текущего значения даты и количества месяцев, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Date AddMonths(int value)
        {
            return (Date)ToUtcDateTime().AddMonths(value);
        }

        /// <summary>
        /// Возвращает новое значение даты <see cref="Date"/>, полученное путем добавления к текущему значению указанного количества дней.
        /// </summary>
        /// <param name="value">Количество дней.</param>
        /// <returns>
        /// Объект <see cref="Date"/>, значение которого равно сумме текущего значения даты и количества дней, представленного параметром <paramref name="value"/>.
        /// </returns>
        public Date AddDays(int value)
        {
            return (Date)ToUtcDateTime().AddDays(value);
        }


        /// <summary>
        /// Преобразует текущее значение даты в значение типа <see cref="DateTime"/> в локальной временной зоне.
        /// </summary>
        public DateTime ToLocalDateTime()
        {
            var utcDateTime = ToUtcDateTime();
            return new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, 0, 0, 0, DateTimeKind.Local);
        }

        /// <summary>
        /// Преобразует текущее значение даты в значение типа <see cref="DateTime"/> в нулевой временной зоне.
        /// </summary>
        public DateTime ToUtcDateTime()
        {
            return UnixTimeZero.AddSeconds(UnixTime);
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

            if (!(second is Date))
            {
                throw new ArgumentException();
            }

            return Compare(this, (Date)second);
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
        public int CompareTo(Date second)
        {
            return Compare(this, second);
        }

        /// <summary>
        /// Сравнивает два значения типа <see cref="Date"/>.
        /// </summary>
        /// <param name="first">Первое значение.</param>
        /// <param name="second">Второе значение.</param>
        /// <returns>
        /// Целое число, определяющее отношение между значениями <paramref name="first"/> и <paramref name="second"/>.
        /// Значение меньше <c>0</c>, если значение <paramref name="first"/> меньше, чем значение <paramref name="second"/>.
        /// Значение больше <c>0</c>, если значение <paramref name="first"/> больше, чем значение <paramref name="second"/>.
        /// Значение равно <c>0</c>, если значение <paramref name="first"/> равно значению <paramref name="second"/>.
        /// </returns>
        public static int Compare(Date first, Date second)
        {
            if (first.UnixTime > second.UnixTime)
            {
                return 1;
            }

            if (first.UnixTime < second.UnixTime)
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
            return (second is Date) && Equals(this, (Date)second);
        }

        /// <summary>
        /// Проверяет равенство текущего значения с указанным.
        /// </summary>
        /// <param name="second">Значение для сравнение с текущим.</param>
        /// <returns>
        /// Значение <c>true</c>, если текущее значение равно значению <paramref name="second"/>, в противном случае - <c>false</c>.
        /// </returns>
        public bool Equals(Date second)
        {
            return Equals(this, second);
        }

        /// <summary>
        /// Проверяет равенство двух значений типа <see cref="Date"/>.
        /// </summary>
        /// <param name="first">Первое значение.</param>
        /// <param name="second">Второе значение.</param>
        /// <returns>
        /// Значение <c>true</c>, если значение <paramref name="first"/> равно значению <paramref name="second"/>, в противном случае - <c>false</c>.
        /// </returns>
        public static bool Equals(Date first, Date second)
        {
            return (first.UnixTime == second.UnixTime);
        }


        /// <summary>
        /// Возвращает хэш-код для данного экземпляра.
        /// </summary>
        /// <returns>
        /// Целочисленный хэш-код.
        /// </returns>
        public override int GetHashCode()
        {
            return UnixTime.GetHashCode();
        }


        /// <summary>
        /// Преобразует строковое представление даты в эквивалентный значение <see cref="Date"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая дату, которую нужно преобразовать.</param>
        /// <returns>
        /// Значение <see cref="Date"/>, эквивалентное дате в параметре <paramref name="value"/>.
        /// </returns>
        public static Date Parse(string value)
        {
            return (Date)DateTime.Parse(value);
        }

        /// <summary>
        /// Преобразует строковое представление даты в эквивалентный значение <see cref="Date"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая дату, которую нужно преобразовать.</param>
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <returns>
        /// Значение <see cref="Date"/>, эквивалентное дате в параметре <paramref name="value"/>.
        /// </returns>
        public static Date Parse(string value, IFormatProvider provider)
        {
            return (Date)DateTime.Parse(value, provider, DateTimeStyles.None);
        }


        /// <summary>
        /// Преобразует строковое представление даты в эквивалентный значение <see cref="Date"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая дату, которую нужно преобразовать.</param>
        /// <param name="result">Значение <see cref="Date"/>, эквивалентное дате в параметре <paramref name="value"/>.</param>
        /// <returns>
        /// Значение <c>true</c>, если параметр <paramref name="value"/> успешно преобразован, в противном случае — <c>false</c>.
        /// </returns>
        public static bool TryParse(string value, out Date result)
        {
            DateTime dateTime;
            var success = DateTime.TryParse(value, out dateTime);
            result = (Date)dateTime;
            return success;
        }

        /// <summary>
        /// Преобразует строковое представление даты в эквивалентный значение <see cref="Date"/>.
        /// </summary>
        /// <param name="value">Строка, содержащая дату, которую нужно преобразовать.</param>
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <param name="result">Значение <see cref="Date"/>, эквивалентное дате в параметре <paramref name="value"/>.</param>
        /// <returns>
        /// Значение <c>true</c>, если параметр <paramref name="value"/> успешно преобразован, в противном случае — <c>false</c>.
        /// </returns>
        public static bool TryParse(string value, IFormatProvider provider, out Date result)
        {
            DateTime dateTime;
            var success = DateTime.TryParse(value, provider, DateTimeStyles.None, out dateTime);
            result = (Date)dateTime;
            return success;
        }


        /// <summary>
        /// Преобразует текущее значение даты <see cref="Date"/> в эквивалентное ему строковое представление.
        /// </summary>
        public override string ToString()
        {
            return ToShortDateString();
        }

        /// <summary>
        /// Преобразует текущее значение даты <see cref="Date"/> в эквивалентное ему короткое строковое представление.
        /// </summary>
        public string ToShortDateString()
        {
            return ((DateTime)this).ToString("d");
        }

        /// <summary>
        /// Преобразует текущее значение даты <see cref="Date"/> в эквивалентное ему длинное строковое представление.
        /// </summary>
        public string ToLongDateString()
        {
            return ((DateTime)this).ToString("D");
        }

        /// <summary>
        /// Преобразует текущее значение даты <see cref="Date"/> в эквивалентное ему строковое представление.
        /// </summary>
        /// <param name="format">Строка стандартного или пользовательского формата даты.</param> 
        /// <remarks>
        /// Значение параметра <paramref name="format"/> должно удовлетворять требованиям к одноименному параметру в методе <see cref="DateTime.ToString(string)"/>.
        /// </remarks>
        public string ToString(string format)
        {
            return ((DateTime)this).ToString(format);
        }

        /// <summary>
        /// Преобразует текущее значение даты <see cref="Date"/> в эквивалентное ему строковое представление.
        /// </summary>
        /// <param name="format">Строка стандартного или пользовательского формата даты.</param> 
        /// <param name="provider">Объект, предоставляющий сведения о форматировании.</param>
        /// <remarks>
        /// Значение параметра <paramref name="format"/> должно удовлетворять требованиям к одноименному параметру в методе <see cref="DateTime.ToString(string,IFormatProvider)"/>.
        /// </remarks>
        public string ToString(string format, IFormatProvider provider)
        {
            return ((DateTime)this).ToString(format, provider);
        }


        public static explicit operator Date(long value) { return new Date(value); }
        public static explicit operator long(Date value) { return value.UnixTime; }
        
        public static explicit operator Date(int value) { return new Date(value); }
        public static explicit operator int(Date value) { return (int)value.UnixTime; }

        public static explicit operator Date(DateTime value) { return new Date(value.Year, value.Month, value.Day); }
        public static explicit operator DateTime(Date value) { return value.ToUtcDateTime(); }

        public static explicit operator Date(DateTimeOffset value) { return new Date(value.Year, value.Month, value.Day); }
        public static explicit operator DateTimeOffset(Date value) { return value.ToUtcDateTime(); }


        public static bool operator ==(Date left, Date right) { return Equals(left, right); }
        public static bool operator !=(Date left, Date right) { return !(left == right); }
        public static bool operator ==(Date left, long right) { return Equals(left, (Date)right); }
        public static bool operator !=(Date left, long right) { return !(left == right); }
        public static bool operator ==(long left, Date right) { return Equals((Date)left, right); }
        public static bool operator !=(long left, Date right) { return !(left == right); }
        public static bool operator ==(Date left, int right) { return Equals(left, (Date)right); }
        public static bool operator !=(Date left, int right) { return !(left == right); }
        public static bool operator ==(int left, Date right) { return Equals((Date)left, right); }
        public static bool operator !=(int left, Date right) { return !(left == right); }


        public static bool operator <(Date left, Date right) { return left.UnixTime < right.UnixTime; }
        public static bool operator <=(Date left, Date right) { return left.UnixTime <= right.UnixTime; }
        public static bool operator >(Date left, Date right) { return left.UnixTime > right.UnixTime; }
        public static bool operator >=(Date left, Date right) { return left.UnixTime >= right.UnixTime; }


        public static bool operator <(Date left, long right) { return left.UnixTime < right; }
        public static bool operator <=(Date left, long right) { return left.UnixTime <= right; }
        public static bool operator >(Date left, long right) { return left.UnixTime > right; }
        public static bool operator >=(Date left, long right) { return left.UnixTime >= right; }
        public static bool operator <(long left, Date right) { return left < right.UnixTime; }
        public static bool operator <=(long left, Date right) { return left <= right.UnixTime; }
        public static bool operator >(long left, Date right) { return left > right.UnixTime; }
        public static bool operator >=(long left, Date right) { return left >= right.UnixTime; }


        public static bool operator <(Date left, int right) { return left.UnixTime < right; }
        public static bool operator <=(Date left, int right) { return left.UnixTime <= right; }
        public static bool operator >(Date left, int right) { return left.UnixTime > right; }
        public static bool operator >=(Date left, int right) { return left.UnixTime >= right; }
        public static bool operator <(int left, Date right) { return left < right.UnixTime; }
        public static bool operator <=(int left, Date right) { return left <= right.UnixTime; }
        public static bool operator >(int left, Date right) { return left > right.UnixTime; }
        public static bool operator >=(int left, Date right) { return left >= right.UnixTime; }

        public static Time operator -(Date left, Date right) { return new Time(left.UnixTime - right.UnixTime); }
    }
}