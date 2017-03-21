using System;

using InfinniPlatform.Sdk.Types;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests.Types
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DateTest
    {
        [Test]
        public void ShouldCreateDateFromUnixTime()
        {
            // Given
            const long unixTime = 1234567890;

            // When
            var date = new Date(unixTime);

            // Then
            Assert.AreEqual(unixTime, date.UnixTime);
        }

        [Test]
        public void ShouldCreateDateFromYearMonthDay()
        {
            // Given
            const int year = 2016;
            const int month = 03;
            const int day = 30;

            // When
            var date = new Date(year, month, day);

            // Then
            Assert.AreEqual(year, date.Year);
            Assert.AreEqual(month, date.Month);
            Assert.AreEqual(day, date.Day);
        }

        [Test]
        public void ShouldReturnsNowDate()
        {
            // Given
            var nowDateTime = DateTime.Now;
            var utcNowDateTime = DateTime.UtcNow;

            // When
            var nowDate = Date.Now;
            var utcNowDate = Date.UtcNow;

            // Then
            Assert.IsTrue(Math.Abs((new DateTime(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0, DateTimeKind.Local) - nowDateTime.Date).TotalSeconds) <= 1);
            Assert.IsTrue(Math.Abs((new DateTime(utcNowDate.Year, utcNowDate.Month, utcNowDate.Day, 0, 0, 0, DateTimeKind.Utc) - utcNowDateTime.Date).TotalSeconds) <= 1);
        }

        [Test]
        public void ShouldCastToLocalDateTime()
        {
            // Given
            var date = Date.Now;

            // When
            var localDateTime = date.ToLocalDateTime();

            // Then
            Assert.AreEqual(date.Year, localDateTime.Year);
            Assert.AreEqual(date.Month, localDateTime.Month);
            Assert.AreEqual(date.Day, localDateTime.Day);
            Assert.AreEqual(0, localDateTime.Hour);
            Assert.AreEqual(0, localDateTime.Minute);
            Assert.AreEqual(0, localDateTime.Second);
            Assert.AreEqual(0, localDateTime.Millisecond);
            Assert.AreEqual(DateTimeKind.Local, localDateTime.Kind);
        }

        [Test]
        public void ShouldCastToUtcDateTime()
        {
            // Given
            var date = Date.UtcNow;

            // When
            var utcDateTime = date.ToUtcDateTime();

            // Then
            Assert.AreEqual(date.Year, utcDateTime.Year);
            Assert.AreEqual(date.Month, utcDateTime.Month);
            Assert.AreEqual(date.Day, utcDateTime.Day);
            Assert.AreEqual(0, utcDateTime.Hour);
            Assert.AreEqual(0, utcDateTime.Minute);
            Assert.AreEqual(0, utcDateTime.Second);
            Assert.AreEqual(0, utcDateTime.Millisecond);
            Assert.AreEqual(DateTimeKind.Utc, utcDateTime.Kind);
        }

        [Test]
        public void ShouldAddYears()
        {
            // Given
            const int value = 10;
            var date = new Date(2016, 03, 30);
            var dateTime = new DateTime(date.Year, date.Month, date.Day);

            // When
            var newDate = date.AddYears(value);
            var newDateTime = dateTime.AddYears(value);

            // Then
            Assert.AreEqual(newDateTime.Year, newDate.Year);
            Assert.AreEqual(newDateTime.Month, newDate.Month);
            Assert.AreEqual(newDateTime.Day, newDate.Day);
        }

        [Test]
        public void ShouldAddMonths()
        {
            // Given
            const int value = 10;
            var date = new Date(2016, 03, 30);
            var dateTime = new DateTime(date.Year, date.Month, date.Day);

            // When
            var newDate = date.AddMonths(value);
            var newDateTime = dateTime.AddMonths(value);

            // Then
            Assert.AreEqual(newDateTime.Year, newDate.Year);
            Assert.AreEqual(newDateTime.Month, newDate.Month);
            Assert.AreEqual(newDateTime.Day, newDate.Day);
        }

        [Test]
        public void ShouldAddDays()
        {
            // Given
            const int value = 10;
            var date = new Date(2016, 03, 30);
            var dateTime = new DateTime(date.Year, date.Month, date.Day);

            // When
            var newDate = date.AddDays(value);
            var newDateTime = dateTime.AddDays(value);

            // Then
            Assert.AreEqual(newDateTime.Year, newDate.Year);
            Assert.AreEqual(newDateTime.Month, newDate.Month);
            Assert.AreEqual(newDateTime.Day, newDate.Day);
        }

        [Test]
        public void ShouldCompareTo()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = new Date(2016, 03, 31);
            var date3 = new Date(2016, 04, 01);

            // When & Then
            Assert.AreEqual(0, date1.CompareTo(date1));
            Assert.AreEqual(-1, date1.CompareTo(date2));
            Assert.AreEqual(-1, date1.CompareTo(date3));
            Assert.AreEqual(1, date2.CompareTo(date1));
            Assert.AreEqual(0, date2.CompareTo(date2));
            Assert.AreEqual(-1, date2.CompareTo(date3));
            Assert.AreEqual(1, date3.CompareTo(date1));
            Assert.AreEqual(1, date3.CompareTo(date2));
            Assert.AreEqual(0, date3.CompareTo(date3));
        }

        [Test]
        public void ShouldEquals()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = new Date(2016, 03, 31);
            var date3 = new Date(2016, 04, 01);

            // When & Then
            Assert.AreEqual(true, date1.Equals(date1));
            Assert.AreEqual(false, date1.Equals(date2));
            Assert.AreEqual(false, date1.Equals(date3));
            Assert.AreEqual(false, date2.Equals(date1));
            Assert.AreEqual(true, date2.Equals(date2));
            Assert.AreEqual(false, date2.Equals(date3));
            Assert.AreEqual(false, date3.Equals(date1));
            Assert.AreEqual(false, date3.Equals(date2));
            Assert.AreEqual(true, date3.Equals(date3));
        }

        [Test]
        public void ShouldParse()
        {
            // Given
            const string dateString = "2016-03-30";
            var expectedDate = new Date(2016, 03, 30);

            // When
            var date = Date.Parse(dateString);

            // Then
            Assert.AreEqual(expectedDate.UnixTime, date.UnixTime);
            Assert.AreEqual(expectedDate.Year, date.Year);
            Assert.AreEqual(expectedDate.Month, date.Month);
            Assert.AreEqual(expectedDate.Day, date.Day);
        }

        [Test]
        public void ShouldTryParse()
        {
            // Given
            const string rightDateString = "2016-03-30";
            const string wrongDateString = "2016-03-30!";
            var expectedDate = new Date(2016, 03, 30);

            // When

            Date rightDate;
            var rightResult = Date.TryParse(rightDateString, out rightDate);

            Date wrongDate;
            var wrongResult = Date.TryParse(wrongDateString, out wrongDate);

            // Then
            Assert.AreEqual(true, rightResult);
            Assert.AreEqual(false, wrongResult);
            Assert.AreEqual(expectedDate.UnixTime, rightDate.UnixTime);
            Assert.AreEqual(expectedDate.Year, rightDate.Year);
            Assert.AreEqual(expectedDate.Month, rightDate.Month);
            Assert.AreEqual(expectedDate.Day, rightDate.Day);
        }

        [Test]
        public void ShouldToString()
        {
            // Given
            var date = Date.Now;
            var expectedDateString = new DateTime(date.Year, date.Month, date.Day).ToString("d");

            // When
            var actualDateString = date.ToString();

            // Then
            Assert.AreEqual(expectedDateString, actualDateString);
        }

        [Test]
        public void ShouldToShortDateString()
        {
            // Given
            var date = Date.Now;
            var expectedDateString = new DateTime(date.Year, date.Month, date.Day).ToString("d");

            // When
            var actualDateString = date.ToShortDateString();

            // Then
            Assert.AreEqual(expectedDateString, actualDateString);
        }

        [Test]
        public void ShouldToLongDateString()
        {
            // Given
            var date = Date.Now;
            var expectedDateString = new DateTime(date.Year, date.Month, date.Day).ToString("D");

            // When
            var actualDateString = date.ToLongDateString();

            // Then
            Assert.AreEqual(expectedDateString, actualDateString);
        }

        [Test]
        public void ShouldToStringFormat()
        {
            // Given
            var date = Date.Now;
            var expectedDateString = new DateTime(date.Year, date.Month, date.Day).ToString("yyyy-M-d dddd");

            // When
            var actualDateString = date.ToString("yyyy-M-d dddd");

            // Then
            Assert.AreEqual(expectedDateString, actualDateString);
        }

        [Test]
        public void ShouldCastOperatorToLong()
        {
            // Given
            const long unixTime = 1234567890;

            // When
            var date1 = (Date)unixTime;
            var date2 = (long)date1;
            
            // Then
            Assert.AreEqual(unixTime, date1.UnixTime);
            Assert.AreEqual(unixTime, date2);
        }

        [Test]
        public void ShouldCastOperatorToInt()
        {
            // Given
            const int unixTime = 342423432;

            // When
            var date1 = (Date)unixTime;
            var date2 = (int)date1;

            // Then
            Assert.AreEqual(unixTime, date1.UnixTime);
            Assert.AreEqual(unixTime, date2);
        }

        [Test]
        public void ShouldCastOperatorToDateTime()
        {
            // Given
            var dateTime = DateTime.Now;

            // When
            var date1 = (Date)dateTime;
            var date2 = (DateTime)date1;
            
            // Then
            Assert.AreEqual(dateTime.Year, date1.Year);
            Assert.AreEqual(dateTime.Month, date1.Month);
            Assert.AreEqual(dateTime.Day, date1.Day);
            Assert.AreEqual(dateTime.Year, date2.Year);
            Assert.AreEqual(dateTime.Month, date2.Month);
            Assert.AreEqual(dateTime.Day, date2.Day);
            Assert.AreEqual(0, date2.Hour);
            Assert.AreEqual(0, date2.Minute);
            Assert.AreEqual(0, date2.Second);
            Assert.AreEqual(0, date2.Minute);
            Assert.AreEqual(DateTimeKind.Utc, date2.Kind);
        }

        [Test]
        public void ShouldCastOperatorToDateTimeOffset()
        {
            // Given
            var dateTime = DateTimeOffset.Now;

            // When
            var date1 = (Date)dateTime;
            var date2 = (DateTimeOffset)date1;
            
            // Then
            Assert.AreEqual(dateTime.Year, date1.Year);
            Assert.AreEqual(dateTime.Month, date1.Month);
            Assert.AreEqual(dateTime.Day, date1.Day);
            Assert.AreEqual(dateTime.Year, date2.Year);
            Assert.AreEqual(dateTime.Month, date2.Month);
            Assert.AreEqual(dateTime.Day, date2.Day);
            Assert.AreEqual(0, date2.Hour);
            Assert.AreEqual(0, date2.Minute);
            Assert.AreEqual(0, date2.Second);
            Assert.AreEqual(0, date2.Minute);
            Assert.AreEqual(TimeSpan.Zero, date2.Offset);
        }

        [Test]
        public void ShouldEqualOperatorsForDate()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = new Date(2016, 03, 30);
            var date3 = new Date(2016, 04, 01);

            // When & Then

            Assert.IsTrue(date1 == date2);
            Assert.IsFalse(date1 != date2);
            Assert.IsFalse(date1 == date3);
            Assert.IsTrue(date1 != date3);

            Assert.IsTrue(date2 == date1);
            Assert.IsFalse(date2 != date1);
            Assert.IsFalse(date2 == date3);
            Assert.IsTrue(date2 != date3);

            Assert.IsFalse(date3 == date1);
            Assert.IsTrue(date3 != date1);
            Assert.IsFalse(date3 == date2);
            Assert.IsTrue(date3 != date2);
        }

        [Test]
        public void ShouldEqualOperatorsForLong()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = date1.UnixTime;
            var date3 = date1.UnixTime + 1;

            // When & Then

            Assert.IsTrue(date1 == date2);
            Assert.IsFalse(date1 != date2);
            Assert.IsFalse(date1 == date3);
            Assert.IsTrue(date1 != date3);

            Assert.IsTrue(date2 == date1);
            Assert.IsFalse(date2 != date1);
            Assert.IsFalse(date2 == date3);
            Assert.IsTrue(date2 != date3);

            Assert.IsFalse(date3 == date1);
            Assert.IsTrue(date3 != date1);
            Assert.IsFalse(date3 == date2);
            Assert.IsTrue(date3 != date2);
        }

        [Test]
        public void ShouldEqualOperatorsForInt()
        {
            // Given
            var date1 = new Date(1980, 03, 30);
            var date2 = (int)date1.UnixTime;
            var date3 = (int)date1.UnixTime + 1;

            // When & Then

            Assert.IsTrue(date1 == date2);
            Assert.IsFalse(date1 != date2);
            Assert.IsFalse(date1 == date3);
            Assert.IsTrue(date1 != date3);

            Assert.IsTrue(date2 == date1);
            Assert.IsFalse(date2 != date1);
            Assert.IsFalse(date2 == date3);
            Assert.IsTrue(date2 != date3);

            Assert.IsFalse(date3 == date1);
            Assert.IsTrue(date3 != date1);
            Assert.IsFalse(date3 == date2);
            Assert.IsTrue(date3 != date2);
        }

        [Test]
        public void ShouldCompareOperatorsForDate()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = new Date(2016, 03, 30);
            var date3 = new Date(2016, 04, 01);

            // When & Then

            Assert.IsFalse(date1 < date2);
            Assert.IsTrue(date1 < date3);
            Assert.IsTrue(date1 <= date2);
            Assert.IsTrue(date1 <= date3);
            Assert.IsFalse(date1 > date2);
            Assert.IsFalse(date1 > date3);
            Assert.IsTrue(date1 >= date2);
            Assert.IsFalse(date1 >= date3);

            Assert.IsFalse(date2 < date1);
            Assert.IsTrue(date2 < date3);
            Assert.IsTrue(date2 <= date1);
            Assert.IsTrue(date2 <= date3);
            Assert.IsFalse(date2 > date1);
            Assert.IsFalse(date2 > date3);
            Assert.IsTrue(date2 >= date1);
            Assert.IsFalse(date2 >= date3);

            Assert.IsFalse(date3 < date1);
            Assert.IsFalse(date3 < date2);
            Assert.IsFalse(date3 <= date1);
            Assert.IsFalse(date3 <= date2);
            Assert.IsTrue(date3 > date1);
            Assert.IsTrue(date3 > date2);
            Assert.IsTrue(date3 >= date1);
            Assert.IsTrue(date3 >= date2);
        }

        [Test]
        public void ShouldCompareOperatorsForLong()
        {
            // Given
            var date1 = new Date(2016, 03, 30);
            var date2 = date1.UnixTime;
            var date3 = date1.UnixTime + 1;

            // When & Then

            Assert.IsFalse(date1 < date2);
            Assert.IsTrue(date1 < date3);
            Assert.IsTrue(date1 <= date2);
            Assert.IsTrue(date1 <= date3);
            Assert.IsFalse(date1 > date2);
            Assert.IsFalse(date1 > date3);
            Assert.IsTrue(date1 >= date2);
            Assert.IsFalse(date1 >= date3);

            Assert.IsFalse(date2 < date1);
            Assert.IsTrue(date2 < date3);
            Assert.IsTrue(date2 <= date1);
            Assert.IsTrue(date2 <= date3);
            Assert.IsFalse(date2 > date1);
            Assert.IsFalse(date2 > date3);
            Assert.IsTrue(date2 >= date1);
            Assert.IsFalse(date2 >= date3);

            Assert.IsFalse(date3 < date1);
            Assert.IsFalse(date3 < date2);
            Assert.IsFalse(date3 <= date1);
            Assert.IsFalse(date3 <= date2);
            Assert.IsTrue(date3 > date1);
            Assert.IsTrue(date3 > date2);
            Assert.IsTrue(date3 >= date1);
            Assert.IsTrue(date3 >= date2);
        }

        [Test]
        public void ShouldCompareOperatorsForInt()
        {
            // Given
            var date1 = new Date(1980, 03, 30);
            var date2 = (int)date1.UnixTime;
            var date3 = (int)date1.UnixTime + 1;

            // When & Then

            Assert.IsFalse(date1 < date2);
            Assert.IsTrue(date1 < date3);
            Assert.IsTrue(date1 <= date2);
            Assert.IsTrue(date1 <= date3);
            Assert.IsFalse(date1 > date2);
            Assert.IsFalse(date1 > date3);
            Assert.IsTrue(date1 >= date2);
            Assert.IsFalse(date1 >= date3);

            Assert.IsFalse(date2 < date1);
            Assert.IsTrue(date2 < date3);
            Assert.IsTrue(date2 <= date1);
            Assert.IsTrue(date2 <= date3);
            Assert.IsFalse(date2 > date1);
            Assert.IsFalse(date2 > date3);
            Assert.IsTrue(date2 >= date1);
            Assert.IsFalse(date2 >= date3);

            Assert.IsFalse(date3 < date1);
            Assert.IsFalse(date3 < date2);
            Assert.IsFalse(date3 <= date1);
            Assert.IsFalse(date3 <= date2);
            Assert.IsTrue(date3 > date1);
            Assert.IsTrue(date3 > date2);
            Assert.IsTrue(date3 >= date1);
            Assert.IsTrue(date3 >= date2);
        }

        [Test]
        public void ShouldMinusOperator()
        {
            // Given
            const int days = 5;
            var date1 = Date.Now;
            var date2 = Date.Now.AddDays(days);

            // When
            var time1 = date2 - date1;
            var time2 = date1 - date2;

            // Then
            Assert.AreEqual(days, (int)time1.TotalDays);
            Assert.AreEqual(-days, (int)time2.TotalDays);
        }
    }
}