using System;
using System.Security.Cryptography.X509Certificates;

using GostCryptography.Cryptography;

using InfinniPlatform.Esia.Properties;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	sealed class DataDecryptorFactory : IDataDecryptorFactory
	{
		public IDataDecryptor CreateDecryptor(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}

			if (!certificate.HasPrivateKey)
			{
				throw new ArgumentException(string.Format(Resources.CertificateDoesNotHavePrivateKey, certificate.Thumbprint), "certificate");
			}

			var privateKeyInfo = certificate.GetPrivateKeyInfo();

			if (privateKeyInfo != null)
			{
				if (privateKeyInfo.ProviderType == ProviderTypes.CryptoPro)
				{
					return new DataDecryptorCryptoPro();
				}

				if (privateKeyInfo.ProviderType == ProviderTypes.VipNet)
				{
					return new DataDecryptorVipNet();
				}
			}

			return new DataDecryptorDefault();
		}
	}
}