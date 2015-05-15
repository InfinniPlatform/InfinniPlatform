using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using InfinniPlatform.Esia.Implementation.Compression;
using InfinniPlatform.Esia.Implementation.Cryptography;
using InfinniPlatform.Esia.Implementation.DataEncoding;

namespace InfinniPlatform.Esia.Implementation.Protocol
{
	sealed class AuthRequestBuilder
	{
		public AuthRequestBuilder(string serverUri, string clientId, X509Certificate2 clientSecretCert)
			: this(serverUri, clientId, clientSecretCert, new DataCompressor(), new DataEncoder(), new DataSignerFactory())
		{
		}

		public AuthRequestBuilder(string serverUri, string clientId, X509Certificate2 clientSecretCert, IDataCompressor dataCompressor, IDataEncoder dataEncoder, IDataSignerFactory dataSignerFactory)
		{
			if (string.IsNullOrWhiteSpace(serverUri))
			{
				throw new ArgumentNullException("serverUri");
			}

			if (string.IsNullOrWhiteSpace(clientId))
			{
				throw new ArgumentNullException("clientId");
			}

			if (clientSecretCert == null)
			{
				throw new ArgumentNullException("clientSecretCert");
			}

			if (dataCompressor == null)
			{
				throw new ArgumentNullException("dataCompressor");
			}

			if (dataEncoder == null)
			{
				throw new ArgumentNullException("dataEncoder");
			}

			if (dataSignerFactory == null)
			{
				throw new ArgumentNullException("dataSignerFactory");
			}

			_clientId = clientId;
			_serverUri = serverUri.TrimEnd('/');
			_dataCompressor = dataCompressor;
			_dataEncoder = dataEncoder;
			_dataSigner = dataSignerFactory.CreateSigner(clientSecretCert);
		}


		private readonly string _clientId;
		private readonly string _serverUri;
		private readonly IDataCompressor _dataCompressor;
		private readonly IDataEncoder _dataEncoder;
		private readonly IDataSigner _dataSigner;


		public string BuildAuthEndpoint(string state, string callbackUri)
		{
			var sigAlg = BuildSignatureAlgorithm();
			var samlRequest = BuildSamlRequest(callbackUri);
			var relayState = BuildRelayState(state);

			var sigData = string.Format("SAMLRequest={0}&SigAlg={1}&RelayState={2}", samlRequest, sigAlg, relayState);
			var signature = BuildSignature(sigData);

			return string.Format(@"{0}/SAML_SSO/?{1}&Signature={2}", _serverUri, sigData, signature);
		}


		private string BuildSamlRequest(string callbackUri)
		{
			var requestString = new StringBuilder("<saml2p:AuthnRequest xmlns:saml2p=\"urn:oasis:names:tc:SAML:2.0:protocol\"")
				.AppendAttribute("AssertionConsumerServiceURL", callbackUri)
				.AppendAttribute("Destination", _serverUri + "/SAML_SSO")
				.AppendAttribute("ForceAuthn", "false")
				.AppendAttribute("ID", Guid.NewGuid().ToString("N"))
				.AppendAttribute("IsPassive", "false")
				.AppendAttribute("IssueInstant", DateTime.UtcNow.ToString("O"))
				.AppendAttribute("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST")
				.AppendAttribute("Version", "2.0")
				.Append(">")
				.Append("<saml2:Issuer xmlns:saml2=\"urn:oasis:names:tc:SAML:2.0:assertion\">")
				.Append(_clientId)
				.Append("</saml2:Issuer>")
				.Append("</saml2p:AuthnRequest>")
				.ToString();

			var requestCompress = _dataCompressor.Compress(requestString);
			var requestEncode = _dataEncoder.EncodeBytes(requestCompress);

			return requestEncode;
		}

		private string BuildSignatureAlgorithm()
		{
			var signatureAlgorithm = _dataSigner.SignatureAlgorithm;
			var signatureAlgorithmEncode = _dataEncoder.EncodeString(signatureAlgorithm);

			return signatureAlgorithmEncode;
		}

		private string BuildSignature(string data)
		{
			var dataSignature = _dataSigner.CreateSignature(data);
			var dataSignatureEncode = _dataEncoder.EncodeBytes(dataSignature);

			return dataSignatureEncode;
		}

		private string BuildRelayState(string state)
		{
			var relayStateEncode = _dataEncoder.EncodeString(state);

			return relayStateEncode;
		}
	}
}