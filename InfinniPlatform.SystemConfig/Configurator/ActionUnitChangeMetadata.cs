using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///  Регистрация новой  конфигурации
    /// </summary>
    public sealed class ActionUnitChangeMetadata
    {
        public void Action(IApplyContext target)
        {
            var result = target.Context.GetComponent<DocumentApi>(target.Version).SetDocument(target.Configuration, target.Metadata, target.Item);
	        
			target.Result = new DynamicWrapper();
	        target.Result.Name = target.Item.Name;
	        target.Result.Id = target.Id;
	        target.Result.IsValid = result.IsValid;
	        target.Result.ValidationMessage = result.ValidationMessage;

        }
    }
}
