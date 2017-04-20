using System;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.Settings;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.Cache.TwoLayer
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class TwoLayerCacheImplTest
    {
        private TwoLayerCache _cache;
        private FakeCacheImpl _memoryCache;
        private FakeCacheImpl _sharedCache;
        private Mock<IBroadcastProducer> _broadcastProducerMock;

        [SetUp]
        public void SetUp()
        {
            var appOptions = new AppOptions();

            _memoryCache = new FakeCacheImpl();
            _sharedCache = new FakeCacheImpl();
            _broadcastProducerMock = new Mock<IBroadcastProducer>();
            
            _cache = new TwoLayerCache(_memoryCache, _sharedCache, appOptions, _broadcastProducerMock.Object, new Mock<ILog>().Object);
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

            // When
            _cache.Set(key, value);

            // Then
            _broadcastProducerMock.Verify(producer => producer.PublishAsync(key, nameof(TwoLayerCache)), Times.Once);
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

            // When
            _cache.Remove(key);

            // Then
            _broadcastProducerMock.Verify(producer => producer.PublishAsync(key, nameof(TwoLayerCache)), Times.Exactly(2));
        }
    }
}