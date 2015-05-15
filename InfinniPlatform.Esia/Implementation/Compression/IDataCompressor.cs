namespace InfinniPlatform.Esia.Implementation.Compression
{
	interface IDataCompressor
	{
		byte[] Compress(string data);
	}
}