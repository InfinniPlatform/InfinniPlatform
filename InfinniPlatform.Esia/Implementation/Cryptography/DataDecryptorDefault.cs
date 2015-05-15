using System;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	sealed class DataDecryptorDefault : IDataDecryptor
	{
		public void DecryptDocument(XmlDocument document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}

			var encryptedXml = new EncryptedXml(document);
			encryptedXml.DecryptDocument();
		}
	}
}