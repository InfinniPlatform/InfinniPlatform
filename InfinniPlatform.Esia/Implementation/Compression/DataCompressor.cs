using System.IO;
using System.IO.Compression;
using System.Text;

namespace InfinniPlatform.Esia.Implementation.Compression
{
	sealed class DataCompressor : IDataCompressor
	{
		private static readonly Encoding DataEncoding = Encoding.UTF8;

		public byte[] Compress(string data)
		{
			using (var destinationStream = new MemoryStream())
			{
				using (var sourceStream = new MemoryStream(DataEncoding.GetBytes(data)))
				{
					using (var compressionStream = new DeflateStream(destinationStream, CompressionLevel.Fastest, true))
					{
						sourceStream.CopyTo(compressionStream);
					}
				}

				return destinationStream.ToArray();
			}
		}
	}
}