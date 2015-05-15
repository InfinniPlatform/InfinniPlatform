using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using InfinniPlatform.Owin.Properties;

namespace InfinniPlatform.Owin
{
	public static class OwinExtensions
	{
		/// <summary>
		/// Определяет, является ли адрес локальным.
		/// </summary>
		public static bool IsLocalAddress(this string address, out string normalizeAddress)
		{
			var result = false;

			normalizeAddress = address;

			if (!string.IsNullOrWhiteSpace(address))
			{
				try
				{
					var hostIPs = Dns.GetHostAddresses(address);
					var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

					if (hostIPs != null)
					{
						foreach (var hostIp in hostIPs)
						{
							// localhost
							if (IPAddress.IsLoopback(hostIp))
							{
								normalizeAddress = "+";
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


		private static readonly Guid ApplicationId = new Guid("{a6c54cea-f380-42d3-b1d5-d71d2de1a02f}");

		/// <summary>
		/// Осуществляет привязку сертификата к порту.
		/// </summary>
		public static void BindCertificate(int port, string certificate)
		{
			if (port <= 0)
			{
				throw new ArgumentOutOfRangeException(string.Format(Resources.ServerPortIsIncorrect, port));
			}

			if (string.IsNullOrWhiteSpace(certificate))
			{
				throw new ArgumentNullException(Resources.ServerCertificateCannotBeNullOrWhiteSpace);
			}

			var endPoint = new IPEndPoint(IPAddress.Any, port);

			try
			{
				var normalizeCertificate = certificate.Trim().Replace(" ", "");
				var certificateBytes = Enumerable.Range(0, normalizeCertificate.Length).Where(i => i % 2 == 0).Select(i => Convert.ToByte(normalizeCertificate.Substring(i, 2), 16)).ToArray();

				NativeHttpApi.CreateBindCertificate(endPoint, certificateBytes, StoreName.My, ApplicationId);
			}
			catch (Exception error)
			{
				throw new InvalidOperationException(string.Format(Resources.ServerCertificateCannotBindOnPort, certificate, port), error);
			}
		}
	}
}