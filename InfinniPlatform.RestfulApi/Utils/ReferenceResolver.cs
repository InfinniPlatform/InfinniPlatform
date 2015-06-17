﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.RestfulApi.Utils
{
	public sealed class ReferenceResolver : IReferenceResolver
	{
		private readonly IMetadataComponent _metadataComponent;

		public ReferenceResolver(IMetadataComponent metadataComponent)
		{
			_metadataComponent = metadataComponent;
		}

		public void ResolveReferences(string version, string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve)
		{
			var linkMap = new DocumentLinkMap(_metadataComponent);

			var metadataOperator = new MetadataOperator(_metadataComponent, linkMap, ignoreResolve);

			dynamic typeInfo = new DynamicWrapper();
			typeInfo.ConfigId = configId;
			typeInfo.DocumentId = documentId;


			if (documents is IEnumerable<dynamic>)
			{
				foreach (var doc in documents)
				{
					metadataOperator.ProcessMetadata(version,doc, typeInfo);
				}
			}
			else if (documents != null)
			{
				metadataOperator.ProcessMetadata(version,documents, typeInfo);
			}
			linkMap.ResolveLinks(version,typeInfo, metadataOperator.TypeInfoChain);
		}
	}
}
