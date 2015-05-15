using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Api.TestEnvironment
{
	/// <summary>
	/// Отслеживает экземпляры тестовых серверов в границах текущего домена приложения.
	/// </summary>
	static class TestServerTracker
	{
		private static readonly List<TestServerInfo> Instances = new List<TestServerInfo>();


		public static void AddTrackedServer(TestServerInfo instance)
		{
			Instances.Add(instance);
		}

		public static void RemoveTrackedServer(TestServerInfo instance)
		{
			Instances.Remove(instance);
		}

		public static TestServerInfo FindTrackedServer(HostingConfig hostingConfig)
		{
			return Instances.FirstOrDefault(i => EquasHostingConfig(i.HostingConfig, hostingConfig));
		}


		private static bool EquasHostingConfig(HostingConfig left, HostingConfig right)
		{
			return ReferenceEquals(left, right)
				   || (left != null && right != null
					   && string.Equals(left.ServerScheme, right.ServerScheme, StringComparison.OrdinalIgnoreCase)
					   && string.Equals(left.ServerName, right.ServerName, StringComparison.OrdinalIgnoreCase)
					   && string.Equals(left.ServerPort, right.ServerPort));
		}
	}
}