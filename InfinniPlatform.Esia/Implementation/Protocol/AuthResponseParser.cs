using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

using InfinniPlatform.Esia.Implementation.Cryptography;
using InfinniPlatform.Esia.Implementation.DataEncoding;
using InfinniPlatform.Esia.Properties;

using Microsoft.Owin;
using Microsoft.Owin.Helpers;

namespace InfinniPlatform.Esia.Implementation.Protocol
{
	sealed class AuthResponseParser
	{
		public AuthResponseParser(X509Certificate2 clientSecretCert)
			: this(clientSecretCert, new DataEncoder(), new DataDecryptorFactory())
		{
		}

		public AuthResponseParser(X509Certificate2 clientSecretCert, IDataEncoder dataEncoder, IDataDecryptorFactory dataDecryptorFactory)
		{
			if (clientSecretCert == null)
			{
				throw new ArgumentNullException("clientSecretCert");
			}

			if (dataEncoder == null)
			{
				throw new ArgumentNullException("dataEncoder");
			}

			if (dataDecryptorFactory == null)
			{
				throw new ArgumentNullException("dataDecryptorFactory");
			}


			_dataEncoder = dataEncoder;
			_dataDecryptor = dataDecryptorFactory.CreateDecryptor(clientSecretCert);
		}


		private readonly IDataEncoder _dataEncoder;
		private readonly IDataDecryptor _dataDecryptor;


		public ICollection<KeyValuePair<string, string>> GetAuthenticatedUserInfo(IOwinRequest request, out string state)
		{
			var authResponse = ReadResponseBody(request);
			var samlResponse = ReadSamlResponse(authResponse);

			state = ReadRelayState(authResponse);
			return ReadUserClaims(samlResponse);
		}


		private static IDictionary<string, string> ReadResponseBody(IOwinRequest response)
		{
			string formText;

			// Чтение текста запроса
			using (var reader = new StreamReader(response.Body, Encoding.UTF8))
			{
				formText = reader.ReadToEnd();
			}

			// Разбор текста запроса
			IFormCollection formCollection = WebHelpers.ParseForm(formText);

			// Преобразование в объект
			var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach (var item in formCollection)
			{
				if (item.Value != null && item.Value.Length > 0)
				{
					result[item.Key] = item.Value[0];
				}
			}

			return result;
		}

		private XmlDocument ReadSamlResponse(IDictionary<string, string> response)
		{
			string responseEncode;

			if (!response.TryGetValue("SAMLResponse", out responseEncode))
			{
				throw new InvalidOperationException(Resources.SamlResponseNotFound);
			}

			var responseBytes = _dataEncoder.DecodeBytes(responseEncode);

			using (var responseStream = new MemoryStream(responseBytes))
			{
				var responseXml = new XmlDocument { PreserveWhitespace = true };
				responseXml.Load(responseStream);

				_dataDecryptor.DecryptDocument(responseXml);

				return responseXml;
			}
		}

		private string ReadRelayState(IDictionary<string, string> response)
		{
			string relayStateEncode;

			if (!response.TryGetValue("RelayState", out relayStateEncode))
			{
				throw new InvalidOperationException(Resources.RelayStateNotFound);
			}

			return _dataEncoder.DecodeString(relayStateEncode);
		}

		private static ICollection<KeyValuePair<string, string>> ReadUserClaims(XmlDocument samlResponse)
		{
			var claims = new List<KeyValuePair<string, string>>();

			var namespaceManager = new XmlNamespaceManager(samlResponse.NameTable);
			namespaceManager.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");

			var claimNodes = samlResponse.SelectNodes("//saml2:Attribute", namespaceManager);

			if (claimNodes != null)
			{
				foreach (XmlNode claimNode in claimNodes)
				{
					if (claimNode.ChildNodes.Count > 0)
					{
						var claimName = GetAttributeValue(claimNode, "FriendlyName");
						var claimValues = ReadClaimValues(claimName, claimNode.ChildNodes[0]);

						if (claimValues != null)
						{
							claims.AddRange(claimValues);
						}
					}
				}
			}

			return claims;
		}

		private static IEnumerable<KeyValuePair<string, string>> ReadClaimValues(string claimName, XmlNode claimValueNode)
		{
			var claimValues = new List<KeyValuePair<string, string>>();

			if (IsSimpleClaimValue(claimValueNode))
			{
				var value = GetSimpleClaimValue(claimValueNode);

				if (!string.IsNullOrWhiteSpace(value))
				{
					claimValues.Add(new KeyValuePair<string, string>(claimName, value));
				}
			}
			else
			{
				var values = GetComplexClaimValues(claimValueNode);

				foreach (var value in values)
				{
					claimValues.Add(new KeyValuePair<string, string>(claimName + "Code", value.Key));
					claimValues.Add(new KeyValuePair<string, string>(claimName + "Name", value.Value));
				}
			}

			return claimValues;
		}

		private static bool IsSimpleClaimValue(XmlNode attributeValue)
		{
			var valueType = GetAttributeValue(attributeValue, "xsi:type");

			return (string.IsNullOrEmpty(valueType) || string.Equals(valueType, "xs:string", StringComparison.OrdinalIgnoreCase));
		}

		private static string GetSimpleClaimValue(XmlNode attributeValue)
		{
			return attributeValue.InnerText;
		}

		private static IEnumerable<KeyValuePair<string, string>> GetComplexClaimValues(XmlNode attributeValue)
		{
			var codeNodes = attributeValue.SelectNodes(".//code");

			if (codeNodes != null)
			{
				foreach (XmlNode codeNode in codeNodes)
				{
					var code = codeNode.InnerText;

					if (!string.IsNullOrWhiteSpace(code))
					{
						var name = GetChildNodeText(codeNode.ParentNode, "name");
						yield return new KeyValuePair<string, string>(code, name);
					}
				}
			}
		}

		private static string GetAttributeValue(XmlNode parentNode, string attributeName)
		{
			if (parentNode.Attributes != null)
			{
				var attribute = parentNode.Attributes[attributeName];

				if (attribute != null)
				{
					return attribute.Value;
				}
			}

			return null;
		}

		private static string GetChildNodeText(XmlNode parentNode, string childNodeName)
		{
			if (parentNode != null)
			{
				var childNode = parentNode.SelectSingleNode(childNodeName);

				if (childNode != null)
				{
					return childNode.InnerText;
				}
			}

			return null;
		}
	}
}