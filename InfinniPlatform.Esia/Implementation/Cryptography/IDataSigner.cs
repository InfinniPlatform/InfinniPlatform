namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	interface IDataSigner
	{
		string SignatureAlgorithm { get; }

		byte[] CreateSignature(string data);
	}
}