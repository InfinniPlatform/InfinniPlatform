using System.Security.Cryptography.X509Certificates;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	interface IDataDecryptorFactory
	{
		IDataDecryptor CreateDecryptor(X509Certificate2 certificate);
	}
}