using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Versioning
{
	[Obsolete("Not actual!")]
	public sealed class VersionStrategy : IVersionStrategy
	{
		public string GetActualVersion(string metadataConfigurationId, IEnumerable<Tuple<string, string>> configurationVersions, string userName)
		{
			return null;
		}

		public IEnumerable<dynamic> GetIrrelevantVersionList(IEnumerable<Tuple<string, string>> configurationVersions, string userName)
		{
			return Enumerable.Empty<object>();
		}

		public void SetRelevantVersion(string version, string configurationId, string userName)
		{
		}
	}
}