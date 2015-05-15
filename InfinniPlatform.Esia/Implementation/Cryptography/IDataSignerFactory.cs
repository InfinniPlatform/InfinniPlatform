using System.Security.Cryptography.X509Certificates;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	interface IDataSignerFactory
	{
		IDataSigner CreateSigner(X509Certificate2 certificate);
	}
}