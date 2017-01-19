using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace InfinniPlatform.Auth.Cookie.DataProtectors
{
    internal class AesSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly IDataProtector _protector;
        private readonly TicketSerializer _serializer;

        public AesSecureDataFormat(string key)
        {
            _serializer = new TicketSerializer();
            _protector = new AesDataProtector(key);
        }


        public string Protect(AuthenticationTicket ticket)
        {
            var ticketData = _serializer.Serialize(ticket);
            var protectedData = _protector.Protect(ticketData);
            var protectedString = Encode(protectedData);
            return protectedString;
        }

        public AuthenticationTicket Unprotect(string text)
        {
            var protectedData = Decode(text);
            var ticketData = _protector.Unprotect(protectedData);
            var ticket = _serializer.Deserialize(ticketData);
            return ticket;
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            return Protect(data);
        }

        public AuthenticationTicket Unprotect(string text, string purpose)
        {
            return Unprotect(text);
        }

        private static string Encode(byte[] data)
        {
            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private static byte[] Decode(string text)
        {
            return Convert.FromBase64String(Pad(text.Replace('-', '+').Replace('_', '/')));
        }

        private static string Pad(string text)
        {
            var count = 3 - (text.Length + 3) % 4;

            if (count == 0)
            {
                return text;
            }

            return text + new string('=', count);
        }
    }
}