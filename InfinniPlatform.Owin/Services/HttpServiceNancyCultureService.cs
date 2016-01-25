using System.Globalization;
using System.Threading;

using Nancy;
using Nancy.Culture;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Сервис определения <see cref="CultureInfo"/> текущего запроса.
    /// </summary>
    /// <remarks>
    /// По каким-то причинам <see cref="DefaultCultureService"/> под Mono не работает.
    /// </remarks>
    internal sealed class HttpServiceNancyCultureService : ICultureService
    {
        public CultureInfo DetermineCurrentCulture(NancyContext context)
        {
            return Thread.CurrentThread.CurrentCulture;
        }
    }
}