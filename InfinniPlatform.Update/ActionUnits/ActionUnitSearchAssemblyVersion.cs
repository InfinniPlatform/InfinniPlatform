using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Update.ActionUnits
{
	public sealed class ActionUnitSearchAssemblyVersion
	{
		public void Action(ISearchContext target)
		{
			var blobStorage = target.Context.GetComponent<IBlobStorageComponent>(target.Version).GetBlobStorage();

			foreach (var result in target.SearchResult)
			{
				var contentId = result.ContentId;

				if (contentId != null)
				{
					var assembly = blobStorage.GetBlobData(new Guid(contentId));
					result.Assembly = (assembly != null) ? assembly.Data : null;
				}

				var pdbId = result.PdbId;

				if (pdbId != null)
				{
					var pdb = blobStorage.GetBlobData(new Guid(pdbId));
					result.Pdb = (pdb != null) ? pdb.Data : null;
				}

				result.ContentId = null;
				result.PdbId = null;
			}
		}
	}
}