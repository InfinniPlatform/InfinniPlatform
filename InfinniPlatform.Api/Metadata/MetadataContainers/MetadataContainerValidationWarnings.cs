﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Metadata.MetadataContainers
{
	public sealed class MetadataContainerValidationWarnings : IMetadataContainerInfo
	{
		public string GetMetadataContainerName()
		{
			return MetadataType.ValidationWarningsContainer;
		}

		public string GetMetadataTypeName()
		{
			return MetadataType.ValidationWarning;
		}
	}
}
