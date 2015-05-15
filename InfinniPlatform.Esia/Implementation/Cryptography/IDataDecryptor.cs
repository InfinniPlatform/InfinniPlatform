using System.Xml;

namespace InfinniPlatform.Esia.Implementation.Cryptography
{
	interface IDataDecryptor
	{
		void DecryptDocument(XmlDocument document);
	}
}