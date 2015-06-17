using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.Generators
{
	/// <summary>
	///   Удаление генератора метаданных
	/// </summary>
	public sealed class ActionUnitDeleteGenerator
	{
		public void Action(IApplyContext target)
		{
			//удаляем сгенерированный для генератора автоматически сервис
			var body = new
						{
                            ActionName = target.Item.GeneratorName,
							Configuration = target.Item.Configuration,
							Metadata = target.Item.Metadata,
						};

            RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "deletegeneratedservice", null, target.Item, target.Version);

			//удаляем метаданные самого генератора
			var generatorManager = new ManagerFactoryDocument(target.Version, target.Item.Configuration, target.Item.Metadata).BuildGeneratorManager();

		    var generator = generatorManager.MetadataReader.GetItem(target.Item.GeneratorName);

		    if (generator != null)
		    {
		        generatorManager.DeleteItem(generator);
		    }

		}
	}
}
