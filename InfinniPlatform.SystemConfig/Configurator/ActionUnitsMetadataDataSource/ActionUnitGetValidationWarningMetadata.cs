﻿using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
	public sealed class ActionUnitGetValidationWarningMetadata
	{
		public void Action(IApplyResultContext target)
		{
			dynamic bodyQuery = new DynamicWrapper();
			bodyQuery.ConfigId = target.Item.ConfigId;
			bodyQuery.DocumentId = target.Item.DocumentId;
			bodyQuery.MetadataType = MetadataType.ValidationWarning;
			bodyQuery.MetadataName = target.Item.MetadataName;

			target.Result = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getdocumentelementmetadata", null, bodyQuery, target.Version).ToDynamic();
		}
	}
}
