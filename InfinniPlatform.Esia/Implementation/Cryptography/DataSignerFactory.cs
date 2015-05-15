using System;
using System.Security.Cryptography.X509Certificates;

using GostCryptography.Cryptography;

using InfinniPlatform.Esia.Properties;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	sealed class DataSignerFactory : IDataSignerFactory
	{
		public IDataSigner CreateSigner(X509Certificate2 certificate)
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
					return new DataSignerCryptoPro(certificate);
				}

				if (privateKeyInfo.ProviderType == ProviderTypes.VipNet)
				{
					return new DataSignerVipNet(certificate);
				}
			}

			return new DataSignerDefault(certificate);
		}
	}
}