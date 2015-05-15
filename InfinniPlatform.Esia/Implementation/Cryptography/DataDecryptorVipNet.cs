using System;
using System.Xml;

using GostCryptography.Xml;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	sealed class DataDecryptorVipNet : IDataDecryptor
	{
		public void DecryptDocument(XmlDocument document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			var encryptedXml = new GostEncryptedXml(document);
			encryptedXml.DecryptDocument();
		}
	}
}