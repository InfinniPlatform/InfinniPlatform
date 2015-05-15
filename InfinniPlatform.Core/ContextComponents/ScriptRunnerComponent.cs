using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.ContextComponents
{
	/// <summary>
	///   Исполнитель скриптов из глобального контекста
	/// </summary>
	public sealed class ScriptRunnerComponent : IScriptRunnerComponent
	{
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

		private Dictionary<string, IScriptProcessor> _scriptProcessors = new Dictionary<string, IScriptProcessor>();

		public ScriptRunnerComponent(IMetadataConfigurationProvider metadataConfigurationProvider)
		{
			_metadataConfigurationProvider = metadataConfigurationProvider;
		}

		/// <summary>
		///   Получить исполнителя скриптов для указанного идентификатора метаданных конфигурации
		/// </summary>
		/// <param name="configurationId">Идентификатор метаданных конфигурации</param>
		/// <returns>Исполнитель скриптов</returns>
		public IScriptProcessor GetScriptRunner(string configurationId)
		{
			if (_scriptProcessors.ContainsKey(configurationId))
			{
				return _scriptProcessors[configurationId];
			}

			var scriptProcessor = _metadataConfigurationProvider.GetMetadataConfiguration(configurationId).ScriptConfiguration.GetScriptProcessor();

			_scriptProcessors.Add(configurationId, scriptProcessor);

			return scriptProcessor;
		}
	}
}
