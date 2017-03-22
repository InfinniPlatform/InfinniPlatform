using System.IO;

using InfinniPlatform.Core.Compression;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Compression
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class GZipDataCompressorTest
    {
        [Test]
        public void CompressAndDecompressShouldBeAssociativeOperations()
        {
            // Given

            var compressor = new GZipDataCompressor();
            var originalData = new MemoryStream(new byte[] { 1, 2, 3 });

            // When

            var compressSource1 = originalData;
            var compressDestination1 = new MemoryStream();
            compressor.Compress(compressSource1, compressDestination1);

            var decompressSource1 = compressDestination1;
            var decompressDestination1 = new MemoryStream();
            compressor.Decompress(decompressSource1, decompressDestination1);

            var compressSource2 = decompressDestination1;
            var compressDestination2 = new MemoryStream();
            compressor.Compress(compressSource2, compressDestination2);

            var decompressSource2 = compressDestination2;
            var decompressDestination2 = new MemoryStream();
            compressor.Decompress(decompressSource2, decompressDestination2);

            // Then

            CollectionAssert.AreEquivalent(originalData.ToArray(), decompressDestination1.ToArray());
            CollectionAssert.AreEquivalent(originalData.ToArray(), decompressDestination2.ToArray());
            CollectionAssert.AreEquivalent(compressDestination1.ToArray(), compressDestination2.ToArray());
        }


        [Test]
        public void CanCompressEmptyStream()
        {
            // Given

            var source = new MemoryStream();
            var compressor = new GZipDataCompressor();

            // When

            var destination = new MemoryStream();

            TestDelegate compressEmptyStream
                = () => compressor.Compress(source, destination);

            // Then

            Assert.DoesNotThrow(compressEmptyStream);
            Assert.AreEqual(0, destination.Length);
        }

        [Test]
        public void CompressShouldNotCloseSourceStream()
        {
            // Given

            var source = new MemoryStream(new byte[] { 1, 2, 3 });
            var compressor = new GZipDataCompressor();

            // When

            TestDelegate compressSourceStreamTwice
                = () =>
                      {
                          var destination1 = new MemoryStream();
                          compressor.Compress(source, destination1);

                          var destination2 = new MemoryStream();
                          compressor.Compress(source, destination2);
                      };

            // Then

            Assert.DoesNotThrow(compressSourceStreamTwice);
        }

        [Test]
        public void CompressResultShouldBeImmutable()
        {
            // Given

            var source = new MemoryStream(new byte[] { 1, 2, 3 });
            var compressor = new GZipDataCompressor();

            // When

            var destination1 = new MemoryStream();
            compressor.Compress(source, destination1);

            var destination2 = new MemoryStream();
            compressor.Compress(source, destination2);

            // Then

            CollectionAssert.AreEquivalent(destination1.ToArray(), destination2.ToArray());
        }

        [Test]
        public void CompressShouldNotCloseDestinationStream()
        {
            // Given

            var source = new MemoryStream(new byte[] { 1, 2, 3 });
            var compressor = new GZipDataCompressor();

            // When

            var destination = new MemoryStream();
            compressor.Compress(source, destination);

            // Then

            Assert.IsTrue(destination.CanSeek);
            Assert.IsTrue(destination.CanRead);
            Assert.IsTrue(destination.CanWrite);
        }


        [Test]
        public void CanDecompressEmptyStream()
        {
            // Given

            var source = new MemoryStream();
            var compressor = new GZipDataCompressor();

            // When

            var destination = new MemoryStream();

            TestDelegate decompressEmptyStream
                = () => compressor.Decompress(source, destination);

            // Then

            Assert.DoesNotThrow(decompressEmptyStream);
            Assert.AreEqual(0, destination.Length);
        }

        [Test]
        public void DecompressShouldNotCloseSourceStream()
        {
            // Given

            var source = new MemoryStream();
            var compressor = new GZipDataCompressor();

            // When

            TestDelegate decompressSourceStreamTwice
                = () =>
                      {
                          var destination1 = new MemoryStream();
                          compressor.Decompress(source, destination1);

                          var destination2 = new MemoryStream();
                          compressor.Decompress(source, destination2);
                      };

            // Then

            Assert.DoesNotThrow(decompressSourceStreamTwice);
        }

        [Test]
        public void DecompressResultShouldBeImmutable()
        {
            // Given

            var source = new MemoryStream();
            var compressor = new GZipDataCompressor();

            // When

            compressor.Compress(new MemoryStream(new byte[] { 1, 2, 3 }), source);
            source.Position = 0;

            var destination1 = new MemoryStream();
            compressor.Decompress(source, destination1);

            var destination2 = new MemoryStream();
            compressor.Decompress(source, destination2);

            // Then

            CollectionAssert.AreEquivalent(destination1.ToArray(), destination2.ToArray());
        }

        [Test]
        public void DecompressShouldNotCloseDestinationStream()
        {
            // Given

            var source = new MemoryStream();
            var compressor = new GZipDataCompressor();

            // When

            compressor.Compress(new MemoryStream(new byte[] { 1, 2, 3 }), source);
            source.Position = 0;

            var destination = new MemoryStream();
            compressor.Decompress(source, destination);

            // Then

            Assert.IsTrue(destination.CanSeek);
            Assert.IsTrue(destination.CanRead);
            Assert.IsTrue(destination.CanWrite);
        }
    }
}