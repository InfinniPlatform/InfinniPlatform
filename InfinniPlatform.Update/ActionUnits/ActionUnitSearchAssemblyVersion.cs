using System;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Update.ActionUnits
{
	public sealed class ActionUnitSearchAssemblyVersion
	{
		public void Action(ISearchContext target)
		{
			var blobStorage = target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage();

			foreach (var result in target.SearchResult)
			{
				string contentId = result.ContentId;

				if (contentId != null)
				{
					var assembly = blobStorage.GetBlobData(contentId);
					result.Assembly = (assembly != null) ? assembly.Data : null;
				}

				string pdbId = result.PdbId;

				if (pdbId != null)
				{
					var pdb = blobStorage.GetBlobData(pdbId);
					result.Pdb = (pdb != null) ? pdb.Data : null;
				}

				result.ContentId = null;
				result.PdbId = null;
			}
		}
	}
}