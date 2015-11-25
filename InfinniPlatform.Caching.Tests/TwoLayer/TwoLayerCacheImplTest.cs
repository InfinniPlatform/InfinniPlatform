using System;
using System.Threading;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.TwoLayer;

using NUnit.Framework;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class TwoLayerCacheImplTest
    {
        private TwoLayerCacheImpl _cache;
        private FakeCacheImpl _memoryCache;
        private FakeCacheImpl _sharedCache;
        private MemoryCacheMessageBusImpl _sharedCacheMessageBus;


        [SetUp]
        public void SetUp()
        {
            _memoryCache = new FakeCacheImpl();
            _sharedCache = new FakeCacheImpl();
            _sharedCacheMessageBus = new MemoryCacheMessageBusImpl();

            _cache = new TwoLayerCacheImpl(_memoryCache, _sharedCache, _sharedCacheMessageBus);
        }

        [TearDown]
        public void TearDown()
        {
            _cache.Dispose();

            _sharedCacheMessageBus.Dispose();
            _sharedCache.Dispose();
            _memoryCache.Dispose();
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

            _cache.Set(key, value);

            // When
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
        public void ContainsShouldReturnTrueWhenKeyExistsInSharedCacheOnly()
        {
            // Given
            const string key = "Contains_SharedKey";
            const string value = "Contains_SharedValue";

            _sharedCache.Set(key, value);

            // When
            var result = _cache.Contains(key);

            // Then
            Assert.IsTrue(result);
        }


        [Test]
        public void GetShouldReturnValueWhenKeyExists()
        {
            // Given
            const string key = "Get_ExistingKey";
            const string value = "Get_ExistingValue";

            _cache.Set(key, value);

            // When
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
        public void GetShouldReturnValueWhenKeyExistsInSharedCacheOnly()
        {
            // Given
            const string key = "Get_SharedKey";
            const string value = "Get_SharedValue";

            _sharedCache.Set(key, value);

            // When
            var result = _cache.Get(key);

            // Then
            Assert.AreEqual(value, result);
        }


        [Test]
        public void TryGetShouldReturnValueWhenKeyExists()
        {
            // Given
            const string key = "TryGet_ExistingKey";
            const string value = "TryGet_ExistingValue";

            _cache.Set(key, value);

            // When
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
        public void TryGetShouldReturnValueWhenKeyExistsInSharedCacheOnly()
        {
            // Given
            const string key = "TryGet_SharedKey";
            const string value = "TryGet_SharedValue";

            _sharedCache.Set(key, value);

            // When
            string result;
            var exists = _cache.TryGet(key, out result);

            // Then
            Assert.IsTrue(exists);
            Assert.AreEqual(value, result);
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
            var resultMemory = _memoryCache.Get(key);
            var resultShared = _sharedCache.Get(key);

            // Then
            Assert.AreEqual(value, result);
            Assert.AreEqual(value, resultMemory);
            Assert.AreEqual(value, resultShared);
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
            var resultMemory = _memoryCache.Get(key);
            var resultShared = _sharedCache.Get(key);

            // Then
            Assert.AreEqual(value2, result);
            Assert.AreEqual(value2, resultMemory);
            Assert.AreEqual(value2, resultShared);
        }

        [Test]
        public void SetShoudPublishMessage()
        {
            // Given

            const string key = "Set_PublishKey";
            const string value = "Set_PublishValue";

            var published = new AutoResetEvent(false);
            _sharedCacheMessageBus.Subscribe(key, (k, v) => published.Set());

            // When
            _cache.Set(key, value);

            // Then
            Assert.IsTrue(published.WaitOne(5000));
        }


        [Test]
        public void RemoveShouldDeleteValueWhenKeyExists()
        {
            // Given
            const string key = "Remove_ExistingKey";
            const string value = "Remove_ExistingValue";

            _cache.Set(key, value);

            // When

            _cache.Remove(key);

            var result = _cache.Contains(key);
            var resultMemory = _memoryCache.Contains(key);
            var resultShared = _sharedCache.Contains(key);

            // Then
            Assert.IsFalse(result);
            Assert.IsFalse(resultShared);
            Assert.IsFalse(resultMemory);
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
        public void RemoveShoudPublishMessage()
        {
            // Given

            const string key = "Remove_PublishKey";
            const string value = "Remove_PublishValue";

            _cache.Set(key, value);

            var published = new AutoResetEvent(false);
            _sharedCacheMessageBus.Subscribe(key, (k, v) => published.Set());

            // When
            _cache.Remove(key);

            // Then
            Assert.IsTrue(published.WaitOne(5000));
        }


        [Test]
        public void ClearShouldDeleteAllKeysFromCache()
        {
            // Given
            const string key1 = "Clear_Key1";
            const string key2 = "Clear_Key2";
            const string key3 = "Clear_Key3";
            const string value1 = "Clear_Value1";
            const string value2 = "Clear_Value1";
            const string value3 = "Clear_Value1";

            _cache.Set(key1, value1);
            _cache.Set(key2, value2);
            _cache.Set(key3, value3);

            // When

            _cache.Clear();

            var result1 = _cache.Contains(key1);
            var resultMemory1 = _memoryCache.Contains(key1);
            var resultShared1 = _sharedCache.Contains(key1);
            var result2 = _cache.Contains(key2);
            var resultMemory2 = _memoryCache.Contains(key2);
            var resultShared2 = _sharedCache.Contains(key2);
            var result3 = _cache.Contains(key3);
            var resultMemory3 = _memoryCache.Contains(key3);
            var resultShared3 = _sharedCache.Contains(key3);

            // Then
            Assert.IsFalse(result1);
            Assert.IsFalse(resultMemory1);
            Assert.IsFalse(resultShared1);
            Assert.IsFalse(result2);
            Assert.IsFalse(resultMemory2);
            Assert.IsFalse(resultShared2);
            Assert.IsFalse(result3);
            Assert.IsFalse(resultMemory3);
            Assert.IsFalse(resultShared3);
        }

        [Test]
        [Ignore("Manual")]
        public void ClearShouldShoudPublishMessages()
        {
            // Given

            const string key1 = "Clear_PublishKey1";
            const string key2 = "Clear_PublishKey2";
            const string key3 = "Clear_PublishKey3";
            const string value1 = "Clear_PublishValue1";
            const string value2 = "Clear_PublishValue1";
            const string value3 = "Clear_PublishValue1";

            _cache.Set(key1, value1);
            _cache.Set(key2, value2);
            _cache.Set(key3, value3);

            var published = new CountdownEvent(3);
            _sharedCacheMessageBus.Subscribe(key1, (k, v) => published.Signal());
            _sharedCacheMessageBus.Subscribe(key2, (k, v) => published.Signal());
            _sharedCacheMessageBus.Subscribe(key3, (k, v) => published.Signal());

            // When

            _cache.Clear();

            // Then

            Assert.IsTrue(published.Wait(5000));
        }
    }
}