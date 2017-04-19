using System;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Cache
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public sealed class RedisCacheImplTest
    {
        private RedisSharedCache _cache;

        [SetUp]
        public void SetUp()
        {
            var appOptions = new AppOptions { AppName = nameof(RedisCacheImplTest) };

            var settings = new RedisSharedCacheOptions
            {
                Host = "localhost",
                Password = "TeamCity"
            };

            var log = new Mock<ILog>().Object;
            var performanceLog = new Mock<IPerformanceLog>().Object;

            _cache = new RedisSharedCache(appOptions, new RedisConnectionFactory(settings), log, performanceLog);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldThrowExceptionWhenKeyIsNullOrEmpty(string key)
        {
            Assert.Throws<ArgumentNullException>(() => _cache.Contains(key));
            Assert.Throws<ArgumentNullException>(() => _cache.Get(key));
            Assert.Throws<ArgumentNullException>(() => _cache.Set(key, "value"));
            Assert.Throws<ArgumentNullException>(() => _cache.Remove(key));
        }

        [Test]
        public void SetShouldThrowExceptionWhenValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _cache.Set("ValueNull", null));
        }


        [Test]
        public void ContainsShouldReturnTrueWhenKeyExists()
        {
            // Given
            const string key = "Contains_ExistingKey";
            const string value = "Contains_ExistingValue";

            // When
            _cache.Set(key, value);
            var result = _cache.Contains(key);

            // Then
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsShouldReturnFalseWhenKeyDoesNotExists()
        {
            // Given
            const string key = "Contains_NonExistingKey";

            // When
            var result = _cache.Contains(key);

            // Then
            Assert.IsFalse(result);
        }


        [Test]
        public void GetShouldReturnValueWhenKeyExists()
        {
            // Given
            const string key = "Get_ExistingKey";
            const string value = "Get_ExistingValue";

            // When
            _cache.Set(key, value);
            var result = _cache.Get(key);

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void GetShouldReturnNullWhenKeyDoesNotExists()
        {
            // Given
            const string key = "Get_NonExistingKey";

            // When
            var result = _cache.Get(key);

            // Then
            Assert.IsNull(result);
        }


        [Test]
        public void TryGetShouldReturnValueWhenKeyExists()
        {
            // Given
            const string key = "TryGet_ExistingKey";
            const string value = "TryGet_ExistingValue";

            // When
            _cache.Set(key, value);
            string result;
            var exists = _cache.TryGet(key, out result);

            // Then
            Assert.IsTrue(exists);
            Assert.AreEqual(value, result);
        }

        [Test]
        public void TryGetShouldReturnNullWhenKeyDoesNotExists()
        {
            // Given
            const string key = "TryGet_NonExistingKey";

            // When
            string result;
            var exists = _cache.TryGet(key, out result);

            // Then
            Assert.IsFalse(exists);
            Assert.IsNull(result);
        }


        [Test]
        public void SetShouldSaveValue()
        {
            // Given
            const string key = "Set_SomeKey";
            const string value = "Set_SomeValue";

            // When
            _cache.Set(key, value);
            var result = _cache.Get(key);

            // Then
            Assert.AreEqual(value, result);
        }

        [Test]
        public void SetShouldReplaceValue()
        {
            // Given
            const string key = "Set_ReplaceKey";
            const string value1 = "Set_ReplaceValue1";
            const string value2 = "Set_ReplaceValue2";

            // When
            _cache.Set(key, value1);
            _cache.Set(key, value2);
            var result = _cache.Get(key);

            // Then
            Assert.AreEqual(value2, result);
        }


        [Test]
        public void RemoveShouldDeleteValueWhenKeyExists()
        {
            // Given
            const string key = "Remove_ExistingKey";
            const string value = "Remove_ExistingValue";

            // When
            _cache.Set(key, value);
            _cache.Remove(key);
            var result = _cache.Contains(key);

            // Then
            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveShouldDoNothingWhenKeyExists()
        {
            // Given
            const string key = "Remove_NonExistingKey";

            // When
            _cache.Remove(key);
            var result = _cache.Contains(key);

            // Then
            Assert.IsFalse(result);
        }


        [Test]
        [Ignore("TODO: It not works")]
        public void ClearShouldDeleteAllKeysFromCache()
        {
            // Given
            const string key1 = "Clear_Key1";
            const string key2 = "Clear_Key2";
            const string key3 = "Clear_Key3";
            const string value1 = "Clear_Value1";
            const string value2 = "Clear_Value1";
            const string value3 = "Clear_Value1";

            // When
            _cache.Set(key1, value1);
            _cache.Set(key2, value2);
            _cache.Set(key3, value3);
            _cache.Clear();
            var result1 = _cache.Contains(key1);
            var result2 = _cache.Contains(key2);
            var result3 = _cache.Contains(key3);

            // Then
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }
    }
}