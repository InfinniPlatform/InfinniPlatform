using System;

using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Authentication.DataProtectors
{
    internal sealed class AesDataProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            if (purposes == null)
            {
                throw new ArgumentNullException();
            }

            var key = string.Join(";", purposes);

            return new AesDataProtector(key);
        }
    }
}