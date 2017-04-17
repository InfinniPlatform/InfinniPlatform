using System.Net;
using System.Threading.Tasks;

using InfinniPlatform.Core.Abstractions.Http;

namespace InfinniPlatform.Core.Http
{
    internal class HostAddressParser : IHostAddressParser
    {
        public async Task<bool> IsLocalAddress(string hostNameOrAddress)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(hostNameOrAddress))
            {
                try
                {
                    var hostIPs = await Dns.GetHostAddressesAsync(hostNameOrAddress);
                    var localIPs = await Dns.GetHostAddressesAsync(Dns.GetHostName());

                    if (hostIPs != null)
                    {
                        foreach (var hostIp in hostIPs)
                        {
                            // localhost
                            if (IPAddress.IsLoopback(hostIp))
                            {
                                hostNameOrAddress = "+";
                                result = true;
                                break;
                            }

                            if (localIPs != null)
                            {
                                foreach (var localIp in localIPs)
                                {
                                    // local
                                    if (hostIp.Equals(localIp))
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return result;
        }
    }
}