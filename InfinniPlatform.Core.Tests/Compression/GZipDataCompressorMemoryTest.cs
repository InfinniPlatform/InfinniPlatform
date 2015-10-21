using System;
using System.IO;

using InfinniPlatform.Compression;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Compression
{
    [TestFixture]
    [Category(TestCategories.MemoryLeakTest)]
    [Ignore("Manual")]
    public sealed class GZipDataCompressorMemoryTest
    {
        [Test]
        [TestCase(1024, 10000, Description = "1 Kb")]
        [TestCase(1024 * 1024, 100, Description = "1 Mb")]
        public void CompressMemoryTest(long size, int iterations)
        {
            // Given

            var compressor = new GZipDataCompressor();

            var compressSource = new byte[size];
            new Random(DateTime.Now.Millisecond).NextBytes(compressSource);
            var compressSourceStream = new MemoryStream(compressSource);

            // When

            var memoryBefore = GC.GetTotalMemory(true);

            for (var i = 0; i < iterations; ++i)
            {
                using (var compressResultStream = new MemoryStream())
                {
                    compressor.Compress(compressSourceStream, compressResultStream);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memoryAfter = GC.GetTotalMemory(true);

            // Then

            var memoryLeak = memoryAfter - memoryBefore;
            var memeryLeakFactor = (double)memoryLeak / size;

            Console.WriteLine(@"Size: {0:N0} Kb", size / 1024.0);
            Console.WriteLine(@"Iterations: {0} times", iterations);
            Console.WriteLine(@"Memory leak: {0:N4} Kb", memoryLeak / 1024.0);
            Console.WriteLine(@"Memory leak factor: {0:N4}", memeryLeakFactor);

            Assert.Less(memeryLeakFactor, 2.5);
        }


        [Test]
        [TestCase(1024, 10000, Description = "1 Kb")]
        [TestCase(1024 * 1024, 100, Description = "1 Mb")]
        public void DecompressMemoryTest(long size, int iterations)
        {
            // Given

            var compressor = new GZipDataCompressor();

            var compressSource = new byte[size];
            new Random(DateTime.Now.Millisecond).NextBytes(compressSource);
            var compressSourceStream = new MemoryStream(compressSource);
            var decompressSourceStream = new MemoryStream();
            compressor.Compress(compressSourceStream, decompressSourceStream);

            // When

            var memoryBefore = GC.GetTotalMemory(true);

            for (var i = 0; i < iterations; ++i)
            {
                using (var decompressResultStream = new MemoryStream())
                {
                    compressor.Decompress(decompressSourceStream, decompressResultStream);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memoryAfter = GC.GetTotalMemory(true);

            // Then

            var memoryLeak = memoryAfter - memoryBefore;
            var memeryLeakFactor = (double)memoryLeak / size;

            Console.WriteLine(@"Size: {0:N0} Kb", size / 1024.0);
            Console.WriteLine(@"Iterations: {0} times", iterations);
            Console.WriteLine(@"Memory leak: {0:N4} Kb", memoryLeak / 1024.0);
            Console.WriteLine(@"Memory leak factor: {0:N4}", memeryLeakFactor);

            Assert.Less(memeryLeakFactor, 2.5);
        }
    }
}