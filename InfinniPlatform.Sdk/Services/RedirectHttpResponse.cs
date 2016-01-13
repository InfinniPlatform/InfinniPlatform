using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Services
{
    public sealed class RedirectHttpResponse : HttpResponse
    {
        public RedirectHttpResponse(string location, RedirectType type = RedirectType.SeeOther)
        {
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                      {
                          { "Location", location }
                      };

            StatusCode = (int)type;
        }


        public enum RedirectType
        {
            Permanent = 301,
            Temporary = 307,
            SeeOther = 303
        }
    }
}