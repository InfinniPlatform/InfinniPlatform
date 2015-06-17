using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Удаление объекта метаданных 
	/// </summary>
	public sealed class ActionUnitDeleteMetadata
	{
		public void Action(IApplyContext target)
		{
			target.Context.GetComponent<DocumentApi>(target.Version).DeleteDocument("systemconfig", target.Metadata, target.Item.Id);

			target.Result = new DynamicWrapper();
			target.Result.Name = target.Item.Name;
			target.Result.IsValid = true;
			target.Result.ValidationMessage = Resources.MetadataObjectDeleted;

		}
	}
}
