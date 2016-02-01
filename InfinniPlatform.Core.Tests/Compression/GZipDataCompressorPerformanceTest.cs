using System;
using System.Diagnostics;
using System.IO;

using InfinniPlatform.Core.Compression;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Compression
{
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	public sealed class GZipDataCompressorPerformanceTest
	{
		[Test]
		[TestCase(1024, 10000, Description = "1 Kb")]
		[TestCase(1024 * 1024, 100, Description = "1 Mb")]
		public void CompressPerformanceTest(long size, int iterations)
		{
			// Given

			var compressor = new GZipDataCompressor();

			var stopwatch = new Stopwatch();

			var compressSource = new byte[size];
			new Random(DateTime.Now.Millisecond).NextBytes(compressSource);
			var compressSourceStream = new MemoryStream(compressSource);

			// When

			compressor.Compress(new MemoryStream(), new MemoryStream()); // JIT

			for (var i = 0; i < iterations; ++i)
			{
				using (var compressResultStream = new MemoryStream())
				{
					stopwatch.Start();

					compressor.Compress(compressSourceStream, compressResultStream);

					stopwatch.Stop();
				}
			}

			// Then

			Console.WriteLine("Size: {0:N0} Kb", size / 1024.0);
			Console.WriteLine("Iterations: {0} times", iterations);
			Console.WriteLine("Compress speed: {0:N4} Kbytes/sec", (size * iterations / 1024.0) / stopwatch.Elapsed.TotalSeconds);
		}

		[Test]
		[TestCase(1024, 10000, Description = "1 Kb")]
		[TestCase(1024 * 1024, 100, Description = "1 Mb")]
		public void DecompressPerformanceTest(long size, int iterations)
		{
			// Given

			var compressor = new GZipDataCompressor();

			var stopwatch = new Stopwatch();

			var compressSource = new byte[size];
			new Random(DateTime.Now.Millisecond).NextBytes(compressSource);
			var compressSourceStream = new MemoryStream(compressSource);
			var decompressSourceStream = new MemoryStream();
			compressor.Compress(compressSourceStream, decompressSourceStream);

			// When

			compressor.Decompress(new MemoryStream(), new MemoryStream()); // JIT

			for (var i = 0; i < iterations; ++i)
			{
				using (var decompressResultStream = new MemoryStream())
				{
					stopwatch.Start();

					compressor.Decompress(decompressSourceStream, decompressResultStream);

					stopwatch.Stop();
				}
			}

			// Then

			Console.WriteLine("Size: {0:N0} Kb", size / 1024.0);
			Console.WriteLine("Iterations: {0} times", iterations);
			Console.WriteLine("Compress speed: {0:N4} Kbytes/sec", (size * iterations / 1024.0) / stopwatch.Elapsed.TotalSeconds);
		}
	}
}