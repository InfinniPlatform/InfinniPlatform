using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
	public sealed class MetadataContainerValidationErrors : IMetadataContainerInfo
	{
		public string GetMetadataContainerName()
		{
			return MetadataType.ValidationErrorsContainer;
		}

		public string GetMetadataTypeName()
		{
			return MetadataType.ValidationError;
		}
	}
}
