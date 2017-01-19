using System;

using Microsoft.AspNetCore.DataProtection;

namespace InfinniPlatform.Auth.Cookie.DataProtectors
{
    internal class AesDataProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector CreateProtector(string purpose)
        {
            if (purpose == null)
            {
                throw new ArgumentNullException();
            }

            return new AesDataProtector(purpose);
        }
    }
}