using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Authentication.DataProtectors
{
	sealed class AesSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
	{
		private readonly TicketSerializer _serializer;
		private readonly IDataProtector _protector;
		private readonly ITextEncoder _encoder;

		public AesSecureDataFormat(string key)
		{
			_serializer = new TicketSerializer();
			_protector = new AesDataProtector(key);
			_encoder = TextEncodings.Base64Url;
		}

		public string Protect(AuthenticationTicket ticket)
		{
			var ticketData = _serializer.Serialize(ticket);
			var protectedData = _protector.Protect(ticketData);
			var protectedString = _encoder.Encode(protectedData);
			return protectedString;
		}

		public AuthenticationTicket Unprotect(string text)
		{
			var protectedData = _encoder.Decode(text);
			var ticketData = _protector.Unprotect(protectedData);
			var ticket = _serializer.Deserialize(ticketData);
			return ticket;
		}
	}
}