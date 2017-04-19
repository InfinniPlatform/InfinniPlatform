using System;
using System.Collections.Generic;

namespace InfinniPlatform.Core.Http
{
    /// <summary>
    /// Ответ перенаправления.
    /// </summary>
    public sealed class RedirectHttpResponse : HttpResponse
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="location">Адрес назначения.</param>
        /// <param name="type">Тип перенаправления.</param>
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