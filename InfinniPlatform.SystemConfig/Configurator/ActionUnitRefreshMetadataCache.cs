using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.ContextComponents;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Обновление кэша метаданных при изменении объектов в хранилище
	/// </summary>
	public sealed class ActionUnitRefreshMetadataCache
	{
		public void Action(IApplyContext target)
		{
		    if (target.Item.IsElementDeleted != null &&
		        target.Item.IsElementDeleted == true)
		    {
				target.Context.GetComponent<IMetadataComponent>().DeleteMetadata(
		            target.Item.ConfigId,
		            target.Item.DocumentId,
		            target.Item.MetadataType,
		            target.Item.MetadataName);
		    }
		    else
		    {
				target.Context.GetComponent<IMetadataComponent>().UpdateMetadata(
		            target.Item.ConfigId,
		            target.Item.DocumentId,
		            target.Item.MetadataType,
		            target.Item.MetadataName);
		    }
		}
	}
}
