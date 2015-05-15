using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using InfinniPlatform.Esia.Properties;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	sealed class DataSignerCryptoPro : IDataSigner
	{
		public DataSignerCryptoPro(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}

			if (!certificate.HasPrivateKey)
			{
				throw new InvalidOperationException(string.Format(Resources.CertificateDoesNotHavePrivateKey, _certificate.Thumbprint));
			}

			_certificate = certificate;
		}


		private readonly X509Certificate2 _certificate;


		public string SignatureAlgorithm
		{
			get { return "http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411"; }
		}


		public byte[] CreateSignature(string data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			// Инфорация о подписи
			var privateKey = _certificate.PrivateKey;
			var signatureDescription = CreateSignatureDescription(privateKey);

			// Вычисление хэша данных
			var hashAlgorithm = CreateHashAlgorithm(signatureDescription);
			var dataBytes = Encoding.UTF8.GetBytes(data);
			var dataHash = hashAlgorithm.ComputeHash(dataBytes);
			hashAlgorithm.Dispose();

			// Вычисление подписи по хэшу
			var asymmetricSignatureFormatter = signatureDescription.CreateFormatter(privateKey);
			asymmetricSignatureFormatter.SetHashAlgorithm(signatureDescription.DigestAlgorithm);
			var dataSignature = asymmetricSignatureFormatter.CreateSignature(dataHash);

			return dataSignature;
		}


		private SignatureDescription CreateSignatureDescription(AsymmetricAlgorithm signatureKey)
		{
			var signatureDescription = CryptoConfig.CreateFromName(signatureKey.SignatureAlgorithm) as SignatureDescription;

			if (signatureDescription == null)
			{
				throw new InvalidOperationException(string.Format(Resources.SignatureDescriptionNotCreated, _certificate.Thumbprint, signatureKey.SignatureAlgorithm));
			}

			return signatureDescription;
		}

		private HashAlgorithm CreateHashAlgorithm(SignatureDescription signatureDescription)
		{
			var hashAlgorithm = signatureDescription.CreateDigest();

			if (hashAlgorithm == null)
			{
				throw new InvalidOperationException(string.Format(Resources.CreateHashAlgorithmFailed, _certificate.Thumbprint, signatureDescription.DigestAlgorithm));
			}

			return hashAlgorithm;
		}
	}
}