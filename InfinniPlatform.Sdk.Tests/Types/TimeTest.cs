using System;

using InfinniPlatform.Sdk.Types;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests.Types
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class TimeTest
    {
        private const double TimeInaccuracy = 0.0001;


        [Test]
        public void ShouldCreateTimeFromTotalSeconds()
        {
            // Given
            const double totalSeconds = 123456.7890;

            // When
            var time = new Time(totalSeconds);

            // Then
            Assert.AreEqual(totalSeconds, time.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromHoursMinutesSeconds()
        {
            // Given
            const int hours = 11;
            const int minutes = 22;
            const int seconds = 33;

            // When
            var time = new Time(hours, minutes, seconds);

            // Then
            Assert.AreEqual(hours, time.Hours);
            Assert.AreEqual(minutes, time.Minutes);
            Assert.AreEqual(seconds, time.Seconds);
            Assert.AreEqual(0, time.Milliseconds);
        }

        [Test]
        public void ShouldCreateTimeFromDaysHoursMinutesSeconds()
        {
            // Given
            const int days = 1;
            const int hours = 11;
            const int minutes = 22;
            const int seconds = 33;

            // When
            var time = new Time(days, hours, minutes, seconds);

            // Then
            Assert.AreEqual(days, time.Days);
            Assert.AreEqual(hours, time.Hours);
            Assert.AreEqual(minutes, time.Minutes);
            Assert.AreEqual(seconds, time.Seconds);
            Assert.AreEqual(0, time.Milliseconds);
        }

        [Test]
        public void ShouldCreateTimeFromDaysHoursMinutesSecondsMilliseconds()
        {
            // Given
            const int days = 1;
            const int hours = 11;
            const int minutes = 22;
            const int seconds = 33;
            const int milliseconds = 456;

            // When
            var time = new Time(days, hours, minutes, seconds, milliseconds);

            // Then
            Assert.AreEqual(days, time.Days);
            Assert.AreEqual(hours, time.Hours);
            Assert.AreEqual(minutes, time.Minutes);
            Assert.AreEqual(seconds, time.Seconds);
            Assert.AreEqual(milliseconds, time.Milliseconds);
        }

        [Test]
        public void ShouldCreateTimeFromDays()
        {
            // Given
            const double value = 12345.6789;

            // When
            var result = Time.FromDays(value);

            // Then
            Assert.AreEqual(24 * 60 * 60 * value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromHours()
        {
            // Given
            const double value = 12345.6789;

            // When
            var result = Time.FromHours(value);

            // Then
            Assert.AreEqual(60 * 60 * value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromMinutes()
        {
            // Given
            const double value = 12345.6789;

            // When
            var result = Time.FromMinutes(value);

            // Then
            Assert.AreEqual(60 * value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromSeconds()
        {
            // Given
            const double value = 12345.6789;

            // When
            var result = Time.FromSeconds(value);

            // Then
            Assert.AreEqual(value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromMilliseconds()
        {
            // Given
            const double value = 12345.6789;

            // When
            var result = Time.FromMilliseconds(value);

            // Then
            Assert.AreEqual(0.001 * value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromTimeSpan()
        {
            // Given
            var timeSpan = new TimeSpan(1, 2, 3, 4, 5);

            // When
            var result = Time.FromTimeSpan(timeSpan);

            // Then
            Assert.AreEqual(timeSpan.TotalSeconds, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCreateTimeFromDateTime()
        {
            // Given
            var dateTime = DateTime.Now;

            // When
            var result = Time.FromDateTime(dateTime);

            // Then
            Assert.AreEqual((dateTime - dateTime.Date).TotalSeconds, result.TotalSeconds, TimeInaccuracy);
        }


        [Test]
        public void ShouldReturnsNowTime()
        {
            // Given
            var nowDateTime = DateTime.Now;
            var utcNowDateTime = DateTime.UtcNow;

            // When
            var nowTime = Time.Now;
            var utcNowTime = Time.UtcNow;

            // Then
            Assert.AreEqual(60 * 60 * nowDateTime.Hour + 60 * nowDateTime.Minute + nowDateTime.Second + 0.001 * nowDateTime.Millisecond, nowTime.TotalSeconds, 1);
            Assert.AreEqual(60 * 60 * utcNowDateTime.Hour + 60 * utcNowDateTime.Minute + utcNowDateTime.Second + 0.001 * utcNowDateTime.Millisecond, utcNowTime.TotalSeconds, 1);
        }

        [Test]
        public void ShouldCastToTimeSpan()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);

            // When
            var timeSpan = time.ToTimeSpan();

            // Then
            Assert.AreEqual(timeSpan.Days, timeSpan.Days);
            Assert.AreEqual(timeSpan.Hours, timeSpan.Hours);
            Assert.AreEqual(timeSpan.Minutes, timeSpan.Minutes);
            Assert.AreEqual(timeSpan.Seconds, timeSpan.Seconds);
            Assert.AreEqual(timeSpan.Milliseconds, timeSpan.Milliseconds);
        }

        [Test]
        public void ShouldReturnsTotalDays()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);
            var expectedValue = new TimeSpan(1, 2, 3, 4, 5).TotalDays;

            // When
            var actualValue = time.TotalDays;

            // Then
            Assert.AreEqual(expectedValue, actualValue, TimeInaccuracy);
        }

        [Test]
        public void ShouldReturnsTotalHours()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);
            var expectedValue = new TimeSpan(1, 2, 3, 4, 5).TotalHours;

            // When
            var actualValue = time.TotalHours;

            // Then
            Assert.AreEqual(expectedValue, actualValue, TimeInaccuracy);
        }

        [Test]
        public void ShouldReturnsTotalMinutes()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);
            var expectedValue = new TimeSpan(1, 2, 3, 4, 5).TotalMinutes;

            // When
            var actualValue = time.TotalMinutes;

            // Then
            Assert.AreEqual(expectedValue, actualValue, TimeInaccuracy);
        }

        [Test]
        public void ShouldReturnsTotalSeconds()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);
            var expectedValue = new TimeSpan(1, 2, 3, 4, 5).TotalSeconds;

            // When
            var actualValue = time.TotalSeconds;

            // Then
            Assert.AreEqual(expectedValue, actualValue, TimeInaccuracy);
        }

        [Test]
        public void ShouldReturnsTotalMilliseconds()
        {
            // Given
            var time = new Time(1, 2, 3, 4, 5);
            var expectedValue = new TimeSpan(1, 2, 3, 4, 5).TotalMilliseconds;

            // When
            var actualValue = time.TotalMilliseconds;

            // Then
            Assert.AreEqual(expectedValue, actualValue, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddTime()
        {
            // Given
            var time1 = new Time(0, 1, 2, 3, 4);
            var time2 = new Time(5, 6, 7, 8, 9);

            // When
            var result1 = time1.Add(time2);
            var result2 = time2.Add(time1);

            // Then
            Assert.AreEqual(time1.TotalSeconds + time2.TotalSeconds, result1.TotalSeconds, TimeInaccuracy);
            Assert.AreEqual(time2.TotalSeconds + time1.TotalSeconds, result2.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddDays()
        {
            // Given
            const double value = 123.456;
            var time = new Time(12345);

            // When
            var result = time.AddDays(value);

            // Then
            Assert.AreEqual(time.TotalSeconds + value * 24 * 60 * 60, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddHours()
        {
            // Given
            const double value = 123.456;
            var time = new Time(12345);

            // When
            var result = time.AddHours(value);

            // Then
            Assert.AreEqual(time.TotalSeconds + value * 60 * 60, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddMinutes()
        {
            // Given
            const double value = 123.456;
            var time = new Time(12345);

            // When
            var result = time.AddMinutes(value);

            // Then
            Assert.AreEqual(time.TotalSeconds + value * 60, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddSeconds()
        {
            // Given
            const double value = 123.456;
            var time = new Time(12345);

            // When
            var result = time.AddSeconds(value);

            // Then
            Assert.AreEqual(time.TotalSeconds + value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldAddMilliseconds()
        {
            // Given
            const double value = 123.456;
            var time = new Time(12345);

            // When
            var result = time.AddMilliseconds(value);

            // Then
            Assert.AreEqual(time.TotalSeconds + 0.001 * value, result.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldSubtractTime()
        {
            // Given
            var time1 = new Time(12345);
            var time2 = new Time(67890);

            // When
            var result1 = time1.Subtract(time2);
            var result2 = time2.Subtract(time1);

            // Then
            Assert.AreEqual(time1.TotalSeconds - time2.TotalSeconds, result1.TotalSeconds, TimeInaccuracy);
            Assert.AreEqual(time2.TotalSeconds - time1.TotalSeconds, result2.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldCompareTo()
        {
            // Given
            var time1 = new Time(1);
            var time2 = new Time(2);
            var time3 = new Time(3);

            // When & Then
            Assert.AreEqual(0, time1.CompareTo(time1));
            Assert.AreEqual(-1, time1.CompareTo(time2));
            Assert.AreEqual(-1, time1.CompareTo(time3));
            Assert.AreEqual(1, time2.CompareTo(time1));
            Assert.AreEqual(0, time2.CompareTo(time2));
            Assert.AreEqual(-1, time2.CompareTo(time3));
            Assert.AreEqual(1, time3.CompareTo(time1));
            Assert.AreEqual(1, time3.CompareTo(time2));
            Assert.AreEqual(0, time3.CompareTo(time3));
        }

        [Test]
        public void ShouldEquals()
        {
            // Given
            var time1 = new Time(1);
            var time2 = new Time(2);
            var time3 = new Time(3);

            // When & Then
            Assert.AreEqual(true, time1.Equals(time1));
            Assert.AreEqual(false, time1.Equals(time2));
            Assert.AreEqual(false, time1.Equals(time3));
            Assert.AreEqual(false, time2.Equals(time1));
            Assert.AreEqual(true, time2.Equals(time2));
            Assert.AreEqual(false, time2.Equals(time3));
            Assert.AreEqual(false, time3.Equals(time1));
            Assert.AreEqual(false, time3.Equals(time2));
            Assert.AreEqual(true, time3.Equals(time3));
        }

        [Test]
        public void ShouldParse()
        {
            // Given
            const string timeString = "123.12:34:56.789";
            var expectedTime = new Time(123, 12, 34, 56, 789);

            // When
            var time = Time.Parse(timeString);

            // Then
            Assert.AreEqual(expectedTime.TotalSeconds, time.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldTryParse()
        {
            // Given
            const string rightTimeString = "12:34";
            const string wrongTimeString = "12:34!";
            var expectedTime = new Time(12, 34, 00);

            // When

            Time rightTime;
            var rightResult = Time.TryParse(rightTimeString, out rightTime);

            Time wrongTime;
            var wrongResult = Time.TryParse(wrongTimeString, out wrongTime);

            // Then
            Assert.AreEqual(true, rightResult);
            Assert.AreEqual(false, wrongResult);
            Assert.AreEqual(expectedTime.TotalSeconds, rightTime.TotalSeconds);
        }

        [Test]
        public void ShouldToString()
        {
            // Given
            var time1 = new Time(0, 12, 34, 56, 789);
            var time2 = new Time(1, 12, 34, 56, 789);
            var time3 = new Time(-0, -12, -34, -56, -789);
            var time4 = new Time(-1, -12, -34, -56, -789);

            // When & Then
            Assert.AreEqual("12:34:56", time1.ToString());
            Assert.AreEqual("1.12:34:56", time2.ToString());
            Assert.AreEqual("-12:34:56", time3.ToString());
            Assert.AreEqual("-1.12:34:56", time4.ToString());
        }

        [Test]
        public void ShouldToShortTimeString()
        {
            // Given
            var time1 = new Time(0, 12, 34, 56, 789);
            var time2 = new Time(1, 12, 34, 56, 789);
            var time3 = new Time(-0, -12, -34, -56, -789);
            var time4 = new Time(-1, -12, -34, -56, -789);

            // When & Then
            Assert.AreEqual("12:34:56", time1.ToShortTimeString());
            Assert.AreEqual("1.12:34:56", time2.ToShortTimeString());
            Assert.AreEqual("-12:34:56", time3.ToShortTimeString());
            Assert.AreEqual("-1.12:34:56", time4.ToShortTimeString());
        }

        [Test]
        public void ShouldToLongTimeString()
        {
            // Given
            var time1 = new Time(0, 12, 34, 56, 789);
            var time2 = new Time(1, 12, 34, 56, 789);
            var time3 = new Time(-0, -12, -34, -56, -789);
            var time4 = new Time(-1, -12, -34, -56, -789);

            // When & Then
            Assert.AreEqual("12:34:56.7890000", time1.ToLongTimeString());
            Assert.AreEqual("1.12:34:56.7890000", time2.ToLongTimeString());
            Assert.AreEqual("-12:34:56.7890000", time3.ToLongTimeString());
            Assert.AreEqual("-1.12:34:56.7890000", time4.ToLongTimeString());
        }

        [Test]
        public void ShouldToStringFormat()
        {
            // Given
            const string format = @"hh\:mm";
            var time1 = new Time(0, 12, 34, 56, 789);

            // When & Then
            Assert.AreEqual("12:34", time1.ToString(format));
        }

        [Test]
        public void ShouldCastOperatorToDouble()
        {
            // Given
            const double totalSeconds = 12345.67890;

            // When
            var time1 = (Time)totalSeconds;
            var time2 = (double)time1;

            // Then
            Assert.AreEqual(totalSeconds, time1.TotalSeconds, TimeInaccuracy);
            Assert.AreEqual(totalSeconds, time2, TimeInaccuracy);
        }

        [Test]
        public void ShouldCastOperatorToTimeSpan()
        {
            // Given
            var timeSpan = TimeSpan.FromSeconds(12345.67890);

            // When
            var time1 = (Time)timeSpan;
            var time2 = (TimeSpan)time1;

            // Then
            Assert.AreEqual(timeSpan.TotalSeconds, time1.TotalSeconds, TimeInaccuracy);
            Assert.AreEqual(timeSpan.TotalSeconds, time2.TotalSeconds, TimeInaccuracy);
        }

        [Test]
        public void ShouldEqualOperatorsForTime()
        {
            // Given
            var time1 = new Time(1);
            var time2 = new Time(1);
            var time3 = new Time(2);

            // When & Then

            Assert.IsTrue(time1 == time2);
            Assert.IsFalse(time1 != time2);
            Assert.IsFalse(time1 == time3);
            Assert.IsTrue(time1 != time3);

            Assert.IsTrue(time2 == time1);
            Assert.IsFalse(time2 != time1);
            Assert.IsFalse(time2 == time3);
            Assert.IsTrue(time2 != time3);

            Assert.IsFalse(time3 == time1);
            Assert.IsTrue(time3 != time1);
            Assert.IsFalse(time3 == time2);
            Assert.IsTrue(time3 != time2);
        }

        [Test]
        public void ShouldEqualOperatorsForDouble()
        {
            // Given
            var time1 = new Time(12.34);
            var time2 = time1.TotalSeconds;
            var time3 = time1.TotalSeconds + 56.78;

            // When & Then

            Assert.IsTrue(time1 == time2);
            Assert.IsFalse(time1 != time2);
            Assert.IsFalse(time1 == time3);
            Assert.IsTrue(time1 != time3);

            Assert.IsTrue(time2 == time1);
            Assert.IsFalse(time2 != time1);

            Assert.IsFalse(time3 == time1);
            Assert.IsTrue(time3 != time1);
        }

        [Test]
        public void ShouldCompareOperatorsForTime()
        {
            // Given
            var time1 = new Time(1);
            var time2 = new Time(1);
            var time3 = new Time(2);

            // When & Then

            Assert.IsFalse(time1 < time2);
            Assert.IsTrue(time1 < time3);
            Assert.IsTrue(time1 <= time2);
            Assert.IsTrue(time1 <= time3);
            Assert.IsFalse(time1 > time2);
            Assert.IsFalse(time1 > time3);
            Assert.IsTrue(time1 >= time2);
            Assert.IsFalse(time1 >= time3);

            Assert.IsFalse(time2 < time1);
            Assert.IsTrue(time2 < time3);
            Assert.IsTrue(time2 <= time1);
            Assert.IsTrue(time2 <= time3);
            Assert.IsFalse(time2 > time1);
            Assert.IsFalse(time2 > time3);
            Assert.IsTrue(time2 >= time1);
            Assert.IsFalse(time2 >= time3);

            Assert.IsFalse(time3 < time1);
            Assert.IsFalse(time3 < time2);
            Assert.IsFalse(time3 <= time1);
            Assert.IsFalse(time3 <= time2);
            Assert.IsTrue(time3 > time1);
            Assert.IsTrue(time3 > time2);
            Assert.IsTrue(time3 >= time1);
            Assert.IsTrue(time3 >= time2);
        }

        [Test]
        public void ShouldCompareOperatorsForDouble()
        {
            // Given
            var time1 = new Time(12.34);
            var time2 = time1.TotalSeconds;
            var time3 = time1.TotalSeconds + 56.78;

            // When & Then

            Assert.IsFalse(time1 < time2);
            Assert.IsTrue(time1 < time3);
            Assert.IsTrue(time1 <= time2);
            Assert.IsTrue(time1 <= time3);
            Assert.IsFalse(time1 > time2);
            Assert.IsFalse(time1 > time3);
            Assert.IsTrue(time1 >= time2);
            Assert.IsFalse(time1 >= time3);

            Assert.IsFalse(time2 < time1);
            Assert.IsTrue(time2 < time3);
            Assert.IsTrue(time2 <= time1);
            Assert.IsTrue(time2 <= time3);
            Assert.IsFalse(time2 > time1);
            Assert.IsFalse(time2 > time3);
            Assert.IsTrue(time2 >= time1);
            Assert.IsFalse(time2 >= time3);

            Assert.IsFalse(time3 < time1);
            Assert.IsFalse(time3 < time2);
            Assert.IsFalse(time3 <= time1);
            Assert.IsFalse(time3 <= time2);
            Assert.IsTrue(time3 > time1);
            Assert.IsTrue(time3 > time2);
            Assert.IsTrue(time3 >= time1);
            Assert.IsTrue(time3 >= time2);
        }
    }
}