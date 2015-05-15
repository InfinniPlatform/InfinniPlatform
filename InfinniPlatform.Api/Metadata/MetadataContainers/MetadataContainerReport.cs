using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
	public sealed class MetadataContainerReport : IMetadataContainerInfo
	{
		public string GetMetadataContainerName()
		{
			return MetadataType.ReportContainer;
		}

		public string GetMetadataTypeName()
		{
			return MetadataType.Report;
		}
	}
}
