using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Authentication.DataProtectors
{
    internal sealed class AesSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public AesSecureDataFormat(string key)
        {
            _serializer = new TicketSerializer();
            _protector = new AesDataProtector(key);
            _encoder = TextEncodings.Base64Url;
        }

        private readonly ITextEncoder _encoder;
        private readonly IDataProtector _protector;
        private readonly TicketSerializer _serializer;

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