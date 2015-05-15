namespace InfinniPlatform.Esia.Implementation.DataEncoding
{
	interface IDataEncoder
	{
		string EncodeBytes(byte[] data);
		string EncodeString(string data);
		byte[] DecodeBytes(string data);
		string DecodeString(string data);
	}
}