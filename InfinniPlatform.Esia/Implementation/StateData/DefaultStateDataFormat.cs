using System;
using System.Runtime.Caching;

using Microsoft.Owin.Security;

namespace InfinniPlatform.Esia.Implementation.StateData
{
	sealed class DefaultStateDataFormat : ISecureDataFormat<AuthenticationProperties>
	{
		private static readonly MemoryCache AuthenticationProperties = new MemoryCache("Esia");
		private static readonly TimeSpan AuthenticationTimeout = TimeSpan.FromMinutes(5);


		public string Protect(AuthenticationProperties properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			var protectedText = Guid.NewGuid().ToString("N");

			AuthenticationProperties.Add(protectedText, properties, DateTimeOffset.Now + AuthenticationTimeout);

			return protectedText;
		}

		public AuthenticationProperties Unprotect(string protectedText)
		{
			if (!string.IsNullOrEmpty(protectedText))
			{
				var properties = AuthenticationProperties.Remove(protectedText) as AuthenticationProperties;

				return properties;
			}

			return null;
		}
	}
}